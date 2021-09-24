using System;
using InvMan.Common.SDK;
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
            var result = await _defaultProvider.GetAllDevicesRawAsync();

            Assert.True(result.Length > 0);
        }

        [Fact]
        public async void CanLoadFreeIP()
        {
            var result = await _defaultProvider.GetFreeIPRawAsync();

            Assert.True(result.Length > 0);
        }
    }
}
