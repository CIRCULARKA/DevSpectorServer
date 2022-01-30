using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;
using Xunit;
using InvMan.Common.SDK;
using InvMan.Common.SDK.Models;

namespace InvMan.Tests.Server.SDK
{
	public class DevicesProviderTests
	{
		private readonly IRawDataProvider _mockDataProvider;

		private readonly Guid[] _mockDevicesGuids;

		public DevicesProviderTests()
		{
			_mockDevicesGuids = new Guid[] {
				new Guid("16036105-5111-4420-8b26-a18deaeb8f9b"),
				new Guid("ed8c1437-07fd-4ce8-beb2-aba831d05e31"),
			};

			var moq = new Mock<IRawDataProvider>();
			moq.Setup(provider => provider.GetDevicesAsync(Guid.Empty.ToString())).
				Returns(
					Task.FromResult<string>(
						@"[
							{
								""id"": ""16036105-5111-4420-8b26-a18deaeb8f9b"",
								""inventoryNumber"": ""inv1"",
								""networkName"": ""net1"",
								""type"": ""type1"",
								""housing"": ""h1"",
								""cabinet"": ""cab1"",
								""ipAddresses"": [
									""1.1.1.1"",
									""2.2.2.2""
								]
							},
							{
								""id"": ""ed8c1437-07fd-4ce8-beb2-aba831d05e31"",
								""inventoryNumber"": ""inv2"",
								""networkName"": ""net2"",
								""type"": ""type2"",
								""housing"": ""h2"",
								""cabinet"": ""cab2"",
								""ipAddresses"": [
									""3.3.3.3"",
									""4.4.4.4""
								]
							}
						]"
					)
				);

			_mockDataProvider = moq.Object;
		}

		[Fact]
		public async void AreDevicesDeserializeProperly()
		{
			// Arrange
			var provider = new DevicesProvider(_mockDataProvider);

			var expected = new List<Appliance>
			{
				new Appliance(_mockDevicesGuids[0], "inv1", "type1", "net1",
					"h1", "cab1",
					new List<string> {
						"1.1.1.1",
						"2.2.2.2",
					},
					null
				),
				new Appliance(_mockDevicesGuids[1], "inv2", "type2", "net2",
					"h2", "cab2",
					new List<string> {
						"3.3.3.3",
						"4.4.4.4",
					},
					null
				),
			};

			// Act
			var actual = (await provider.GetDevicesAsync(Guid.Empty.ToString())).ToList();

			// Assert
			Assert.Equal(expected.Count(), expected.Count());

			for (int i = 0; i < expected.Count(); i++)
			{
				var expectedIPList = expected[i].IPAddresses.ToList();
				var actualIPList = actual[i].IPAddresses.ToList();
				Assert.Equal(expectedIPList.Count, actualIPList.Count);
				Assert.Equal(expected[i].ID, actual[i].ID);
				Assert.Equal(expected[i].InventoryNumber, actual[i].InventoryNumber);
				Assert.Equal(expected[i].NetworkName, actual[i].NetworkName);
				Assert.Equal(expected[i].Housing, actual[i].Housing);
				Assert.Equal(expected[i].Cabinet, actual[i].Cabinet);
				for (int j = 0; j < expected[i].IPAddresses.Count(); j++)
				{
					Assert.Equal(expectedIPList[i], actualIPList[i]);
				}
			}
		}
	}
}
