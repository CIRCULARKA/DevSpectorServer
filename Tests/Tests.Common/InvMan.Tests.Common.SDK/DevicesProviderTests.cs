using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;
using Xunit;
using InvMan.Common.SDK;
using InvMan.Common.Models;

namespace InvMan.Tests.Server.SDK
{
	public class DevicesProviderTests
	{
		private readonly IRawDataProvider _mockDataProvider;

		public DevicesProviderTests()
		{
			var moq = new Mock<IRawDataProvider>();
			moq.Setup(provider => provider.GetDevicesAsync()).
				Returns(
					Task.FromResult<string>(
						@"[
							{
								""id"": 1,
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
								""id"": 2,
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
		public async void IsDevicesDeserializeProperly()
		{
			// Arrange
			var provider = new DevicesProvider(_mockDataProvider);

			var expected = new List<Appliance>
			{
				new Appliance(1, "inv1", "type1", "net1",
					"h1", "cab1",
					new List<string> {
						"1.1.1.1",
						"2.2.2.2",
					}
				),
				new Appliance(2, "inv2", "type2", "net2",
					"h2", "cab2",
					new List<string> {
						"3.3.3.3",
						"4.4.4.4",
					}
				),
			};

			// Act
			var actual = (await provider.GetDevicesAsync()).ToList();

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
