using System;
using DevSpector.Application.Networking;
using Xunit;

namespace DevSpector.Tests.Application.Networking
{
    public class IP4RangeGeneratorTests
    {
        [Fact]
        public void IPsGeneratesProperly()
        {
            // Arrange
            var generator = new IP4RangeGenerator(new IPValidator());

            var expected = new string[] {
               "255.2.10.153",
               "255.2.10.154",
               "255.2.10.155",
               "255.2.10.156",
               "255.2.10.157",
               "255.2.10.158"
            };

            // Act
            var actual = generator.GenerateRange("255.2.10.158", 29);

            // Arrange
            Assert.Equal(expected.Length, actual.Count);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void CantProceedWithWrongInputs()
        {
            // Arrange
            var generator = new IP4RangeGenerator(new IPValidator());

            // Act & Assert
            Assert.Throws<ArgumentException>(() => {
                generator.GenerateRange("198.2.2.2", 33);
            });

            Assert.Throws<ArgumentException>(() => {
                generator.GenerateRange("198.2.2.2", 2);
            });

            Assert.Throws<ArgumentException>(() => {
                generator.GenerateRange("198.2.2.2.", 24);
            });

            Assert.Throws<ArgumentException>(() => {
                generator.GenerateRange("198.2.2.2.", 33);
            });
        }
    }
}
