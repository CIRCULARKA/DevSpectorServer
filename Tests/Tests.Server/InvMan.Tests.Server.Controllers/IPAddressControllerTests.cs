using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using Xunit;
using Moq;
using InvMan.Server.Domain;
using InvMan.Server.UI.API.Controllers;
using InvMan.Server.Domain.Models;

namespace InvMan.Tests.Server.Controllers
{
	public class IPAddressControllerTests
	{
		[Fact]
		public void IsFreeIPsReturnedProperly()
		{
			// Arrange
			var expected = new List<IPAddress>
			{
				new IPAddress { Address = "1.1.1.1" },
				new IPAddress { Address = "2.2.2.2" },
				new IPAddress { Address = "3.3.3.3" },
				new IPAddress { Address = "4.4.4.4" }
			};

			var repoMock = new Mock<IIPAddressRepository>();
			repoMock.Setup(
				ipRepo => ipRepo.FreeAddresses
			).Returns(expected);

			var ipController = new IPAddressController(repoMock.Object);

			// Act
			var actual = ipController.Get().ToList();

			// Assert
			Assert.Equal(expected.Count, actual.Count);

			for (int i = 0; i < expected.Count; i++)
				Assert.Equal(expected[i].Address, actual[i].Address);
		}
	}
}
