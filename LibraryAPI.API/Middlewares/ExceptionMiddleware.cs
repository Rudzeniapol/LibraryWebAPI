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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new { error = "Невалидный запрос", code = 400, details = ex.Message });
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsJsonAsync(new { error = "Сущность не найдена", code = 404, details = ex.Message });
            }
            catch (EntityExistsException ex)
            {
                _logger.LogError(ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await context.Response.WriteAsJsonAsync(new { error = "Сущность уже существует", code = 409, details = ex.Message });
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError(ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Ошибка токена", code = 401, details = ex.Message });
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning("Истекло время ожидания для запроса");
                context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                await context.Response.WriteAsJsonAsync(new { error = "Время ожидания истекло", code = 408, details = ex.Message });
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning("Запрос отменён");
                context.Response.StatusCode = 499;
                await context.Response.WriteAsJsonAsync(new { error = "Запрос отменён", code = 499, details = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Неавторизованный доступ", code = 401, details = ex.Message });
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new{ error = "Данные не прошли валидацию", code = 400,  details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Произошла ошибка: {ex.Message}");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    error = "Ошибка на стороне сервера",
                    code = ex.HResult,
                    details = ex.Message
                };
                
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}