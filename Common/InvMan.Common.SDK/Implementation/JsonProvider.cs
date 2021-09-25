using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InvMan.Common.SDK
{
    public class JsonProvider : IRawDataProvider
    {
        private readonly Uri _hostAddress;

        private Uri _pathToDevices;

        private Uri _pathToIpAddresses;

        private Uri _pathToHousings;

        private readonly HttpClient _client;

        public JsonProvider(Uri hostAddress)
        {
            _client = new HttpClient();
            _hostAddress = hostAddress;

            BuildEndpointPath();
        }

        public JsonProvider(Uri hostAddress, HttpClient client)
        {
            _client = client;
            _hostAddress = hostAddress;

            BuildEndpointPath();
        }

        public Task<HttpResponseMessage> GetDevicesAsync() =>
            _client.GetAsync(_pathToDevices);

        public Task<HttpResponseMessage> GetFreeIPAsync() =>
            _client.GetAsync(_pathToIpAddresses.AbsoluteUri);

        public Task<HttpResponseMessage> GetHousingsAsync() =>
            _client.GetAsync(_pathToHousings.AbsoluteUri);

        public Task<HttpResponseMessage> GetHousingAsync(int housingID) =>
            _client.GetAsync(_pathToHousings.AbsoluteUri + housingID);

        private Task<string> GetHttpResponseMessageContent(HttpResponseMessage msg) =>
            msg.Content.ReadAsStringAsync();

        private Uri BuildUriWithHostBaseAndPath(string path)
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = _hostAddress.Scheme;
            uriBuilder.Host = _hostAddress.Host;
            uriBuilder.Port = _hostAddress.Port;
            uriBuilder.Path = path;
            return uriBuilder.Uri;
        }

        private void BuildEndpointPath()
        {
            _pathToDevices = BuildUriWithHostBaseAndPath("api/devices/");
            _pathToIpAddresses = BuildUriWithHostBaseAndPath("api/ipaddress/free");
            _pathToHousings = BuildUriWithHostBaseAndPath("api/location/housings");
        }
    }
}
