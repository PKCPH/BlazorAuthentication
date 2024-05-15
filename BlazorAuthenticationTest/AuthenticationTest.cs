using Bunit;
using Bunit.TestDoubles;
using BlazorAuthenticationWeb.Components.Pages;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.AspNetCore.Identity;
using BlazorAuthenticationWeb.Handlers;
using System.Runtime.InteropServices;

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

    [Fact]
    public void ShowsAuthorizedUserContent_WhenUserIsAuthorized()
    {
        // Arrange
        var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        authContext.SetAuthorized("testuser", AuthorizationState.Unauthorized);
        authContext.SetRoles("ADMIN");

        // Act
        var cut = ctx.RenderComponent<TestAuthorization>();

        // Assert
        cut.MarkupMatches("<h1>Authentication Test Page</h1>\r\n<div>\r\n  <h1>Hello, testuser</h1>\r\n  <h3>You are admin!</h3>\r\n</div>");
    }

    [Fact]
    public void ShowsNotAuthorizedUserContent_WhenUserIsAuthorized()
    {
        // Arrange
        var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        authContext.SetAuthorized("testuser", AuthorizationState.Unauthorized);
        authContext.SetRoles("USER");

        // Act
        var cut = ctx.RenderComponent<TestAuthorization>();

        // Assert
        cut.MarkupMatches("<h1>Authentication Test Page</h1>\r\n<div>\r\n  <h1>Hello, testuser</h1>\r\n  <h3>You are not admin sadly.</h3>\r\n</div>");
    }

    [Fact]
    public void SimpleComponentViewMarkupTest()
    {
        // Arrange
        var context = new TestContext();

        // Act
        var codeUnderTest = context.RenderComponent<Home>();

        // Assert
        codeUnderTest.MarkupMatches("<h1>Hello, world!</h1>\r\n\r\nWelcome to your new app.");
    }

    [Fact]
    public void ShowsAuthorizedCode_WhenFileCreatedSuccesfully()
    {
        // TODO - Should mock filesystem instead of actual creation and deletion of files.

        // Arrange
        var filePath = "";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            filePath = @"C:\temp\backlog.txt";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            filePath = @"/home/martin/backlog.txt";
        }
        var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        authContext.SetAuthorized("");
        authContext.SetRoles("ADMIN");

        // Act
        var cut = ctx.RenderComponent<SaveFile>();
        var saveFileObj = cut.Instance;

        saveFileObj.WriteFile();

        // Assert
        Assert.True(File.Exists(filePath));

        // Cleaning up test file
        File.Delete(filePath);
    }
}
