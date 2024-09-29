using System.Security.Claims;
using API.Authorization;
using API.Utilities;
using API.Utilities.Extensions;

namespace APITests.Utilities;

public class ClaimsPrincipalExtensionsTests
{
    [Fact]
    public void ClaimsPrincipal_Default_HasNoSpecialPermissions()
    {
        var claims = new ClaimsPrincipal();
        Assert.False(claims.IsAdmin());
        Assert.False(claims.IsSystem());
        Assert.False(claims.IsMatchVerifier());
        Assert.False(claims.IsWhitelisted());
    }

    [Fact]
    public void ClaimsPrincipal_IsAdmin()
    {
        var claims = new ClaimsPrincipal();
        claims.AddIdentity(new ClaimsIdentity(new List<Claim> { new(ClaimTypes.Role, OtrJwtRoles.Admin) }));

        Assert.True(claims.IsAdmin());
    }

    [Fact]
    public void ClaimsPrincipal_IsMatchVerifier()
    {
        var claims = new ClaimsPrincipal();
        claims.AddIdentity(new ClaimsIdentity(new List<Claim> { new(ClaimTypes.Role, OtrJwtRoles.Verifier) }));

        Assert.True(claims.IsMatchVerifier());
    }

    [Fact]
    public void ClaimsPrincipal_IsUser()
    {
        var claims = new ClaimsPrincipal();
        claims.AddIdentity(new ClaimsIdentity(new List<Claim> { new(ClaimTypes.Role, OtrJwtRoles.User) }));

        Assert.True(claims.IsUser());
    }

    [Fact]
    public void ClaimsPrinciple_IsWhitelisted()
    {
        var claims = new ClaimsPrincipal();
        claims.AddIdentity(new ClaimsIdentity(new List<Claim> { new(ClaimTypes.Role, OtrJwtRoles.Whitelist) }));

        Assert.True(claims.IsWhitelisted());
    }

    [Fact]
    public void ClaimsPrincipal_IsAdmin_WhenClaimTypeMapped()
    {
        var claims = new ClaimsPrincipal();
        claims.AddIdentity(new ClaimsIdentity(new List<Claim> { new("role", OtrJwtRoles.Admin) }));

        Assert.True(claims.IsAdmin());
    }

    [Fact]
    public void ClaimsPrincipal_IsMatchVerifier_WhenClaimTypeMapped()
    {
        var claims = new ClaimsPrincipal();
        claims.AddIdentity(new ClaimsIdentity(new List<Claim> { new("role", OtrJwtRoles.Verifier) }));

        Assert.True(claims.IsMatchVerifier());
        Assert.False(claims.IsAdmin());
        Assert.False(claims.IsSystem());
    }

    [Fact]
    public void ClaimsPrincipal_IsUser_WhenClaimTypeMapped()
    {
        var claims = new ClaimsPrincipal();
        claims.AddIdentity(new ClaimsIdentity(new List<Claim> { new("role", OtrJwtRoles.User) }));

        Assert.True(claims.IsUser());
    }

    [Fact]
    public void ClaimsPrinciple_IsWhitelisted_WhenClaimTypeMapped()
    {
        var claims = new ClaimsPrincipal();
        claims.AddIdentity(new ClaimsIdentity(new List<Claim> { new("role", OtrJwtRoles.Whitelist) }));

        Assert.True(claims.IsWhitelisted());
    }

    [Fact]
    public void ClaimsPrincipal_IsNotAdmin_WhenClaimTypeInvalid()
    {
        var claims = new ClaimsPrincipal();
        claims.AddIdentity(new ClaimsIdentity(new List<Claim> { new("123", OtrJwtRoles.Admin) }));

        Assert.False(claims.IsAdmin());
    }

    [Fact]
    public void ClaimsPrincipal_IsNotMatchVerifier_WhenClaimTypeInvalid()
    {
        var claims = new ClaimsPrincipal();
        claims.AddIdentity(new ClaimsIdentity(new List<Claim> { new("123", OtrJwtRoles.Verifier) }));

        Assert.False(claims.IsMatchVerifier());
    }

    [Fact]
    public void ClaimsPrincipal_IsNotUser_WhenClaimTypeInvalid()
    {
        var claims = new ClaimsPrincipal();
        claims.AddIdentity(new ClaimsIdentity(new List<Claim> { new("123", OtrJwtRoles.User) }));

        Assert.False(claims.IsUser());
    }

    [Fact]
    public void ClaimsPrinciple_IsNotWhitelisted_WhenClaimTypeInvalid()
    {
        var claims = new ClaimsPrincipal();
        claims.AddIdentity(new ClaimsIdentity(new List<Claim> { new("123", OtrJwtRoles.Whitelist) }));

        Assert.False(claims.IsWhitelisted());
    }
}
