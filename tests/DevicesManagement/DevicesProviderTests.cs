using System;
using Xunit;
using System.Linq;
using DevSpector.Tests.Database;
using DevSpector.Application.Devices;
using DevSpector.Domain;
using DevSpector.Application.Networking;

namespace DevSpector.Tests.Application.Devices
{
    public class DevicesProviderTests
    {
        private readonly IDevicesProvider _provider;

        private readonly TestDbContext _context;

        public DevicesProviderTests()
        {
            _context = new TestDbContext();

            _provider = new DevicesProvider(
                new Repository(_context),
                new IPValidator()
            );
        }

        [Fact]
        public void ReturnsValidDevicesAmount()
        {
            // Act
            var actualAmount = _provider.GetDevices().Count;

            // Assert
            Assert.Equal(_context.Devices.Count(), actualAmount);
        }

        [Fact]
        public void ReturnsValidDeviceTypesAmount()
        {
            // Act
            var actualAmount = _provider.GetDeviceTypes().Count();

            // Assert
            Assert.Equal(_context.DeviceTypes.Count(), actualAmount);
        }

        [Fact]
        public void ReturnsDevice()
        {
            // Arrange
            var expectedDevice = _context.Devices.First();

            // Act
            var actualDevice = _provider.GetDevice(expectedDevice.InventoryNumber);

            // Assert
            Assert.Equal(expectedDevice.InventoryNumber, actualDevice.InventoryNumber);
            Assert.Equal(expectedDevice.NetworkName, actualDevice.NetworkName);
            Assert.Equal(expectedDevice.ModelName, actualDevice.ModelName);
            Assert.Equal(expectedDevice.TypeID, actualDevice.TypeID);
        }

        [Fact]
        public void ReturnsIPs()
        {
            // Arrange
            var targetDevice = _context.Devices.FirstOrDefault();
            var expectedIps = _context.IPAddresses.Where(ip => ip.DeviceID == targetDevice.ID).ToList();

            // Act
            var actualIPs = _provider.GetIPAddresses(targetDevice.ID).ToList();

            // Assert
            Assert.Equal(expectedIps.Count, actualIPs.Count);
            for (int i = 0; i < expectedIps.Count; i++)
                Assert.Equal(expectedIps[i].Address, actualIPs[i].Address);
        }

        [Fact]
        public void ReturnsSoftware()
        {
            // Arrange
            var targetDevice = _context.Devices.FirstOrDefault();
            var expectedSoftware = _context.DeviceSoftware.Where(s => s.DeviceID == targetDevice.ID).ToList();

            // Act
            var actualSoftware = _provider.GetDeviceSoftware(targetDevice.ID);

            // Assert
            Assert.Equal(expectedSoftware.Count, actualSoftware.Count);
            for (int i = 0; i < expectedSoftware.Count; i++)
            {
                Assert.Equal(expectedSoftware[i].SoftwareName, actualSoftware[i].SoftwareName);
                Assert.Equal(expectedSoftware[i].SoftwareVersion, actualSoftware[i].SoftwareVersion);
            }
        }

        [Fact]
        public void DoesDeviceExistsTest()
        {
            // Arrange
            var targetDevice = _context.Devices.FirstOrDefault();

            // Assert
            Assert.True(_provider.DoesDeviceExist(targetDevice.InventoryNumber));
            Assert.False(_provider.DoesDeviceExist(targetDevice.InventoryNumber + "_"));
            Assert.False(_provider.DoesDeviceExist(targetDevice.InventoryNumber + " "));
        }

        [Fact]
        public void HasIPTest()
        {
            // Arrange
            var targetDevice = _context.Devices.FirstOrDefault();
            var anotherDevice = _context.Devices.Skip(5).FirstOrDefault();

            var ips = _provider.GetIPAddresses(targetDevice.ID);
            var anotherIps = _provider.GetIPAddresses(anotherDevice.ID);

            // Assert
            foreach (var ip in ips)
                Assert.True(_provider.HasIP(targetDevice.ID, ip.Address));

            foreach (var ip in anotherIps)
                Assert.False(_provider.HasIP(targetDevice.ID, ip.Address));
        }

        [Fact]
        public void IsNetworkNameUniqueTest()
        {
            // Arrange
            var targetDevices = _provider.GetDevices();

            // Assert
            foreach (var device in targetDevices)
                Assert.False(_provider.IsNetworkNameUnique(device.NetworkName));

            foreach (var device in targetDevices)
                Assert.True(_provider.IsNetworkNameUnique(device.NetworkName + "_mess"));
        }

        [Fact]
        public void DoesDeviceTypeExistsTest()
        {
            // Arrange
            var types = _provider.GetDeviceTypes();

            // Assert
            foreach (var type in types)
                Assert.True(_provider.DoesDeviceTypeExist(type.ID));

            Assert.False(_provider.DoesDeviceTypeExist(Guid.NewGuid()));
            Assert.False(_provider.DoesDeviceTypeExist(Guid.Empty));
        }
    }
}
