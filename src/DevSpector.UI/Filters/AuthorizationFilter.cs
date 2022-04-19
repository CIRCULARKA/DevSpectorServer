using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DevSpector.Application;

namespace DevSpector.UI.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly UsersManager _usersManager;

        public AuthorizationFilter(UsersManager usersManager)
        {
            _usersManager = usersManager;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var query = context.HttpContext.Request.Query;
            var request = context.HttpContext.Request;
            var unauthorizedResult = new UnauthorizedObjectResult(
                new BadRequestError {
                    Error = "Неверный ключ доступа",
                    Description = new List<string> { "Передайте ключ доступа в запросе" }
                }
            );

            string api = query["api"].Count == 0 ? request.Headers["API"] : query["api"];

            var callingUser = _usersManager.FindByApi(api);

            if (callingUser == null)
                context.Result = unauthorizedResult;
        }
    }
}
