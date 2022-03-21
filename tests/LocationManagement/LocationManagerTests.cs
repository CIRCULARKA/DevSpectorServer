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
    }
}
