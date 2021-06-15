using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using ServiceStack.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerosCompanyApi.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly Logger _logger;

        //private Logger logger = LogManager.GetLogger("HerosCompanyLoggerRule");
        public ErrorController(Logger logger)
        {
            _logger = logger;
        }
        [Route("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if(context.Error is HttpException)
            {
                HttpException httpError = (HttpException)context.Error;

                _logger.Error("Error Code: " + httpError.StatusCode + " | " + "Error Message: " + httpError.StatusDescription);


                return Problem(
                    detail: httpError.StatusDescription,
                    statusCode: httpError.StatusCode
                    );
            }    
            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message,
                statusCode: 500);
        }
    }
}
