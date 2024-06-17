using Microsoft.EntityFrameworkCore;
using api_server.Data;
using System.Security.Cryptography;
using api_server.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
int sessionTimeOutMinutes = 15;
builder.Configuration.GetSection("SessionTimeOutMinutes").Bind(sessionTimeOutMinutes);
builder.Services.AddSession(option => {
    option.IdleTimeout = TimeSpan.FromMinutes(sessionTimeOutMinutes);
    option.Cookie.IsEssential = true;
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ApplicationDatabase");
builder.Services.AddDbContext<ProjectContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseSession();

app.UseAuthorization();

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/customer"), appBuilder =>
{
    appBuilder.UseMiddleware<AuthMiddleware>();
});

app.MapControllers();

app.Run();
