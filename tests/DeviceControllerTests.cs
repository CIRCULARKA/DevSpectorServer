using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using DevSpector.Domain;
using DevSpector.Server.SDK.Models;
using DevSpector.Application;
using DevSpector.Domain.Models;
using DevSpector.UI.API.Controllers;

namespace DevSpector.Server.Tests.Server.Controllers
{
	public class DeviceControllerTests
	{
		private readonly DevicesController _controller;

		private readonly List<Appliance> _expected;

		public DeviceControllerTests()
		{
			var testDevices = new List<Device> {
				new Device {
					ID = Guid.NewGuid(),
					InventoryNumber = "inv1",
					NetworkName = "net1",
					Type = new DeviceType { Name = "type1" },
					Location = new Location {
						Cabinet = new Cabinet { Name = "cab1" },
						Housing = new Housing { Name = "h1" }
					}
				},
				new Device {
					ID = Guid.NewGuid(),
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
				new IPAddress { ID = Guid.NewGuid(), Address = "1.1.1.1", DeviceID = testDevices[0].ID },
				new IPAddress { ID = Guid.NewGuid(), Address = "2.2.2.2", DeviceID = testDevices[0].ID },
				new IPAddress { ID = Guid.NewGuid(), Address = "3.3.3.3", DeviceID = testDevices[1].ID },
				new IPAddress { ID = Guid.NewGuid(), Address = "4.4.4.4", DeviceID = testDevices[1].ID },
			};

			_expected = new List<Appliance> {
				new Appliance(testDevices[0].ID, testDevices[0].InventoryNumber,
					testDevices[0].Type.Name, testDevices[0].NetworkName,
					testDevices[0].Location.Housing.Name, testDevices[0].Location.Cabinet.Name,
					new List<string> { testIPs[0].Address, testIPs[1].Address },
					null
				),
				new Appliance(testDevices[1].ID, testDevices[1].InventoryNumber,
					testDevices[1].Type.Name, testDevices[1].NetworkName,
					testDevices[1].Location.Housing.Name, testDevices[1].Location.Cabinet.Name,
					new List<string> { testIPs[2].Address, testIPs[3].Address },
					null
				)
			};

			var repoMock = new Mock<IRepository>();
			repoMock.Setup(
				repo => repo.Get<Device>(null, null, "")
			).Returns(testDevices);

			// repoMock.Setup(
			// 	repo => repo.Get<IPAddress>(ip => ip.DeviceID == Guid.NewGuid(), null, "")
			// ).Returns(testIPs);

			var devicesManagerMock = new DevicesManager(
				repoMock.Object
			);

			_controller = new DevicesController(
				devicesManagerMock
			);
		}

		[Fact]
		public void CanCreateDevice()
		{
			_controller.CreateDevice("TestNetworkName", "TestInvNum", "Сервер");
		}
	}
}
