using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryAPI.API.Validators;
using LibraryAPI.Application.Commands.Author;
using LibraryAPI.Application.Commands.Book;
using LibraryAPI.Application.Commands.Token;
using LibraryAPI.Application.Commands.User;
using LibraryAPI.Persistence.Data;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Queries.Author;
using LibraryAPI.Application.Queries.Book;
using LibraryAPI.Application.Queries.Notification;
using LibraryAPI.Application.Queries.User;
using LibraryAPI.Persistence.Repositories;
using LibraryAPI.Application.Services;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Domain.Models;
using LibraryAPI.Persistence.Services;
using LibraryAPI.Persistence.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LibraryAPI.API.Extentions;

public static class ServiceExtentions
{
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing in appsettings.json");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {   
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = key
                };
                options.MapInboundClaims = false;
            });
        return services;
    }

    public static IServiceCollection ConfigureValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>(); // Автоматическая регистрация валидаторов
        services.AddFluentValidationAutoValidation();
        return services;
    }
    
    public static IServiceCollection ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(connectionString));
        return services;
    }

    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
            options.AddPolicy("AllUsers", policy => policy.RequireRole("user", "admin"));
        });
        services.AddSwaggerGen(options =>
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
        
        return services;
    }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IImageService>(provider => 
            new ImageService(Path.Combine(provider.GetRequiredService<IWebHostEnvironment>().WebRootPath, "uploads")));
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        
        services.AddScoped<IRequestHandler<AddBookCommand>, AddBookCommandHandler>();
        services.AddScoped<IRequestHandler<BorrowBookCommand>, BorrowBookCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteBookCommand>, DeleteBookCommandHandler>();
        services.AddScoped<IRequestHandler<ReturnBookCommand>, ReturnBookCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateBookCommand>, UpdateBookCommandHandler>();
        services.AddScoped<IRequestHandler<UploadBookImageCommand, string>, UploadBookImageCommandHandler>();
        
        services.AddScoped<IRequestHandler<AddAuthorCommand>, AddAuthorCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteAuthorCommand>, DeleteAuthorCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateAuthorCommand>, UpdateAuthorCommandHandler>();
        
        services.AddScoped<IRequestHandler<LoginUserCommand, TokenDto>, LoginUserCommandHandler>();
        services.AddScoped<IRequestHandler<RegisterUserCommand, User>, RegisterUserCommandHandler>();
        
        services.AddScoped<IRequestHandler<RefreshTokenCommand, TokenDto>, RefreshTokenCommandHandler>();
        
        services.AddScoped<IRequestHandler<GetAuthorByIdQuery, Author>, GetAuthorByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetAuthorsQuery, IEnumerable<Author>>, GetAuthorsQueryHandler>();
        services.AddScoped<IRequestHandler<GetAuthorByIdQuery, Author>, GetAuthorByIdQueryHandler>();
        
        services.AddScoped<IRequestHandler<GetAllBooksQuery, IEnumerable<Book>>, GetAllBooksQueryHandler>();
        services.AddScoped<IRequestHandler<GetBookByIdQuery, Book>, GetBookByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetBookByISBNQuery, Book>, GetBookByISBNQueryHandler>();
        services.AddScoped<IRequestHandler<GetBookImageQuery, Stream>, GetBookImageQueryHandler>();
        services.AddScoped<IRequestHandler<GetBooksQuery, IEnumerable<Book>>, GetBooksQueryHandler>();
        
        services.AddScoped<IRequestHandler<GetOverdueBooksQuery, List<string>>, GetOverdueBooksQueryHandler>();
        
        services.AddScoped<IRequestHandler<GetUserByIdQuery, User>, GetUserByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetUserByUsernameQuery, User>, GetUserByUsernameQueryHandler>();
        
        return services;
    }

    public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AuthorMappingProfile));
        services.AddAutoMapper(typeof(BookMappingProfile));
        services.AddAutoMapper(typeof(UserMappingProfile));
        
        return services;
    }
}