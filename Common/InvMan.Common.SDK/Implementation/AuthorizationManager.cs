using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace InvMan.Common.SDK.Authorization
{
    public class AuthorizationManager
    {
        private readonly string _host = "localhost";

        private readonly string _path = "api/users/authorize";

        private readonly HttpClient _client;

        public AuthorizationManager()
        {
            _client = new HttpClient();
        }

        public async Task<string> GetAccessTokenAsync(string login, string password)
        {
            var targetEndpoint = BuildTargetEndpoint(login, password);

            var response = await _client.GetAsync(targetEndpoint);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new ArgumentException("Wrong credentials");

            return response.Headers.GetValues("API").ToList()[0];
        }

        private Uri BuildTargetEndpoint(string login, string password)
        {
            var builder = new UriBuilder();
            builder.Port = 5000;
            builder.Host = _host;
            builder.Path = _path;
            builder.Query = $"login={login}&password={password}";
            builder.Scheme = "http";

            return builder.Uri;
        }
    }
}
