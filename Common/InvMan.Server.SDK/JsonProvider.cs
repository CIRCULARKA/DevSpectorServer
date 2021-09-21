using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InvMan.Server.SDK
{
    public class JsonProvider
    {
        private readonly Uri _hostAddress;

        private Uri _pathToDevices;

        private Uri _pathToIpAddresses;

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

        public async Task<string> GetDeviceJson(int deviceID)
        {
            var response = await _client.GetAsync(_pathToDevices.AbsoluteUri + deviceID);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetDeviceIpsJson(int deviceID)
        {
            var response = await _client.GetAsync(_pathToIpAddresses.AbsoluteUri + deviceID);
            return await response.Content.ReadAsStringAsync();
        }

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
            _pathToIpAddresses = BuildUriWithHostBaseAndPath("api/ipaddress/");
        }
    }
}
