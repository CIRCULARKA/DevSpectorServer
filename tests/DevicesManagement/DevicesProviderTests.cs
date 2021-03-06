using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using DevSpector.Tests.Database;
using DevSpector.Application.Devices;
using DevSpector.Domain.Models;
using DevSpector.Database.DTO;
using DevSpector.Application.Networking;

namespace DevSpector.Tests.Application.Devices
{
    [Collection(nameof(DatabaseCollection))]
    public class DevicesProviderTests : DatabaseTestBase
    {
        private readonly IDevicesProvider _provider;

        public DevicesProviderTests(DatabaseFixture _) : base(_)
        {
            _provider = new DevicesProvider(
                base._repo,
                new IPValidator()
            );
        }

        [Fact]
        public void ReturnsValidDevicesAmount()
        {
            // Act
            int actualAmount = _provider.GetDevices().Count;

            // Assert
            Assert.Equal(_context.Devices.Count(), actualAmount);
        }

        [Fact]
        public void ReturnsValidDeviceTypesAmount()
        {
            // Act
            int actualAmount = _provider.GetDeviceTypes().Count();

            // Assert
            Assert.Equal(_context.DeviceTypes.Count(), actualAmount);
        }

        [Fact]
        public void ReturnsDevice()
        {
            // Arrange
            Device expectedDevice = _context.Devices.First();

            // Act
            Device actualDevice = _provider.GetDevice(expectedDevice.InventoryNumber);

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
            Device targetDevice = _context.Devices.FirstOrDefault();
            List<IPAddress> expectedIps = _context.DeviceIPAddresses.
                Where(ip => ip.DeviceID == targetDevice.ID).
                    Select(di => di.IPAddress).ToList();

            // Act
            List<IPAddress> actualIPs = _provider.GetIPAddresses(targetDevice.ID).ToList();

            // Assert
            Assert.Equal(expectedIps.Count, actualIPs.Count);
            for (int i = 0; i < expectedIps.Count; i++)
                Assert.Equal(expectedIps[i].Address, actualIPs[i].Address);
        }

        [Fact]
        public void ReturnsSoftware()
        {
            // Arrange
            Device targetDevice = _context.Devices.FirstOrDefault();
            List<DeviceSoftware> expectedSoftware = _context.DeviceSoftware.Where(s => s.DeviceID == targetDevice.ID).ToList();

            // Act
            List<DeviceSoftware> actualSoftware = _provider.GetDeviceSoftware(targetDevice.ID);

            // Assert
            Assert.Equal(expectedSoftware.Count, actualSoftware.Count);
            for (int i = 0; i < expectedSoftware.Count; i++)
            {
                Assert.Equal(expectedSoftware[i].SoftwareName, actualSoftware[i].SoftwareName);
                Assert.Equal(expectedSoftware[i].SoftwareVersion, actualSoftware[i].SoftwareVersion);
            }
        }

        [Fact]
        public void ReturnsSingleSoftware()
        {
            // Arrange
            Device targetDevice = _context.Devices.FirstOrDefault();
            List<DeviceSoftware> deviceSoft = _context.DeviceSoftware.Where(
                ds => (ds.DeviceID == targetDevice.ID)
            ).ToList();

            var actualSoft = new List<SoftwareInfo>();

            // Act
            foreach (var soft in deviceSoft)
            {
                DeviceSoftware actual = _provider.GetDeviceSoftware(targetDevice.ID, soft.SoftwareName, soft.SoftwareVersion);

                actualSoft.Add(new SoftwareInfo {
                    SoftwareName = actual.SoftwareName,
                    SoftwareVersion = actual.SoftwareVersion
                });
            }

            // Assert
            for (int i = 0; i < deviceSoft.Count; i++)
            {
                Assert.Equal(deviceSoft[i].SoftwareName, actualSoft[i].SoftwareName);
                Assert.Equal(deviceSoft[i].SoftwareVersion, actualSoft[i].SoftwareVersion);
                Assert.Null(_provider.GetDeviceSoftware(targetDevice.ID, actualSoft[i].SoftwareName, "wrongVersion"));
            }
        }

        [Fact]
        public void DoesDeviceExistsTest()
        {
            // Arrange
            Device targetDevice = _context.Devices.FirstOrDefault();

            // Assert
            Assert.True(_provider.DoesDeviceExist(targetDevice.InventoryNumber));
            Assert.False(_provider.DoesDeviceExist(targetDevice.InventoryNumber + "_"));
            Assert.False(_provider.DoesDeviceExist(targetDevice.InventoryNumber + " "));
        }

