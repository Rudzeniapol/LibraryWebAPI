using LibraryAPI.Data;
using LibraryAPI.Repositories;
using LibraryAPI.Repositories.Interfaces;
using LibraryAPI.Services;
using LibraryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LibraryAPI;
using LibraryAPI.DTOs;
using LibraryAPI.Middlewares;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing in appsettings.json");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {   
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = key
        };
        options.MapInboundClaims = false;
    });
    
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
    options.AddPolicy("AllUsers", policy => policy.RequireRole("user", "admin"));
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите JWT-токен"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddAutoMapper(typeof(AuthorMappingProfile));
builder.Services.AddAutoMapper(typeof(BookMappingProfile));
builder.Services.AddAutoMapper(typeof(UserMappingProfile));

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