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
    public class IPAddressProviderTests : DatabaseTestBase, IClassFixture<DatabaseFixture>
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
        public void GetFreeIPTest()
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
    }
}
