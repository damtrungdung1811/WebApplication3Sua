using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApplication3.Areas.Identity.Data;
using WebApplication3.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Kết nối Database qlbt6 (SQL Server)
var connectionString = builder.Configuration.GetConnectionString("WebApplication3ContextConnection")
    ?? throw new InvalidOperationException("Connection string 'WebApplication3ContextConnection' not found.");

builder.Services.AddDbContext<WebApplication3Context>(options =>
    options.UseSqlServer(connectionString));

// 🔹 Nếu bạn cần đăng nhập với Identity
builder.Services.AddDefaultIdentity<WebApplication3User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Không bắt xác thực email
})
.AddEntityFrameworkStores<WebApplication3Context>();

// 🔹 Thêm Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CoreApi",
        Version = "v1",
        Description = "API Quản lý bảo trì/bảo hành thiết bị"
    });
});

var app = builder.Build();

// 🔹 HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreApi v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // ✅ Chỉ map API Controller, không còn Razor Pages

app.Run();
