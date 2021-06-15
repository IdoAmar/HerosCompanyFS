using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using NLog;
using Microsoft.AspNetCore.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace HerosCompanyApi.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Logger _logger;

        //private Logger logger = LogManager.GetLogger("HerosCompanyLoggerRule");

        public LoggerMiddleware(RequestDelegate next,Logger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {


            //logger.Info("Request : " + httpContext.Request.Method +
            //            " From : " + httpContext.Request.Path.Value +
            //            " By :" + httpContext.Connection.RemoteIpAddress);
            await _next(httpContext);

            _logger.Info("Request : " + httpContext.Request.Method +
                              " | From : " + httpContext.Request.Path.Value +
                              " | By : " + httpContext.Connection.RemoteIpAddress +
                              " | Code : " + httpContext.Response.StatusCode);

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggerMiddleware>();
        }
    }
}
