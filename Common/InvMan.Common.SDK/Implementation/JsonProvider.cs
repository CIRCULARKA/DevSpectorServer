using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace InvMan.Common.SDK
{
    public class JsonProvider : IRawDataProvider
    {
        private Uri _pathToDevices;

        private Uri _pathToFreeIpAddresses;

        private Uri _pathToHousings;

        private readonly HttpClient _client;

        public JsonProvider()
        {
            _client = new HttpClient();

            ConfigureDefaultHost();

            BuildEndpointPath();
        }

        public JsonProvider(Uri host)
        {
            _client = new HttpClient();

            Host = host;

            BuildEndpointPath();
        }

        public Uri Host { get; set; }

        public Task<string> GetDevicesAsync(string accessToken) =>
            GetContentFromUriAsync(_pathToDevices.AbsoluteUri, accessToken);

        public Task<string> GetHousingsAsync(string accessToken) =>
            GetContentFromUriAsync(_pathToHousings.AbsoluteUri, accessToken);

        public Task<string> GetFreeIPAsync(string accessToken) =>
            GetContentFromUriAsync(_pathToFreeIpAddresses.AbsoluteUri, accessToken);

        public Task<string> GetHousingAsync(Guid housingID, string accessToken) =>
            GetContentFromUriAsync(_pathToHousings.AbsoluteUri + housingID, accessToken);

        private async Task<string> GetContentFromUriAsync(string path, string accessToken)
        {
            var request = new HttpRequestMessage {
                RequestUri = new Uri(path),
                Method = HttpMethod.Get
            };

            request.Headers.Add("API", accessToken);

            var response = await _client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new ArgumentException("Wrong API");

            return await response.Content.ReadAsStringAsync();
        }

        private void ConfigureDefaultHost()
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Port = 5000;
            uriBuilder.Host = "localhost";
            uriBuilder.Scheme = "http";
            Host = uriBuilder.Uri;
        }

        private Uri BuildUriWithHostBaseAndPath(string path)
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = Host.Scheme;
            uriBuilder.Host = Host.Host;
            uriBuilder.Port = Host.Port;
            uriBuilder.Path = path;
            return uriBuilder.Uri;
        }

        private void BuildEndpointPath()
        {
            _pathToDevices = BuildUriWithHostBaseAndPath("api/devices/");
            _pathToFreeIpAddresses = BuildUriWithHostBaseAndPath("api/free-ip/");
            _pathToHousings = BuildUriWithHostBaseAndPath("api/location/housings/");
        }
    }
}
