using System;
using InvMan.Server.SDK;
using Xunit;

namespace InvMan.Tests.Server.SDK
{
    public class InvManJsonProviderTests
    {
        [Fact]
        public async void CanLoadDataAboutDevice()
        {
            // Arrange
            var deviceID = 1;
            var uriBuilder = new UriBuilder();
            uriBuilder.Port = 5000;
            uriBuilder.Host = "localhost";
            uriBuilder.Scheme = "http";
            var hostAddress = uriBuilder.Uri;
            var provider = new InvManJsonProvider(hostAddress);

            // Act
            var result = await provider.GetDeviceJson(deviceID);
        }
    }
}
