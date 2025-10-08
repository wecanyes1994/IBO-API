using Microsoft.OpenApi.Models;
using Microsoft.Data.SqlClient;
using IBOWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// === (1) 載入設定來源 ===
// appsettings.json + appsettings.{Environment}.json + 環境變數
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// === (2) 服務註冊 ===
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IBOWebAPI", Version = "v1" });
});

// ADO.NET 連線工廠
builder.Services.AddScoped<Func<SqlConnection>>(sp =>
{
    var cs = sp.GetRequiredService<IConfiguration>().GetConnectionString("Default")
             ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");
    return () => new SqlConnection(cs);
});

// 共用 CRUD 服務
builder.Services.AddScoped<CrudService>();

var app = builder.Build();

// === (3) Middlewares ===
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
