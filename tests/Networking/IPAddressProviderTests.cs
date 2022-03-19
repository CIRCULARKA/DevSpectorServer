using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using DevSpector.Tests.Database;
using DevSpector.Application.Devices;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.Database;
using DevSpector.Application.Networking;
using DevSpector.SDK.Models;

namespace DevSpector.Tests.Application.Networking
{
    [Collection("DbCollection")]
    public class IPAddressProviderTests : DatabaseTestBase
    {
        private readonly IIPAddressProvider _provider;

        public IPAddressProviderTests(DatabaseFixture _) : base(_)
        {
            var ipValidator = new IPValidator();

            _provider = new IPAddressProvider(
                base._repo,
                ipValidator,
                new IP4RangeGenerator(ipValidator)
            );
        }

        [Fact]
        public void ReturnsFreeIP()
        {
            // Arrange
            List<IPAddress> expected =
                _context.IPAddresses.Where(ip => ip.DeviceID == null).ToList();

            // Act
            List<IPAddress> actual = _provider.GetFreeIP();

            // Assert
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Address, actual[i].Address);
                Assert.Equal(expected[i].DeviceID, actual[i].DeviceID);
            }
        }

        [Fact]
        public void ReturnsSortedFreeIP()
        {
            // Arrange
            // Sort IP addresses by first byte, then by second, etc. up to fourth byte
            List<IPAddress> expected = _context.IPAddresses.Where(ip => ip.DeviceID == null).ToList();

            // Act
            List<string> actual = _provider.GetFreeIPSorted().Select(ip => ip.Address).ToList();

            // Arrange
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
                Assert.Contains(expected[i].Address, actual);
        }

        [Fact]
        public void ReturnsIPFromString()
        {
            // Arrange
            var ips = _context.IPAddresses.ToList();

            // Assert
            foreach (var ip in ips)
                Assert.Equal(ip.Address, _provider.GetIP(ip.Address).Address);

            Assert.Null(_provider.GetIP("0.0.0.0"));
        }
    }
}
