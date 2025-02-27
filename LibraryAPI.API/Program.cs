using LibraryAPI.API.Data;
using LibraryAPI.API.Repositories;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.API.Services;
using LibraryAPI.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LibraryAPI.API;
using LibraryAPI.API.DTOs;
using LibraryAPI.API.Extentions;
using LibraryAPI.API.Middlewares;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDatabaseContext(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureDependencyInjection();
builder.Services.ConfigureAutoMapper();

builder.Services.AddMemoryCache();
builder.Services.AddAuthorization(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    await db.Database.MigrateAsync();
}

//if (app.Environment.IsDevelopment())
//
app.UseSwagger();
app.UseSwaggerUI();
//}

//UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();
app.Run();