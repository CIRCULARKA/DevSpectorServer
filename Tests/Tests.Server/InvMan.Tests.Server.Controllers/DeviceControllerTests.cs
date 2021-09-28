using System.Linq;
using System.Collections.Generic;
using Xunit;
using Moq;
using InvMan.Server.Domain;
using InvMan.Common.SDK.Models;
using InvMan.Server.Application;
using InvMan.Server.Domain.Models;
using InvMan.Server.UI.API.Controllers;

namespace InvMan.Tests.Server.Controllers
{
	public class DeviceControllerTests
	{
		private readonly DevicesController _controller;

		private readonly List<Appliance> _expected;

		public DeviceControllerTests()
		{
			var testDevices = new List<Device> {
				new Device {
					ID = 1,
					InventoryNumber = "inv1",
					NetworkName = "net1",
					Type = new DeviceType { Name = "type1" },
					Location = new Location {
						Cabinet = new Cabinet { Name = "cab1" },
						Housing = new Housing { Name = "h1" }
					}
				},
				new Device {
					ID = 2,
					InventoryNumber = "inv2",
					NetworkName = "net2",
					Type = new DeviceType { Name = "type2" },
					Location = new Location {
						Cabinet = new Cabinet { Name = "cab2" },
						Housing = new Housing { Name = "h2" }
					}
				}
			};

			var testIPs = new List<IPAddress> {
				new IPAddress { Address = "1.1.1.1" },
				new IPAddress { Address = "2.2.2.2" },
				new IPAddress { Address = "3.3.3.3" },
				new IPAddress { Address = "4.4.4.4" },
			};

			var testDeviceIPs = new List<DeviceIPAddresses> {
				new DeviceIPAddresses { Device = testDevices[0], IPAddress = testIPs[0] },
				new DeviceIPAddresses { Device = testDevices[0], IPAddress = testIPs[1] },
				new DeviceIPAddresses { Device = testDevices[1], IPAddress = testIPs[2] },
				new DeviceIPAddresses { Device = testDevices[1], IPAddress = testIPs[3] }
			};

			_expected = new List<Appliance> {
				new Appliance(testDevices[0].ID, testDevices[0].InventoryNumber,
					testDevices[0].Type.Name, testDevices[0].NetworkName,
					testDevices[0].Location.Housing.Name, testDevices[0].Location.Cabinet.Name,
					new List<string> { testIPs[0].Address, testIPs[0].Address }
				),
				new Appliance(testDevices[1].ID, testDevices[1].InventoryNumber,
					testDevices[1].Type.Name, testDevices[1].NetworkName,
					testDevices[1].Location.Housing.Name, testDevices[1].Location.Cabinet.Name,
					new List<string> { testIPs[1].Address, testIPs[1].Address }
				)
			};

			var devicesRepoMock = new Mock<IDeviceRepository>();
			devicesRepoMock.Setup(
				repo => repo.Devices
			).Returns(testDevices.AsQueryable());

			var ipRepoMock = new Mock<IIPAddressRepository>();
			ipRepoMock.Setup(repo => repo.IPAddresses).Returns(testIPs.AsQueryable());
			ipRepoMock.Setup(repo => repo.DeviceIPAddresses).Returns(
				testDeviceIPs.AsQueryable()
			);

			var devicesManagerMock = new Mock<IDevicesManager>();
			devicesManagerMock.Setup(
				dm => dm.GetAppliances(2)
			).Returns(
				new List<Appliance> {
					new Appliance(testDevices[0].ID, testDevices[0].InventoryNumber,
						testDevices[0].Type.Name, testDevices[0].NetworkName,
						testDevices[0].Location.Housing.Name,
						testDevices[0].Location.Cabinet.Name,
						_expected[0].IPAddresses
					),
					new Appliance(testDevices[1].ID, testDevices[1].InventoryNumber,
						testDevices[1].Type.Name, testDevices[1].NetworkName,
						testDevices[1].Location.Housing.Name,
						testDevices[1].Location.Cabinet.Name,
						_expected[1].IPAddresses
					)
				}.AsQueryable()
			);

			_controller = new DevicesController(devicesManagerMock.Object);
		}

		[Fact]
		public void AreDevicesReturnProperly()
		{
			// Act
			var actual = _controller.Get(_expected.Count).Cast<Appliance>().ToList();

			// Assert
			Assert.Equal(_expected.Count, actual.Count);
			for (int i = 0; i < _expected.Count; i++)
			{
				var expectedIP = _expected[i].IPAddresses.ToList();
				var actualIP = actual[i].IPAddresses.ToList();
				Assert.Equal(_expected[i].ID, actual[i].ID);
				Assert.Equal(_expected[i].InventoryNumber, actual[i].InventoryNumber);
				Assert.Equal(_expected[i].NetworkName, actual[i].NetworkName);
				Assert.Equal(_expected[i].Housing, actual[i].Housing);
				Assert.Equal(_expected[i].Cabinet, actual[i].Cabinet);
				Assert.Equal(expectedIP.Count, actualIP.Count);
				for (int j = 0; j < expectedIP.Count; j++)
					Assert.Equal(expectedIP[j], actualIP[j]);
			}
		}
	}
}
