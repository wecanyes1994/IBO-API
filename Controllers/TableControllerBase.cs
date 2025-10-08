// Controllers/Base/TableControllerBase.cs
using IBOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IBOWebAPI.Controllers.Base;

public abstract class TableControllerBase : ControllerBase
{
    protected readonly CrudService _svc;
    protected abstract string TableName { get; }

    protected TableControllerBase(CrudService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 50, [FromQuery] string? keyword = null)
        => Ok(await _svc.ListAsync(TableName, page, pageSize, keyword));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
        => (await _svc.GetOneAsync(TableName, id)) is { } row ? Ok(row) : NotFound();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] JsonElement body)
        => await _svc.CreateAsync(TableName, body)
           ? Ok(new { ok = true })
           : BadRequest("Insert failed");

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] JsonElement body)
        => await _svc.UpdateAsync(TableName, id, body) ? NoContent() : NotFound();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
        => await _svc.DeleteAsync(TableName, id) ? NoContent() : NotFound();
}
