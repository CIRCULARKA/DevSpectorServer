using System;
using System.Threading.Tasks;
using Xunit;
using InvMan.Common.SDK.Authorization;
using InvMan.Common.SDK.Models;

namespace InvMan.Tests.Common.SDK.Authorization
{
	public class AuthorizationManagerTests
    {
        [Fact]
        public async Task CanGetUser()
        {
            // Arrange
            var manager = new AuthorizationManager();

            var expectedLogin = "TestAdministrator";
            var expectedGroup = "Администратор";
            var expectedAccessToken = "2d436de5-ca4c-441f-9708-c5e5f955d955";

            var password = "Admin1!";

            var expected = new User(expectedAccessToken, expectedLogin, expectedGroup);

            // Act
            var actual = await manager.TrySignIn(expectedLogin, password);

            // Assert
            Assert.Equal(expected.AccessToken, actual.AccessToken);
            Assert.Equal(expected.Login, actual.Login);
            Assert.Equal(expected.Group, actual.Group);

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await manager.TrySignIn(expectedLogin, password + "1")
            );
        }
    }
}
