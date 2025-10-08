using Microsoft.OpenApi.Models;
using Microsoft.Data.SqlClient;
using IBOWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// === (1) ���J�]�w�ӷ� ===
// appsettings.json + appsettings.{Environment}.json + �����ܼ�
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// === (2) �A�ȵ��U ===
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IBOWebAPI", Version = "v1" });
});

// ADO.NET �s�u�u�t
builder.Services.AddScoped<Func<SqlConnection>>(sp =>
{
    var cs = sp.GetRequiredService<IConfiguration>().GetConnectionString("Default")
             ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");
    return () => new SqlConnection(cs);
});

// �@�� CRUD �A��
builder.Services.AddScoped<CrudService>();

var app = builder.Build();

// === (3) Middlewares ===
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
