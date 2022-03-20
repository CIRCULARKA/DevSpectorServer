using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using DevSpector.Tests.Database;
using DevSpector.Application.Devices;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.Database;
using DevSpector.Application.Networking;
using DevSpector.SDK.Models;

namespace DevSpector.Tests.Application.Devices
{
    [Collection("DbCollection")]
    public class DevicesEditorTests : DatabaseTestBase
    {
        private readonly IDevicesEditor _editor;

        public DevicesEditorTests(DatabaseFixture _) : base(_)
        {
            var ipValidator = new IPValidator();

            _editor = new DevicesEditor(
                base._repo,
                new DevicesProvider(base._repo, ipValidator),
                ipValidator,
                new IPAddressProvider(base._repo, ipValidator, new IP4RangeGenerator(ipValidator))
            );
        }

        [Fact]
        public void CanCreateDevice()
        {
            // Arrange
            int countBefore = _context.Devices.Count();

            DeviceType expectedType = _context.DeviceTypes.FirstOrDefault();
            var expected = new DeviceInfo {
                InventoryNumber = "newTestInvNum",
                TypeID = expectedType.ID,
                NetworkName = "newTestNetworkName",
                ModelName = "newTestModelName"
            };

            // Act
            _editor.CreateDevice(expected);

            int countAfter = _context.Devices.Count();
            Device newDevice = _context.Devices.SingleOrDefault(d => d.InventoryNumber == expected.InventoryNumber);

            // Assert
            Assert.Equal(countBefore + 1, countAfter);
            Assert.Equal(expected.InventoryNumber, newDevice.InventoryNumber);
            Assert.Equal(expected.ModelName, newDevice.ModelName);
            Assert.Equal(expected.NetworkName, newDevice.NetworkName);
            Assert.Equal(expected.TypeID, newDevice.TypeID);
        }

        [Fact]
        public void CantCreateDevice()
        {
            // Arrange
            var wrongDeviceData1 = new DeviceInfo {
                InventoryNumber = "SomeInvNum",
                TypeID = Guid.Empty,
            };

            var wrongDeviceData2 = new DeviceInfo {
                InventoryNumber = null,
                TypeID = _context.DeviceTypes.FirstOrDefault().ID
            };

            // Assert
            Assert.Throws<ArgumentException>(() => _editor.CreateDevice(wrongDeviceData1));
            Assert.Throws<ArgumentNullException>(() => _editor.CreateDevice(wrongDeviceData2));
        }

        [Fact]
        public void CanUpdateDevice()
        {
            // Arrange
            var originalInfo = new DeviceInfo {
                InventoryNumber = Guid.NewGuid().ToString(),
                TypeID = _context.DeviceTypes.FirstOrDefault().ID,
                NetworkName = Guid.NewGuid().ToString(),
                ModelName = Guid.NewGuid().ToString()
            };

            var updatedInfo = new DeviceInfo {
                InventoryNumber = Guid.NewGuid().ToString(),
                TypeID = _context.DeviceTypes.Skip(1).FirstOrDefault().ID,
                NetworkName = Guid.NewGuid().ToString(),
                ModelName = Guid.NewGuid().ToString()
            };

            // Act

            _context.Devices.Add(new Device {
                InventoryNumber = originalInfo.InventoryNumber,
                TypeID = originalInfo.TypeID,
                NetworkName = originalInfo.NetworkName,
                ModelName = originalInfo.ModelName
            });
            _context.SaveChanges();

            _editor.UpdateDevice(originalInfo.InventoryNumber, updatedInfo);

            Device updatedDevice = _context.Devices.Single(d => d.InventoryNumber == updatedInfo.InventoryNumber);

            // Assert
            Assert.NotNull(updatedDevice);
            Assert.Equal(updatedDevice.InventoryNumber, updatedInfo.InventoryNumber);
            Assert.Equal(updatedDevice.NetworkName, updatedInfo.NetworkName);
            Assert.Equal(updatedDevice.ModelName, updatedInfo.ModelName);
            Assert.Equal(updatedDevice.TypeID, updatedInfo.TypeID);
        }
    }
}
