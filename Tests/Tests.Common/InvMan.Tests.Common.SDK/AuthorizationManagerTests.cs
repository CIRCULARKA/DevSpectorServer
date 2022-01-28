using System;
using System.Threading.Tasks;
using Xunit;
using InvMan.Common.SDK.Authorization;

namespace InvMan.Tests.Common.SDK.Authorization
{
	public class AuthorizationManagerTests
    {
        [Fact]
        public async Task CanGetAccessToken()
        {
            // Arrange
            var manager = new AuthorizationManager();
            var login = "ruslan";
            var password = "123Abc!";

            var expected = "51d45c0c-3811-4514-9fe5-1cb936a1c421";

            // Act
            var actual = await manager.GetAccessTokenAsync(login, password);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CannotGetAccessToken()
        {
            // Arrange
            var manager = new AuthorizationManager();
            var login = "ruslan";
            var password = "wrongPassword";

            // Assert
            Assert.ThrowsAsync(
                new ArgumentException().GetType(),
                async () => await manager.GetAccessTokenAsync(login, password)
            );
        }
    }
}
