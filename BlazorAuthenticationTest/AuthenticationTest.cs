using Bunit;
using Bunit.TestDoubles;
using BlazorAuthenticationWeb.Components.Pages;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.AspNetCore.Identity;
using BlazorAuthenticationWeb.Handlers;

namespace BlazorAuthenticationTest;

public class AuthenticationTest : TestContext
{
    [Fact]
    public void ShowsAuthenticatedUserContent_WhenUserIsAuthenticated()
    {
        // Arrange
        var ctx = new TestContext();
        var roleHandler = new RoleHandler();
        ctx.Services.AddSingleton(roleHandler);
        var authContext = ctx.AddTestAuthorization();
        authContext.SetAuthorized("testuser", AuthorizationState.Unauthorized);
        authContext.SetRoles("admin");

        // Act
        var cut = ctx.RenderComponent<TestAuthentication>();

        // Assert
        cut.MarkupMatches("<h1>Authentication Test Page</h1><div><h1>Hello, testuser</h1></div><div><button>Make admin</button></div>");

    }


}
