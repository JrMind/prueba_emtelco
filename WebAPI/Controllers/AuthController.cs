using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Inicia sesión para obtener un token JWT.
    /// </summary>
    /// <param name="request">Credenciales del usuario, en este caso como es una prueba, el usuario es prueba y su contrasena prueba, se expone esta informacion para que no haya errores al momento de logearse.</param>
    /// <returns>Token JWT si las credenciales son válidas.</returns>
    /// <response code="200">Token generado exitosamente</response>
    /// <response code="401">Credenciales inválidas</response>
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(typeof(object), 401)]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Usuario de prueba
        if (request.Username == "prueba" && request.Password == "prueba")
        {
            var token = GenerateToken(request.Username);
            return Ok(new { token });
        }

        return Unauthorized(new { error = "Credenciales inválidas" });
    }

    private string GenerateToken(string username)
    {
        var jwtKey = _config["Jwt:Key"];
        var jwtIssuer = _config["Jwt:Issuer"];
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
