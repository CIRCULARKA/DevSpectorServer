using System.Linq;
using System.Collections.Generic;
using Xunit;
using DevSpector.Tests.Database;
using DevSpector.Domain;
using DevSpector.Domain.Models;
using DevSpector.Application.Networking;

namespace DevSpector.Tests.Application.Networking
{
    [Collection(nameof(DatabaseCollection))]
    public class IPAddressEditorTests : DatabaseTestBase
    {
        private readonly IIPAddressEditor _provider;

        public IPAddressEditorTests(DatabaseFixture _) : base(_)
        {
            var ipValidator = new IPValidator();

            _provider = new IPAddressEditor(
                base._repo,
                ipValidator,
                new IP4RangeGenerator(ipValidator)
            );
        }

        [Fact]
        public void CanGenerateIPRange()
        {
            // Arrange

            // Big changes to IPAddresses table which will be made by GenerateRange() method in database may break other tests
            // so I decided to execute this particullar test in independent database
            using (var localContext = new TestDbContext("Data Source=./IPRangeTest.db"))
            {
                var localRepo = new Repository(localContext);
                var localEditor = new IPAddressEditor(
                    localRepo,
                    new IPValidator(),
                    new IP4RangeGenerator(new IPValidator())
                );

                var expected = new string[] {
                    "255.2.10.153",
                    "255.2.10.154",
                    "255.2.10.155",
                    "255.2.10.156",
                    "255.2.10.157",
                    "255.2.10.158"
                };

                // Act
                localEditor.GenerateRange("255.2.10.158", 29);
                List<string> actual = localRepo.Get<IPAddress>().Select(ip => ip.Address).ToList();

                // Assert
                Assert.Equal(expected.Length, actual.Count);
                for (int i = 0; i < expected.Length; i++)
                    Assert.Contains(expected[i], actual);
            }
        }
    }
}
