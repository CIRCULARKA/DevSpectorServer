using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using InvMan.Server.Domain.Models;

namespace InvMan.Server.UI.Filters
{
    public class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly UserManager<DesktopUser> _usersManager;

        public AuthorizationFilter(UserManager<DesktopUser> usersManager)
        {
            _usersManager = usersManager;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var query = context.HttpContext.Request.Query;
            var request = context.HttpContext.Request;
            var unauthorizedResult = new UnauthorizedObjectResult(
                new BadRequestErrorMessage {
                    Error = "Wrong API key",
                    Description = "Provide an API key as an API header or via api query"
                }
            );

            string api = query["api"].Count == 0 ? request.Headers["API"] : query["api"];

            var callingUser = await _usersManager.FindByIdAsync(api);

            if (callingUser == null)
                context.Result = unauthorizedResult;

            return;
        }
    }
}
