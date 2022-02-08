using System;
using System.Text.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using InvMan.Common.SDK.Models;

namespace InvMan.Common.SDK.Authorization
{
    public class AuthorizationManager : IAuthorizationManager
    {
        private readonly string _host = "localhost";

        private readonly string _path = "api/users/authorize";

        private readonly HttpClient _client;

        public AuthorizationManager()
        {
            _client = new HttpClient();
        }

        public async Task<User> TrySignIn(string login, string password)
        {
            var targetEndpoint = BuildTargetEndpoint(login, password);

            var response = await _client.GetAsync(targetEndpoint);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new ArgumentException("Wrong credentials");

            var result = await JsonSerializer.DeserializeAsync<User>(
                await response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return result;
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
