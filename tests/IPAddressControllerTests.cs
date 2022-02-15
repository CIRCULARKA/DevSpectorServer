using System.Linq;
using System.Collections.Generic;
using Xunit;
using Moq;
using DevSpector.Domain;
using DevSpector.UI.API.Controllers;
using DevSpector.Domain.Models;

namespace DevSpector.Server.Tests.Server.Controllers
{
	public class IPAddressControllerTests
	{
		// private readonly List<IPAddress> _expectedIPs;

		// private readonly IIPAddressRepository _repoMock;

		// private readonly IPAddressController _controller;

		// public IPAddressControllerTests()
		// {
		// 	_expectedIPs = new List<IPAddress>
		// 	{
		// 		new IPAddress { Address = "1.1.1.1" },
		// 		new IPAddress { Address = "2.2.2.2" },
		// 		new IPAddress { Address = "3.3.3.3" },
		// 		new IPAddress { Address = "4.4.4.4" }
		// 	};

		// 	var repoMock = new Mock<IIPAddressRepository>();
		// 	repoMock.Setup(
		// 		ipRepo => ipRepo.FreeAddresses
		// 	).Returns(_expectedIPs);
		// 	repoMock.Setup(
		// 		ipRepo => ipRepo.GetDeviceIPs(1)
		// 	).Returns(_expectedIPs);

		// 	_controller = new IPAddressController(repoMock.Object);
		// }

		// [Fact]
		// public void IsFreeIPsReturnedProperly()
		// {
		// 	// Act
		// 	var actual = _controller.Get().ToList();

		// 	// Assert
		// 	Assert.Equal(_expectedIPs.Count, actual.Count);

		// 	for (int i = 0; i < _expectedIPs.Count; i++)
		// 		Assert.Equal(_expectedIPs[i].Address, actual[i].Address);
		// }

		// [Fact]
		// public void IsDeviceIpsReturnsPropelry()
		// {
		// 	// Act
		// 	var actual = _controller.Get(1).Cast<string>().ToList();

		// 	// Assert
		// 	Assert.Equal(_expectedIPs.Count, actual.Count);

		// 	for (int i = 0; i < _expectedIPs.Count; i++)
		// 		Assert.Equal(_expectedIPs[i].Address, actual[i]);
		// }
	}
}
