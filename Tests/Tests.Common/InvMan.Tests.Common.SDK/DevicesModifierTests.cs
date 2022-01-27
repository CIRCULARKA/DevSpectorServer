using System.Threading.Tasks;
using Xunit;
using InvMan.Common.SDK;

namespace InvMan.Tests.Server.SDK
{
	public class DevicesModifierTests
	{
		public DevicesModifierTests() { }

		[Fact]
		public async Task CanCreateDevice()
		{
			var modifier = new DevicesModifier();
			await modifier.CreateDevice("TestNetworkName1", "TestInventoryNumber1", "Сервер");
		}
	}
}
