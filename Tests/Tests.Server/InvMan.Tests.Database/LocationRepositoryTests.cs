using System.Linq;
using Xunit;
using InvMan.Server.Database;

namespace InvMan.Tests.Server
{
	// Look at models configurations that TestDbContext applies in order to
	// examine what data tested
	public class LocationRepositoryTests
	{
		// [Fact]
		// public void IsSpecifiedHousingCabinetsReturnedProperly()
		// {
		// 	// Arrange
		// 	var context = new TestDbContext();
		// 	var cabinetsCount = context.Cabinets.Count();
		// 	var housingCabinetsCount = context.HousingCabinets.Count();
		// 	var repo = new LocationRepository(context);
		// 	var expectedCabinetsCount = 6;

		// 	// Act
		// 	var actual = repo.GetHousingCabinets(2).ToList();

		// 	// Assert
		// 	Assert.Equal(expectedCabinetsCount, actual.Count);
		// 	for (int i = 0; i < expectedCabinetsCount; i++)
		// 		// 4 is first cabinet ID that second housing has
		// 		Assert.Equal(4 + i, actual[i].ID);
		// }
	}
}
