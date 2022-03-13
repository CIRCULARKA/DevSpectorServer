using DevSpector.Application.Networking;
using DevSpector.Application.Enumerations;
using Xunit;

namespace DevSpector.Tests.Application.Networking
{
    public class IPValidatorTests
    {
        private class Expectation
        {
            public string IP { get; init; }

            public bool IsValid { get; init; }
        }

        [Fact]
        public void IPv4ValidatesRight()
        {
            // Arrange
            var validator = new IPValidator();

            var expectations = new Expectation[] {
                new Expectation { IP = "192.13.1.0", IsValid = true },
                new Expectation { IP = "255.0.0.0.", IsValid = false },
                new Expectation { IP = "0.256.0.0", IsValid = false },
                new Expectation { IP = "01.255.0.0", IsValid = false },
                new Expectation { IP = "1.255.0.0", IsValid = true },
                new Expectation { IP = "1.0.0", IsValid = false },
                new Expectation { IP = "1..0.0", IsValid = false }
            };

            var results = new bool[expectations.Length];

            // Act
            for (int i = 0; i< expectations.Length; i++)
                results[i] = validator.Matches(expectations[i].IP, IPProtocol.Version4);

            // Assert
            for (int i = 0; i < results.Length; i++)
                Assert.True(expectations[i].IsValid == results[i]);
        }
    }
}
