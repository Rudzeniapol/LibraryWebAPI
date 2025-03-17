using System.Data;
using System.Net;
using System.Text.Json;
using FluentValidation;
using LibraryAPI.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace LibraryAPI.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        private async void ConfigureResponse(HttpContext httpContext, int StatusCode, object? Json, string logMessage)
        {
            _logger.LogError(logMessage);
            httpContext.Response.StatusCode = StatusCode;
            await httpContext.Response.WriteAsJsonAsync(Json);
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException ex)
            {
                ConfigureResponse(context,
                    (int)HttpStatusCode.BadRequest,
                    new { error = "Невалидный запрос", code = 400, details = ex.Message },
                    ex.Message);
            }
            catch (NotFoundException ex)
            {
                ConfigureResponse(context,
                    (int)HttpStatusCode.NotFound,
                    new { error = "Сущность не найдена", code = 404, details = ex.Message },
                    ex.Message);
            }
            catch (EntityExistsException ex)
            {
                ConfigureResponse(context,
                    (int)HttpStatusCode.Conflict,
                    new { error = "Сущность уже существует", code = 409, details = ex.Message },
                    ex.Message);
            }
            catch (SecurityTokenException ex)
            {
                ConfigureResponse(context,
                    (int)HttpStatusCode.Unauthorized,
                    new { error = "Ошибка токена", code = 401, details = ex.Message },
                    ex.Message);
            }
            catch (TaskCanceledException ex)
            {
                ConfigureResponse(context,
                    (int)HttpStatusCode.RequestTimeout,
                    new { error = "Время ожидания истекло", code = 408, details = ex.Message },
                    "Истекло время ожидания для запроса");
            }
            catch (OperationCanceledException ex)
            {
                ConfigureResponse(context,
                    499,
                    new { error = "Запрос отменён", code = 499, details = ex.Message },
                    "Запрос отменён");
            }
            catch (UnauthorizedAccessException ex)
            {
                ConfigureResponse(context,
                    (int)HttpStatusCode.Unauthorized,
                    new { error = "Ошибка авторизации", code = 401, details = ex.Message },
                    ex.Message);
            }
            catch (ValidationException ex)
            {
                ConfigureResponse(context,
                    (int)HttpStatusCode.BadRequest,
                    new { error = "Ошибка валидации данных", code = 400, details = ex.Message },
                    ex.Message);
            }
            catch (DataException ex)
            {
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                ConfigureResponse(context,
                    (int)HttpStatusCode.InternalServerError,
                    new { error = $"Произошла ошибка: {ex.Message}", code = ex.HResult, details = ex.Message },
                    "Ошибка на стороне сервера");
            }
        }
    }
}