using System.Linq;
using System.Collections.Generic;
using Xunit;
using DevSpector.Application.Location;
using DevSpector.Domain.Models;
using DevSpector.Tests.Database;

namespace DevSpector.Tests.Application.Location
{
    [Collection(nameof(DatabaseCollection))]
    public class LocationManagerTests : DatabaseTestBase
    {
        private readonly ILocationManager _manager;

        public LocationManagerTests(DatabaseFixture _) : base(_)
        {
            _manager = new LocationManager(base._repo);
        }

        [Fact]
        public void ReturnsHousings()
        {
            // Arrange
            List<Housing> expected = _context.Housings.ToList();

            // Act
            List<Housing> actual = _manager.Housings;

            // Assert
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ID, actual[i].ID);
                Assert.Equal(expected[i].Name, actual[i].Name);
            }
        }

        [Fact]
        public void ReturnsHousingCabinets()
        {
            // Arrange
            var housings = _context.Housings.ToList();

            var expected = new List<List<Cabinet>>();
            for (int i = 0; i < housings.Count; i++)
            {
                List<Cabinet> cabinets = _context.Cabinets.Where(c => c.HousingID == housings[i].ID).ToList();
                expected.Add(cabinets);
            }

            // Act
            var actual = new List<List<Cabinet>>();

            for (int i = 0; i < housings.Count; i++)
            {
                List<Cabinet> actualCabinets = _manager.GetCabinets(housings[i].ID);
                actual.Add(actualCabinets);
            }

            // Assert
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < housings.Count; i++)
            {
                Assert.Equal(expected[i].Count, actual[i].Count);
                for (int j = 0; j < expected[i].Count; j++)
                {
                    Assert.Equal(expected[i][j].ID, actual[i][j].ID);
                    Assert.Equal(expected[i][j].Name, actual[i][j].Name);
                    Assert.Equal(expected[i][j].HousingID, actual[i][j].HousingID);
                    Assert.NotNull(actual[i][j].Housing);
                }
            }
        }
    }
}
