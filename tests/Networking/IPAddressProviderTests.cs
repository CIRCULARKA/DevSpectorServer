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
            List<IPAddress> expected = GetFreeIP();

            // Act
            List<IPAddress> actual = _provider.GetFreeIP();

            // Assert
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.True(_provider.IsAddressFree(expected[i].Address));
                Assert.Equal(expected[i].Address, actual[i].Address);
            }
        }

        [Fact]
        public void ReturnsSortedFreeIP()
        {
            // Arrange
            List<IPAddress> expected = GetFreeIP();

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

        public IPAddress GetFreeIPSingle()
        {
			IEnumerable<IPAddress> allIps = _repo.Get<IPAddress>();
			IEnumerable<IPAddress> busyIps = _repo.Get<DeviceIPAddress>().Select(di => di.IPAddress);

			return allIps.Except(busyIps, new IPAddressComparer()).ToList().FirstOrDefault();
        }

        public List<IPAddress> GetFreeIP()
        {
			IEnumerable<IPAddress> allIps = _repo.Get<IPAddress>();
			IEnumerable<IPAddress> busyIps = _repo.Get<DeviceIPAddress>().Select(di => di.IPAddress);

			return allIps.Except(busyIps, new IPAddressComparer()).ToList();
        }
    }
}
