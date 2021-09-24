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
		private readonly IDataProvider _mockDataProvider;

		public DevicesProviderTests()
		{
			var moq = new Mock<IDataProvider>();
			moq.Setup(provider => provider.GetAllDevicesRawAsync()).
				Returns(
					Task.FromResult<string>(
						@"[
							{
								""id"": 1,
								""inventoryNumber"": ""inv1"",
								""networkName"": ""net1"",
								""type"": ""type1"",
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
				new Appliance( 1, "inv1", "type1", "net1",
					new List<string> {
						"1.1.1.1",
						"2.2.2.2",
					}
				),
				new Appliance( 2, "inv2", "type2", "net2",
					new List<string> {
						"3.3.3.3",
						"4.4.4.4",
					}
				),
			};

			// Act
			var actual = (await provider.GetAllDevicesAsync()).ToList();

			// Assert
			Assert.True(actual.Count() == expected.Count());

			for (int i = 0; i < expected.Count(); i++)
			{
				Assert.Equal(expected[i].ID, actual[i].ID);
				Assert.Equal(expected[i].InventoryNumber, actual[i].InventoryNumber);
				Assert.Equal(expected[i].NetworkName, actual[i].NetworkName);
				Assert.Equal(expected[i].IPAddresses.Count(), actual[i].IPAddresses.Count());
				for (int j = 0; j < expected[i].IPAddresses.Count(); j++)
				{
					Assert.Equal(expected[i].IPAddresses[j], actual[i].IPAddresses[j]);
				}
			}
		}
	}
}
