using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Pm.Common.Exceptions;
using Serilog;

namespace ProjectPlaguemangler.Filters
{
    public class ExceptionHandler : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment env;
        
        public ExceptionHandler(IHostingEnvironment env)
        {
            this.env = env;
        }

        public override void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            var result = new ViewResult();

            if (ex is PmNotFoundException)
            {
                result.ViewName = "NotFound";
                result.ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
                {
                    ["Path"] = context.HttpContext.Request.Path
                };
                context.HttpContext.Response.StatusCode = 404;
            }
            else
            {
                result.ViewName = "Error";
                result.ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
                {
                    ["Message"] = context.Exception.Message,
                    ["Trace"] = env.IsDevelopment() ? context.Exception.StackTrace : null
                };
                context.HttpContext.Response.StatusCode = 500;
            }

            context.Result = result;
            Log.Logger.Error(ex, "GlobalExceptionHandler - Uncaught exception");
        }
    }
}
