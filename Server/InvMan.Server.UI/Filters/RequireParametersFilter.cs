using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InvMan.Server.UI.Filters
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

            foreach (var param in _requiredParameters)
                if (!context.HttpContext.Request.Query.ContainsKey(param))
                    missingParameters.Add(param);

            if (missingParameters.Count == 0)
                return;

            context.Result = new BadRequestObjectResult(
                new {
                    Error = "Some required parameters are missing",
                    RequiredParameters = missingParameters
                }
            );
        }
    }
}
