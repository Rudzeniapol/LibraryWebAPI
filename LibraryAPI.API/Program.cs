using LibraryAPI.Persistence.Data;
using LibraryAPI.Persistence.Repositories;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Persistence.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LibraryAPI.API;
using LibraryAPI.Application.DTOs;
using LibraryAPI.API.Extentions;
using LibraryAPI.API.Middlewares;
using LibraryAPI.Application.Commands.Book;
using LibraryAPI.Application.Exceptions;
using LibraryAPI.Application.Services.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDatabaseContext(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureDependencyInjection();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureValidation();
builder.Services.ConfigureRequestServices();

builder.Services.AddMemoryCache();
builder.Services.AddAuthorization(); 
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    await initializer.InitializeAsync();
}   


//if (app.Environment.IsDevelopment())
//
app.UseSwagger();
app.UseSwaggerUI();
//}

//UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();