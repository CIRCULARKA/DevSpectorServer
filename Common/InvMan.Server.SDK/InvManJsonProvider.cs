using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InvMan.Server.SDK
{
    public class InvManJsonProvider
    {
        private readonly Uri _hostAddress;

        private readonly HttpClient _client;

        private Uri _pathToDevices;

        public InvManJsonProvider(Uri hostAddress)
        {
            _client = new HttpClient();
            _hostAddress = hostAddress;

            BuildEndpointPath();
        }

        public InvManJsonProvider(Uri hostAddress, HttpClient client)
        {
            _client = client;
            _hostAddress = hostAddress;

            BuildEndpointPath();
        }

        public async Task<string> GetDeviceJson(int deviceId)
        {
            var response = await _client.GetAsync(_pathToDevices.AbsoluteUri + deviceId);
            return await response.Content.ReadAsStringAsync();
        }

        private void BuildEndpointPath()
        {
            var deviceUriBuilder = new UriBuilder();
            deviceUriBuilder.Scheme = _hostAddress.Scheme;
            deviceUriBuilder.Host = _hostAddress.Host;
            deviceUriBuilder.Port = _hostAddress.Port;
            deviceUriBuilder.Path = "api/devices/";
            _pathToDevices = deviceUriBuilder.Uri;
        }
    }
}
