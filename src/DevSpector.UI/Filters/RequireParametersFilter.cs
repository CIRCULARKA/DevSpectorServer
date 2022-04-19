using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DevSpector.UI.Filters
{
    public class RequireParametersAttribute : ActionFilterAttribute
    {
        private readonly string[] _requiredParameters;

        public RequireParametersAttribute(params string[] parameters)
        {
            var noParametersException = new ArgumentException("Parameters must be provided");

            if (parameters == null)
                throw noParametersException;

            _requiredParameters = parameters;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var missingParameters = new List<string>();

            foreach (var param in _requiredParameters) {
                string value;
                try
                {
                    value = context.HttpContext.Request.Query[param];
                    if (string.IsNullOrWhiteSpace(value)) throw new Exception();
                }
                catch { missingParameters.Add(param); }
            }

            if (missingParameters.Count == 0)
                return;

            context.Result = new BadRequestObjectResult(
                new BadRequestError {
                    Error = "Не хватает требуемых параметров",
                    Description = missingParameters
                }
            );
        }
    }
}
