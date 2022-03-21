using System.Linq;
using System.Collections.Generic;
using Xunit;
using DevSpector.Tests.Database;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.Application.Networking;

namespace DevSpector.Tests.Application.Networking
{
    [Collection(nameof(DatabaseCollection))]
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
                Assert.True(_provider.IsAddressFree(expected[i].Address));
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

        [Fact]
        public void CanGenerateIPRange()
        {
            // Arrange

            // Big changes to IPAddresses table which will be made by GenerateRange() method in database may break other tests
            // so I decided to execute this particullar test in independent database
            using (var localContext = new TestDbContext("Data Source=./IPRangeTest.db"))
            {
                var localRepo = new Repository(localContext);
                var localProvider = new IPAddressProvider(
                    localRepo,
                    new IPValidator(),
                    new IP4RangeGenerator(new IPValidator())
                );

                var expected = new string[] {
                    "255.2.10.153",
                    "255.2.10.154",
                    "255.2.10.155",
                    "255.2.10.156",
                    "255.2.10.157",
                    "255.2.10.158"
                };

                // Act
                localProvider.GenerateRange("255.2.10.158", 29);
                List<string> actual = localRepo.Get<IPAddress>().Select(ip => ip.Address).ToList();

                // Assert
                Assert.Equal(expected.Length, actual.Count);
                for (int i = 0; i < expected.Length; i++)
                    Assert.Contains(expected[i], actual);
            }
        }
    }
}
