using System;
using InvMan.Common.SDK;
using Xunit;

namespace InvMan.Tests.Server.SDK
{
    public class JsonProviderTests
    {
        private readonly IRawDataProvider _defaultProvider;

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
        public async void CanLoadDevices()
        {
            var result = await _defaultProvider.GetDevicesAsync();
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async void CanLoadFreeIP()
        {
            var result = await _defaultProvider.GetFreeIPAsync();
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async void CanLoadHousings()
        {
            var result = await _defaultProvider.GetHousingsAsync();
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async void CanLoadSpecifiedHousing()
        {
            var result = await _defaultProvider.GetHousingAsync(1);
            Assert.True(result.Length > 0);
        }
    }
}
