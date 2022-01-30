using System;
using InvMan.Common.SDK;
using InvMan.Common.SDK.Authorization;
using Xunit;

namespace InvMan.Tests.Server.SDK
{
    public class JsonProviderTests
    {
        private readonly IRawDataProvider _defaultProvider;

        public JsonProviderTests()
        {
            var authManager = new AuthorizationManager();

            _defaultProvider = new JsonProvider(
                new Uri("http://localhost:5000/api/devices")
            );
        }

        [Fact]
        public async void CanLoadDevices()
        {
            var result = await _defaultProvider.GetDevicesAsync(Guid.Empty.ToString());
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async void CanLoadFreeIP()
        {
            var result = await _defaultProvider.GetFreeIPAsync(Guid.Empty.ToString());
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async void CanLoadHousings()
        {
            var result = await _defaultProvider.GetHousingsAsync(Guid.Empty.ToString());
            Assert.True(result.Length > 0);
        }
    }
}
