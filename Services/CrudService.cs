// Services/CrudService.cs
using IBOWebAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace IBOWebAPI.Services;

public sealed class CrudService
{
    private readonly Func<SqlConnection> _connFactory;

    public CrudService(Func<SqlConnection> connFactory) => _connFactory = connFactory;

    public async Task<IEnumerable<Dictionary<string, object?>>> ListAsync(string table, int page, int pageSize, string? keyword)
    {
        var cfg = GetCfg(table);
        if (page <= 0) page = 1;
        if (pageSize <= 0 || pageSize > 500) pageSize = 50;
        var offset = (page - 1) * pageSize;

        using var conn = _connFactory();
        await conn.OpenAsync();

        var where = "";
        var cmd = new SqlCommand { Connection = conn };

        if (!string.IsNullOrWhiteSpace(keyword) && cfg.SearchColumns is { Length: > 0 })
        {
            var likeConds = cfg.SearchColumns.Select((c, i) => $"{c} LIKE @kw{i}");
            where = "WHERE " + string.Join(" OR ", likeConds);
            for (int i = 0; i < cfg.SearchColumns.Length; i++)
                cmd.Parameters.Add(new SqlParameter($"@kw{i}", "%" + keyword + "%"));
        }

        var order = string.IsNullOrWhiteSpace(cfg.DefaultOrderBy) ? cfg.KeyColumn : cfg.DefaultOrderBy;
        var selectCols = string.Join(",", cfg.Columns);

        cmd.CommandText = $@"
            SELECT {selectCols}
            FROM {cfg.TableName}
            {where}
            ORDER BY {order}
            OFFSET @offset ROWS FETCH NEXT @size ROWS ONLY;";

        cmd.Parameters.Add(new SqlParameter("@offset", SqlDbType.Int) { Value = offset });
        cmd.Parameters.Add(new SqlParameter("@size", SqlDbType.Int) { Value = pageSize });

        var list = new List<Dictionary<string, object?>>();
        using var rd = await cmd.ExecuteReaderAsync();
        while (await rd.ReadAsync())
            list.Add(ReadRow(rd, cfg));

        return list;
    }

    public async Task<Dictionary<string, object?>?> GetOneAsync(string table, string id)
    {
        var cfg = GetCfg(table);
        using var conn = _connFactory();
        await conn.OpenAsync();

        var selectCols = string.Join(",", cfg.Columns);
        using var cmd = new SqlCommand($@"
            SELECT TOP 1 {selectCols}
            FROM {cfg.TableName}
            WHERE {cfg.KeyColumn}=@id;", conn);
        cmd.Parameters.Add(new SqlParameter("@id", id));

        using var rd = await cmd.ExecuteReaderAsync();
        if (!await rd.ReadAsync()) return null;
        return ReadRow(rd, cfg);
    }

    public async Task<bool> CreateAsync(string table, JsonElement body)
    {
        var cfg = GetCfg(table);
        var data = JsonToDictionary(body);

        var cols = data.Keys.Where(c => cfg.Columns.Contains(c, StringComparer.OrdinalIgnoreCase)).ToArray();
        if (!cols.Contains(cfg.KeyColumn, StringComparer.OrdinalIgnoreCase))
            throw new InvalidOperationException($"Missing key column: {cfg.KeyColumn}");
        if (cols.Length == 0) return false;

        using var conn = _connFactory();
        await conn.OpenAsync();

        var colList = string.Join(",", cols);
        var paramList = string.Join(",", cols.Select((c, i) => $"@p{i}"));
        using var cmd = new SqlCommand($@"INSERT INTO {cfg.TableName} ({colList}) VALUES ({paramList});", conn);
        AddParams(cmd, cols, data);
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> UpdateAsync(string table, string id, JsonElement body)
    {
        var cfg = GetCfg(table);
        var data = JsonToDictionary(body);

        var cols = data.Keys.Where(c => !c.Equals(cfg.KeyColumn, StringComparison.OrdinalIgnoreCase)
                                     && cfg.Columns.Contains(c, StringComparer.OrdinalIgnoreCase)).ToArray();
        if (cols.Length == 0) return false;

        using var conn = _connFactory();
        await conn.OpenAsync();

        var setClause = string.Join(",", cols.Select((c, i) => $"{c}=@p{i}"));
        using var cmd = new SqlCommand($@"UPDATE {cfg.TableName} SET {setClause} WHERE {cfg.KeyColumn}=@id;", conn);
        AddParams(cmd, cols, data);
        cmd.Parameters.Add(new SqlParameter("@id", id));
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(string table, string id)
    {
        var cfg = GetCfg(table);
        using var conn = _connFactory();
        await conn.OpenAsync();

        using var cmd = new SqlCommand($@"DELETE FROM {cfg.TableName} WHERE {cfg.KeyColumn}=@id;", conn);
        cmd.Parameters.Add(new SqlParameter("@id", id));
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    // ---------- helpers ----------
    private static TableConfig GetCfg(string table)
        => TableRegistry.Tables.TryGetValue(table, out var cfg)
           ? cfg
           : throw new KeyNotFoundException($"Unknown table: {table}");

    private static Dictionary<string, object?> JsonToDictionary(JsonElement body)
    {
        var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var p in body.EnumerateObject())
        {
            dict[p.Name] = p.Value.ValueKind switch
            {
                JsonValueKind.String => p.Value.GetString(),
                JsonValueKind.Number => p.Value.TryGetInt64(out var l) ? l :
                                        p.Value.TryGetDecimal(out var m) ? m :
                                        p.Value.TryGetDouble(out var d) ? d : p.Value.GetRawText(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => p.Value.GetRawText()
            };
        }
        return dict;
    }

    private static void AddParams(SqlCommand cmd, string[] cols, Dictionary<string, object?> data)
    {
        for (int i = 0; i < cols.Length; i++)
        {
            var name = cols[i];
            data.TryGetValue(name, out var v);
            cmd.Parameters.AddWithValue($"@p{i}", v ?? DBNull.Value);
        }
    }

    private static Dictionary<string, object?> ReadRow(SqlDataReader rd, TableConfig cfg)
    {
        var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < rd.FieldCount; i++)
        {
            var col = rd.GetName(i);
            var val = rd.IsDBNull(i) ? null : rd.GetValue(i);

            if (val is DateTime dt && cfg.DateTimeColumns?.Contains(col, StringComparer.OrdinalIgnoreCase) == true)
                row[col] = dt.ToString("yyyy-MM-dd HH:mm:ss");
            else
                row[col] = val;
        }
        return row;
    }
}
