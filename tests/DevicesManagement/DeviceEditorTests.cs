using System;
using System.Linq;
using Xunit;
using DevSpector.Tests.Database;
using DevSpector.Application.Devices;
using DevSpector.Domain.Models;
using DevSpector.Database.DTO;
using DevSpector.Application.Networking;

namespace DevSpector.Tests.Application.Devices
{
    [Collection(nameof(DatabaseCollection))]
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
            var expected = new DeviceToAdd {
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
            var wrongDeviceData1 = new DeviceToAdd {
                InventoryNumber = "SomeInvNum",
                TypeID = Guid.Empty,
            };

            var wrongDeviceData2 = new DeviceToAdd {
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
            var originalInfo = new DeviceToAdd {
                InventoryNumber = Guid.NewGuid().ToString(),
                TypeID = _context.DeviceTypes.FirstOrDefault().ID,
                NetworkName = Guid.NewGuid().ToString(),
                ModelName = Guid.NewGuid().ToString()
            };

            var updatedInfo = new DeviceToAdd {
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

        [Fact]
        public void CantUpdateDevice()
        {
            // Arrange
            var tempDevice = new Device {
                InventoryNumber = Guid.NewGuid().ToString(),
                TypeID = _context.DeviceTypes.FirstOrDefault().ID
            };

            var conflictDevice = new Device {
                InventoryNumber = Guid.NewGuid().ToString(),
                TypeID = _context.DeviceTypes.Skip(1).FirstOrDefault().ID
            };

            _context.Devices.Add(tempDevice);
            _context.Devices.Add(conflictDevice);
            _context.SaveChanges();

            var wrongType = new DeviceToAdd {
                InventoryNumber = tempDevice.InventoryNumber,
                TypeID = Guid.NewGuid()
            };

            var busyInventoryNumber = new DeviceToAdd {
                InventoryNumber = conflictDevice.InventoryNumber
            };

            // Assert
            Assert.Throws<ArgumentException>(() => _editor.UpdateDevice("wrongString", null));
            Assert.Throws<ArgumentException>(() => _editor.UpdateDevice(tempDevice.InventoryNumber, wrongType));
            Assert.Throws<ArgumentException>(() => _editor.UpdateDevice(tempDevice.InventoryNumber, busyInventoryNumber));
        }

        [Fact]
        public void CanDeleteDevice()
        {
            // Arrange
            var id = Guid.NewGuid();

            var deviceToDelete = new Device {
                ID = id,
                InventoryNumber = Guid.NewGuid().ToString(),
                TypeID = _context.DeviceTypes.FirstOrDefault().ID
            };

            _context.Devices.Add(deviceToDelete);

            _context.DeviceCabinets.Add(new DeviceCabinet {
                DeviceID = deviceToDelete.ID,
                CabinetID = _context.Cabinets.FirstOrDefault().ID
            });

            _context.DeviceSoftware.Add(new DeviceSoftware {
                DeviceID = deviceToDelete.ID,
                SoftwareName = Guid.NewGuid().ToString(),
                SoftwareVersion = Guid.NewGuid().ToString()
            });

            _context.SaveChanges();

            // Act
            _editor.DeleteDevice(deviceToDelete.InventoryNumber);

            // Assert
            Assert.Null(_context.Devices.FirstOrDefault(d => d.InventoryNumber == deviceToDelete.InventoryNumber));
            Assert.Null(_context.DeviceCabinets.FirstOrDefault(dc => dc.DeviceID == id));
            Assert.Null(_context.DeviceSoftware.FirstOrDefault(ds => ds.DeviceID == id));
        }

        [Fact]
        public void CantDeleteDevice()
        {
            // Arrange
            var wrongInvNum = "mess";

            // Assert
            Assert.Throws<ArgumentException>(() => _editor.DeleteDevice(wrongInvNum));
        }

        [Fact]
        public void CanMoveDevice()
        {
            // Arrange
            var newDevice = new Device {
                InventoryNumber = Guid.NewGuid().ToString(),
                TypeID = _context.DeviceTypes.FirstOrDefault().ID
            };

            _context.Devices.Add(newDevice);

            var newDeviceCabinet = new DeviceCabinet {
                DeviceID = newDevice.ID,
                CabinetID = _context.Cabinets.FirstOrDefault().ID
            };

            _context.DeviceCabinets.Add(newDeviceCabinet);

            _context.SaveChanges();

            Guid targetDeviceCabinetID = _context.Cabinets.Skip(1).FirstOrDefault().ID;

            // Act
            _editor.MoveDevice(newDevice.InventoryNumber, targetDeviceCabinetID);

            // Assert
            Assert.Equal(1, _context.DeviceCabinets.Where(dc => dc.DeviceID == newDevice.ID).Count());
            Assert.Equal(targetDeviceCabinetID, _context.DeviceCabinets.FirstOrDefault(dc => dc.DeviceID == newDevice.ID).CabinetID);
        }

        [Fact]
        public void CantMoveDevice()
        {
            // Arrange
            var wrongInvNum = "mess";
            var existingInvNum = _context.Devices.FirstOrDefault().InventoryNumber;

            var wrongCabinetID = Guid.Empty;
            Guid existingCabinetID = _context.Cabinets.FirstOrDefault().ID;

            // Assert
            Assert.Throws<ArgumentException>(() => _editor.MoveDevice(existingInvNum, wrongCabinetID));
            Assert.Throws<ArgumentException>(() => _editor.MoveDevice(wrongInvNum, existingCabinetID));
            Assert.Throws<ArgumentException>(() => _editor.MoveDevice(wrongInvNum, wrongCabinetID));
        }

        [Fact]
        public void CanAddSoftware()
        {
            // Arrange
            Device newDevice = CreateDevice();

            var softInfo1 = new SoftwareInfo {
                SoftwareName = Guid.NewGuid().ToString(),
                SoftwareVersion = Guid.NewGuid().ToString()
            };

            var softInfo2 = new SoftwareInfo {
                SoftwareName = Guid.NewGuid().ToString(),
            };

            var softInfo3 = new SoftwareInfo {
                SoftwareName = softInfo1.SoftwareName,
                SoftwareVersion = Guid.NewGuid().ToString()
            };

            // Act
            _editor.AddSoftware(newDevice.InventoryNumber, softInfo1);
            _editor.AddSoftware(newDevice.InventoryNumber, softInfo2);
            _editor.AddSoftware(newDevice.InventoryNumber, softInfo3);

            // Assert
            var firstResult = _context.DeviceSoftware.FirstOrDefault(ds =>
                (ds.DeviceID == newDevice.ID) && (ds.SoftwareName == softInfo1.SoftwareName)
            );
            Assert.NotNull(firstResult);
            Assert.Equal(softInfo1.SoftwareName, firstResult.SoftwareName);
            Assert.Equal(softInfo1.SoftwareVersion, firstResult.SoftwareVersion);

            var secondResult = _context.DeviceSoftware.FirstOrDefault(
                ds => (ds.DeviceID == newDevice.ID) && (ds.SoftwareName == softInfo2.SoftwareName)
            );
            Assert.NotNull(secondResult);
            Assert.Equal(softInfo2.SoftwareName, secondResult.SoftwareName);
            Assert.Null(secondResult.SoftwareVersion);

            var lastResult = _context.DeviceSoftware.FirstOrDefault(
                ds => (ds.DeviceID == newDevice.ID) && (ds.SoftwareName == softInfo3.SoftwareName)  &&
                    (ds.SoftwareVersion == softInfo3.SoftwareVersion)
            );
            Assert.NotNull(lastResult);
            Assert.Equal(softInfo3.SoftwareName, lastResult.SoftwareName);
            Assert.Equal(softInfo3.SoftwareVersion, lastResult.SoftwareVersion);
        }

        [Fact]
        public void CantAddSoftware()
        {
            // Arrange
            Device newDevice = CreateDevice();

            var validInfo = new SoftwareInfo {
                SoftwareName = Guid.NewGuid().ToString()
            };

            var wrongInvNum = Guid.NewGuid().ToString();
            var wrongInfo = new SoftwareInfo {
                SoftwareName = null
            };

            // Assert
            Assert.Throws<ArgumentNullException>(() => _editor.AddSoftware(newDevice.InventoryNumber, wrongInfo));
            Assert.Throws<ArgumentException>(() => _editor.AddSoftware(wrongInvNum, validInfo));
            Assert.Throws<InvalidOperationException>(() => {
                var sameSoft = new SoftwareInfo {
                    SoftwareName = Guid.NewGuid().ToString(),
                    SoftwareVersion = Guid.NewGuid().ToString()
                };

                _editor.AddSoftware(newDevice.InventoryNumber, sameSoft);
                _editor.AddSoftware(newDevice.InventoryNumber, sameSoft);
            });
        }

        [Fact]
        public void CanRemoveSoftware()
        {
            // Arrange
            Device newDevice = CreateDevice();

            var softInfo = new SoftwareInfo {
                SoftwareName = Guid.NewGuid().ToString(),
                SoftwareVersion = Guid.NewGuid().ToString()
            };

            _context.DeviceSoftware.Add(new DeviceSoftware {
                DeviceID = newDevice.ID,
                SoftwareName = softInfo.SoftwareName,
                SoftwareVersion = softInfo.SoftwareVersion
            });

            _context.SaveChanges();

            // Act
            _editor.RemoveSoftware(newDevice.InventoryNumber, softInfo);

            // Assert
            DeviceSoftware result = _context.DeviceSoftware.FirstOrDefault(ds => (ds.DeviceID == newDevice.ID) && (ds.SoftwareName == softInfo.SoftwareName));
            Assert.Null(result);
        }

        [Fact]
        public void CanAddIP()
        {
            // Arrange
            Device newDevice = CreateDevice();

            IPAddress freeIP = GetFreeIP();

            // Act
            _editor.AddIPAddress(newDevice.InventoryNumber, freeIP.Address);
            IPAddress addedIP = GetDeviceIP(newDevice.ID);

            // Assert
            Assert.NotNull(addedIP);
            Assert.Equal(freeIP.Address, addedIP.Address);
        }

        [Fact]
        public void CantAddIP()
        {
            // Arrange
            Device newDevice = CreateDevice();

            IPAddress busyIP = _context.IPAddresses.FirstOrDefault(ip => ip.DeviceID != null);
            IPAddress freeIP = GetFreeIP();

            // Assert
            Assert.Throws<ArgumentException>(() => _editor.AddIPAddress(newDevice.InventoryNumber, "256.0.-1.0"));
            Assert.Throws<ArgumentException>(() => _editor.AddIPAddress(null, freeIP.Address));
            Assert.Throws<ArgumentException>(() => _editor.AddIPAddress("wrongInvNum", freeIP.Address));
            Assert.Throws<ArgumentNullException>(() => _editor.AddIPAddress(newDevice.InventoryNumber, null));
            Assert.Throws<InvalidOperationException>(() => _editor.AddIPAddress(newDevice.InventoryNumber, busyIP.Address));
        }

        [Fact]
        public void CanRemoveIP()
        {
            // Arrange
            Device newDevice = CreateDevice();

            IPAddress addedIP = GetFreeIP();

            addedIP.DeviceID = newDevice.ID;
            _context.IPAddresses.Update(addedIP);
            _context.SaveChanges();

            // Act
            _editor.RemoveIPAddress(newDevice.InventoryNumber, addedIP.Address);

            // Assert
            Assert.Null(GetDeviceIP(newDevice.ID));
        }

        [Fact]
        public void CantRemoveIP()
        {
            // Arrange
            Device newDevice = CreateDevice();

            IPAddress busyIP = _context.IPAddresses.FirstOrDefault(ip => ip.DeviceID != null);
            IPAddress freeIP = GetFreeIP();
            freeIP.DeviceID = newDevice.ID;
            _context.IPAddresses.Update(freeIP);
            _context.SaveChanges();

            // Assert
            Assert.Throws<InvalidOperationException>(() => _editor.RemoveIPAddress(newDevice.InventoryNumber, busyIP.Address));
            Assert.Throws<ArgumentException>(() => _editor.RemoveIPAddress(null, freeIP.Address));
            Assert.Throws<ArgumentException>(() => _editor.RemoveIPAddress("", freeIP.Address));
            Assert.Throws<ArgumentNullException>(() => _editor.RemoveIPAddress(newDevice.InventoryNumber, null));
        }

        private Device CreateDevice()
        {
            var newDevice = new Device {
                InventoryNumber = Guid.NewGuid().ToString(),
                TypeID = _context.DeviceTypes.FirstOrDefault().ID,
                NetworkName = Guid.NewGuid().ToString(),
                ModelName = Guid.NewGuid().ToString()
            };

            _context.Devices.Add(newDevice);
            _context.SaveChanges();

            return newDevice;
        }

        private IPAddress GetFreeIP() =>
            _context.IPAddresses.FirstOrDefault(ip => ip.DeviceID == null);

        private IPAddress GetDeviceIP(Guid deviceID) =>
            _context.IPAddresses.FirstOrDefault(ip => ip.DeviceID == deviceID);
    }
}