        [Fact]
        public void ReturnsValidDeviceLocation()
        {
            // Arrange
            List<DeviceCabinet> devicesCabinet = _context.DeviceCabinets.ToList();

            // Assert
            foreach (var deviceCabinet in devicesCabinet)
            {
                Cabinet cabinet = _provider.GetDeviceCabinet(deviceCabinet.DeviceID);
                Assert.Equal(deviceCabinet.Cabinet.Name, cabinet.Name);
                Assert.Equal(deviceCabinet.Cabinet.Housing.Name, cabinet.Housing.Name);
            }
        }

        [Fact]
        public void HasIPTest()
        {
            // Arrange
            Device targetDevice = _context.Devices.FirstOrDefault();
            Device anotherDevice = _context.Devices.Skip(5).FirstOrDefault();

            List<IPAddress> ips = _provider.GetIPAddresses(targetDevice.ID);
            List<IPAddress> anotherIps = _provider.GetIPAddresses(anotherDevice.ID);

            // Assert
            foreach (var ip in ips)
                Assert.True(_provider.HasIP(targetDevice.ID, ip.Address));

            foreach (var ip in anotherIps)
                Assert.False(_provider.HasIP(targetDevice.ID, ip.Address));
        }

        [Fact]
        public void HasSoftwareTest()
        {
            // Arrange
            Device targetDevice = _context.Devices.FirstOrDefault();
            Device anotherDevice = _context.Devices.Skip(3).FirstOrDefault();

            List<DeviceSoftware> targetSoft = _provider.GetDeviceSoftware(targetDevice.ID);
            List<DeviceSoftware> anotherSoft = _provider.GetDeviceSoftware(anotherDevice.ID);

            // Assert
            foreach (var soft in targetSoft)
            {
                Assert.True(_provider.HasSoftware(targetDevice.ID, soft.SoftwareName));
                Assert.True(_provider.HasSoftware(targetDevice.ID, soft.SoftwareName, soft.SoftwareVersion));
            }

            foreach (var soft in anotherSoft)
            {
                Assert.False(_provider.HasSoftware(targetDevice.ID, soft.SoftwareName));
                Assert.False(_provider.HasSoftware(targetDevice.ID, soft.SoftwareName, soft.SoftwareVersion));
            }
        }

        [Fact]
        public void IsNetworkNameUniqueTest()
        {
            // Arrange
            List<Device> targetDevices = _provider.GetDevices();

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
            List<DeviceType> types = _provider.GetDeviceTypes();

            // Assert
            foreach (var type in types)
                Assert.True(_provider.DoesDeviceTypeExist(type.ID));

            Assert.False(_provider.DoesDeviceTypeExist(Guid.NewGuid()));
            Assert.False(_provider.DoesDeviceTypeExist(Guid.Empty));
        }

        [Fact]
        public void GetOutputDevicesTest()
        {
            // Arrange
            List<Device> devices = _provider.GetDevices();

            // Act
            List<DeviceToOutput> actualDevices = _provider.GetDevicesToOutput();

            // Assert
            Assert.Equal(devices.Count, actualDevices.Count);
            for (int i = 0; i < devices.Count; i++)
            {
                List<IPAddress> ips = _provider.GetIPAddresses(devices[i].ID);
                List<DeviceSoftware> soft = _provider.GetDeviceSoftware(devices[i].ID);
                Cabinet cabinet = _provider.GetDeviceCabinet(devices[i].ID);

                Assert.Equal(devices[i].ID, actualDevices[i].ID);
                Assert.Equal(devices[i].InventoryNumber, actualDevices[i].InventoryNumber);
                Assert.Equal(devices[i].NetworkName, actualDevices[i].NetworkName);
                Assert.Equal(devices[i].Type.Name, actualDevices[i].Type);
                Assert.Equal(devices[i].ModelName, actualDevices[i].ModelName);

                Assert.Equal(cabinet.Name, actualDevices[i].Cabinet);
                Assert.Equal(cabinet.Housing.Name, actualDevices[i].Housing);

                // Compare IP addresses
                Assert.Equal(ips.Count, actualDevices[i].IPAddresses.Count);
                for (int j = 0; j < ips.Count; j++)
                {
                    List<string> expectedIPs = ips.Select(ip => ip.Address).ToList();
                    List<string> actualIPs = actualDevices[i].IPAddresses;
                    Assert.True(actualIPs.Contains(expectedIPs[j]));
                }

                // Compare software
                Assert.Equal(soft.Count, actualDevices[i].Software.Count);
                for (int j = 0; j < soft.Count; j++)
                {
                    Assert.Equal(soft[j].SoftwareName, actualDevices[i].Software[j].SoftwareName);
                    Assert.Equal(soft[j].SoftwareVersion, actualDevices[i].Software[j].SoftwareVersion);
                }
            }
        }
    }
}
