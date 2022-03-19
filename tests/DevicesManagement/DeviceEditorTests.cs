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
    }
}
