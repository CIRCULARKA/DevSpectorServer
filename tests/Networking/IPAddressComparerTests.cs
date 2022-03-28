using Xunit;
using DevSpector.Domain.Models;
using System.Collections.Generic;

namespace DevSpector.Tests.Database
{
    public class IPAddressComparerTests
    {
        [Fact]
        public void ComparesRight()
        {
            // Arrange
            var comparer = new IPAddressComparer();

            var truePair1 = new KeyValuePair<IPAddress, IPAddress>(
                null, null
            );

            var truePair2 = new KeyValuePair<IPAddress, IPAddress>(
                new IPAddress(), new IPAddress()
            );

            var ip1 = new IPAddress();
            ip1.Address = "Equal";

            var ip2 = new IPAddress();
            ip2.Address = "Equal";
            var truePair3 = new KeyValuePair<IPAddress, IPAddress>(
                ip1, ip2
            );

            var falsePair1 = new KeyValuePair<IPAddress, IPAddress>(
                null, new IPAddress()
            );

            var falsePair2 = new KeyValuePair<IPAddress, IPAddress>(
                new IPAddress(), null
            );

            ip1 = new IPAddress();
            ip1.Address = "not equal1";

            ip2 = new IPAddress();
            ip2.Address = "not equal2";

            var falsePair3 = new KeyValuePair<IPAddress, IPAddress>(
                ip1, ip2
            );

            // Act
            var shouldBeTrue1 = comparer.Equals(truePair1.Key, truePair1.Value);
            var shouldBeTrue2 = comparer.Equals(truePair2.Key, truePair2.Value);
            var shouldBeTrue3 = comparer.Equals(truePair3.Key, truePair3.Value);

            var shouldBeFalse1 = comparer.Equals(falsePair1.Key, falsePair1.Value);
            var shouldBeFalse2 = comparer.Equals(falsePair2.Key, falsePair2.Value);
            var shouldBeFalse3 = comparer.Equals(falsePair3.Key, falsePair3.Value);

            // Assert
            Assert.True(shouldBeTrue1);
            Assert.True(shouldBeTrue2);
            Assert.True(shouldBeTrue3);

            Assert.False(shouldBeFalse1);
            Assert.False(shouldBeFalse2);
            Assert.False(shouldBeFalse3);
        }
    }
}
