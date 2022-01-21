using System;
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

        public Task<string> GetDevicesAsync() =>
            GetContentFromUriAsync(_pathToDevices.AbsoluteUri);

        public Task<string> GetHousingsAsync() =>
            GetContentFromUriAsync(_pathToHousings.AbsoluteUri);

        public Task<string> GetFreeIPAsync() =>
            GetContentFromUriAsync(_pathToFreeIpAddresses.AbsoluteUri);

        public Task<string> GetHousingAsync(Guid housingID) =>
            GetContentFromUriAsync(_pathToHousings.AbsoluteUri + housingID);

        private async Task<string> GetContentFromUriAsync(string path)
        {
            var response = await _client.GetAsync(path);
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
