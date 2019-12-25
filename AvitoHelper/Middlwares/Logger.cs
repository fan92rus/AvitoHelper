using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AvitoHelper.Middlwares
{
    public sealed class ExceptionHandlerMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exp)
            {
                _logger.Error(exp.Message);
                await HandleExceptionAsync(context, exp.GetBaseException());
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exp)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var result = JsonConvert.SerializeObject(new { Code = code, Message = exp.Message, StackTrace = exp.StackTrace });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
