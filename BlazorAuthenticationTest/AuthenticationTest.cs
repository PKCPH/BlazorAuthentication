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
        cut.MarkupMatches("<h1>Authentication Test Page</h1><div><h1>Hello, testuser</h1>You are authenticated.</div>");

    }

    [Fact]
    public void ShowsNotAuthenticatedMessage_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        authContext.SetAuthorized("");

        // Act
        var cut = ctx.RenderComponent<TestAuthentication>();

        // Assert
        cut.MarkupMatches("<h1>Authentication Test Page</h1>\r\n<div>\r\n  <h1>Hello,\r\n  </h1>\r\n  You are authenticated.\r\n</div>");

    }
}
