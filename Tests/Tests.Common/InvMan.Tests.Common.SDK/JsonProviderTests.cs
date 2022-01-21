using System;
using InvMan.Common.SDK;
using Xunit;

namespace InvMan.Tests.Server.SDK
{
    public class JsonProviderTests
    {
        private readonly IRawDataProvider _defaultProvider;

        private readonly Uri _targetHost;

        public JsonProviderTests()
        {
            _defaultProvider = new JsonProvider();
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
    }
}
