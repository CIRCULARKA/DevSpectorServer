using System;
using InvMan.Server.SDK;
using Xunit;

namespace InvMan.Tests.Server.SDK
{
    public class JsonProviderTests
    {
        private readonly IDataProvider _defaultProvider;

        private readonly int _targetDeviceID = 1;

        private readonly Uri _targetHost;

        public JsonProviderTests()
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Port = 5000;
            uriBuilder.Host = "localhost";
            uriBuilder.Scheme = "http";
            _targetHost = uriBuilder.Uri;

            _defaultProvider = new JsonProvider(_targetHost);
        }

        [Fact]
        public async void CanLoadDataAboutDevices()
        {
            var result = await _defaultProvider.GetAllDevicesRaw();

            Assert.True(result.Length > 0);
        }

        [Fact]
        public async void CanLoadDeviceIPs()
        {
            var result = await _defaultProvider.GetDeviceIpsRaw(_targetDeviceID);

            Assert.True(result.Length > 0);
        }
    }
}
