using System.Net;
using Minesweeper.Server.Services;
using Minesweeper.Server.Models;

namespace Minesweeper.Server.Helpers
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IJsonConverter _converter;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, IJsonConverter converter)
        {
            _next = next;
            _logger = logger;
            _converter = converter;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                ErrorResponse errorResponse = new ErrorResponse() 
                { 
                    error = error.Message
                };
                var result = _converter.WriteJson(errorResponse);
                await response.WriteAsync(result);
            }
        }
    }
}
