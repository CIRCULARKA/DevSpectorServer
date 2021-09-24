using System.Linq;
using System.Collections.Generic;
using Xunit;
using Moq;
using InvMan.Common.Models;
using InvMan.Server.Domain;
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
			_expected = new List<Appliance> {
				new Appliance(1, "inv1", "type1", "net1", "h1", "cab1",
					new List<string> { "1.1.1.1", "2.2.2.2" }
				),
				new Appliance(2, "inv2", "type2", "net2", "h2", "cab2",
					new List<string> { "3.3.3.3", "4.4.4.4" }
				)
			};

			var mock = new Mock<IDeviceRepository>();
			mock.Setup(
				repo => repo.AllDevices
			).Returns(
				new List<Device> {
					new Device {
						ID = 1,
						InventoryNumber = "inv1",
						NetworkName = "net1",
						Type = new DeviceType { Name = "type1" },
						Location = new Location {
							Cabinet = new Cabinet { Name = "cab1" },
							Housing = new Housing { Name = "h1" }
						},
						IPAddresses = new List<IPAddress> {
							new IPAddress { Address = "1.1.1.1" },
							new IPAddress { Address = "2.2.2.2" },
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
						},
						IPAddresses = new List<IPAddress> {
							new IPAddress { Address = "3.3.3.3" },
							new IPAddress { Address = "4.4.4.4" },
						}
					}
				});

			_controller = new DevicesController(mock.Object);
		}

		[Fact]
		public void AreDevicesReturnProperly()
		{
			// Act
			var actual = _controller.Get().ToList();

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
