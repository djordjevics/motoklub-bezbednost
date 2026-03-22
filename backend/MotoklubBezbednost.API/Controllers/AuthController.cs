using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MotoklubBezbednost.API.Options;

namespace MotoklubBezbednost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly LocalAuthOptions _auth;
    private readonly JwtOptions _jwt;

    public AuthController(IOptions<LocalAuthOptions> auth, IOptions<JwtOptions> jwt)
    {
        _auth = auth.Value;
        _jwt = jwt.Value;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest();

        if (!string.Equals(request.Username, _auth.Username, StringComparison.Ordinal)
            || !string.Equals(request.Password, _auth.Password, StringComparison.Ordinal))
            return Unauthorized();

        var expires = DateTime.UtcNow.AddHours(_jwt.ExpiryHours);
        var token = CreateToken(request.Username, expires);
        return Ok(new LoginResponse(token, ExpiresAtUtc: expires));
    }

    private string CreateToken(string username, DateTime expiresUtc)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expiresUtc,
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record LoginRequest(string Username, string Password);

public record LoginResponse(string AccessToken, DateTime ExpiresAtUtc);
