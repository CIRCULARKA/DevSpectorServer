using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DevSpector.UI.API.Controllers;
using DevSpector.Application.Devices;
using DevSpector.Database.DTO;
using DevSpector.Tests;
using DevSpector.Tests.Database;

namespace DevSpector.Tests.Controllers
{
    [Collection(nameof(DatabaseCollection))]
    public class DevicesControllerTests
    {
        [Fact]
        public void ReturnsOutputDevices()
        {
            // Arrange
            var expectedDevices = new List<DeviceToOutput>();

            for (int i = 0; i < 5; i++)
            {
                expectedDevices.Add(new DeviceToOutput {
                    ID = Guid.NewGuid(),
                    InventoryNumber = Guid.NewGuid().ToString(),
                    Cabinet = Guid.NewGuid().ToString(),
                    Housing = Guid.NewGuid().ToString(),
                    ModelName = Guid.NewGuid().ToString(),
                    Type = Guid.NewGuid().ToString(),
                    NetworkName = Guid.NewGuid().ToString(),
                    IPAddresses = new List<string> {
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString()
                    },
                    Software = new List<string> {
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString()
                    }
                });
            }

            var providerMock = new Mock<IDevicesProvider>();
            providerMock.
                Setup(p => p.GetDevicesToOutput()).
                Returns(expectedDevices);

            var editorMock = new Mock<IDevicesEditor>();

            var controller = new DevicesController(
                providerMock.Object,
                editorMock.Object
            );

            // Act
            var actualDevices = controller.GetDevices().Value as List<DeviceToOutput>;

            // Assert
            Assert.Equal(expectedDevices, actualDevices);
            Assert.Equal(expectedDevices.Count, actualDevices.Count);
            for (int i = 0; i < expectedDevices.Count; i++)
            {
                Assert.Equal(expectedDevices[i].ID, actualDevices[i].ID);
                Assert.Equal(expectedDevices[i].InventoryNumber, actualDevices[i].InventoryNumber);
                Assert.Equal(expectedDevices[i].ModelName, actualDevices[i].ModelName);
                Assert.Equal(expectedDevices[i].NetworkName, actualDevices[i].NetworkName);
                Assert.Equal(expectedDevices[i].Type, actualDevices[i].Type);

                Assert.Equal(expectedDevices[i].IPAddresses.Count, actualDevices[i].IPAddresses.Count);
                for (int j = 0; j < expectedDevices[i].IPAddresses.Count; j++)
                    Assert.Equal(expectedDevices[i].IPAddresses[j], actualDevices[i].IPAddresses[j]);

                Assert.Equal(expectedDevices[i].Software.Count, actualDevices[i].Software.Count);
                for (int j = 0; j < expectedDevices[i].Software.Count; j++)
                    Assert.Equal(expectedDevices[i].Software[j], actualDevices[i].Software[j]);
            }
        }
    }
}
