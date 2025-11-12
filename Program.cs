// Program.cs － Render 相容版（HTTP 綁定 $PORT、停用生產環境的 HTTPS 轉導）
using Microsoft.OpenApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.HttpOverrides;   // ? ForwardedHeaders
using IBOWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// === (1) 設定來源：appsettings + 環境變數 ===
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// === (2) DI 註冊 ===
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

// 讓 ASP.NET Core 知道會在 Proxy 後面（Render 會帶 X-Forwarded-*）
builder.Services.Configure<ForwardedHeadersOptions>(opt =>
{
    opt.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

// === (3) Middlewares ===

// ? Render 外層已做 SSL，容器內請不要強制轉 https（否則會看到 HttpsRedirection 警告）
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection(); // 只在本機/開發環境啟用
}

app.UseForwardedHeaders();     // 正確還原 Request.Scheme/ClientIP（在 Proxy 後面很重要）

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// === (4) 綁定 Render 指派的動態埠 ===
// Render 會注入環境變數 PORT（例如 10000）；本機沒有時預設 5000
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://+:{port}");

app.Run();
