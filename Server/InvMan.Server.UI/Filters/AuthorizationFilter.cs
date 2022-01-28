using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace InvMan.Server.UI.Filters
{
    public class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        public AuthorizationFilter()
        {

        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var query = context.HttpContext.Request.Query;
            var request = context.HttpContext.Request;
            var unauthorizedResult = new JsonResult(new { Error = "Invalid API key", StatusCode = 401 }) {
                StatusCode = 401
            };

            // Firstly, try to get API from url. If theres is valid key, then quit with 200
            if (query.ContainsKey("api")) {
                if (query["api"] != "123") {
                    context.Result = unauthorizedResult;
                    return Task.CompletedTask;
                }
                else return Task.CompletedTask;
            }

            // If no key specified in url then try to find it in headers and if no api key there throw 401
            if (request.Headers.ContainsKey("API")) {
                if (request.Headers["API"] != "123")
                    context.Result = unauthorizedResult;
            } else context.Result = unauthorizedResult;

            return Task.CompletedTask;
        }
    }
}
