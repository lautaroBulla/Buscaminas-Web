using Backend.Entities;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        [Authorize(Policy = "Authenticated")]
        [HttpGet("me")]
        public async Task<ActionResult<GetMeDto>> GetMe([FromHeader(Name = "lang")] string lang)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                var message = lang == "en" ? "Unauthenticated user." : "Usuario no autenticado.";
                return Unauthorized(new { message = message });
            }

            var user = await authService.GetMeAsync(Guid.Parse(userIdClaim.Value));
            if (user == null)
            {
                var message = lang == "en" ? "Unauthenticated user." : "Usuario no autenticado.";
                return Unauthorized(new { message = message });
            }

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenResponseDto>> Register(
            UserDto request,
            [FromHeader(Name = "lang")] string lang
            )
        {
            var result = await authService.RegisterAsync(request);
            if (result == null)
            {
                var message = lang == "en" ? "Username already exists." : "El nombre de usuario ya existe.";
                return BadRequest(new { message = message });
            }

            Response.Cookies.Append("Authentication", result.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, //true en produccion
                SameSite = SameSiteMode.Lax, //esto tiene que ir en Strict en produccion
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            Response.Cookies.Append("Refresh", result.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, //true en produccion
                SameSite = SameSiteMode.Lax, //esto tiene que ir en Strict en produccion
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(
            UserDto request,
            [FromHeader(Name = "lang")] string lang
            )
        {
            var result = await authService.LoginAsync(request);
            if (result == null)
            {
                var message = lang == "en" ? "Invalid username or password." : "Nombre de usuario o contraseña inválidos.";
                return BadRequest(new { message = message });
            }

            Response.Cookies.Append("Authentication", result.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, //true en produccion
                SameSite = SameSiteMode.Lax, //esto tiene que ir en Strict en produccion
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            Response.Cookies.Append("Refresh", result.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, //true en produccion
                SameSite = SameSiteMode.Lax, //esto tiene que ir en Strict en produccion
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Ok();
        }


        [Authorize(Policy = "Refresh")]
        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromHeader(Name = "lang")] string lang)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                var message = lang == "en" ? "Unauthenticated user." : "Usuario no autenticado.";
                return Unauthorized(new { message = message });
            }
            var refreshTokenCookie = Request.Cookies["Refresh"];
            if (string.IsNullOrEmpty(refreshTokenCookie))
            {
                var message = lang == "en" ? "Unauthenticated user." : "Usuario no autenticado.";
                return Unauthorized(new { message = message });
            }

            var newAccessToken = await authService.RefreshAccessTokenAsync(Guid.Parse(userIdClaim.Value), refreshTokenCookie);
            if (newAccessToken == null)
            {
                var message = lang == "en" ? "Unauthenticated user." : "Usuario no autenticado.";
                return Unauthorized(new { message = message });
            }

            Response.Cookies.Append("Authentication", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, //true en produccion
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            return Ok();
        }

        [Authorize(Policy = "Authenticated")]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout([FromHeader(Name = "lang")] string lang)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                var message = lang == "en" ? "Unauthenticated user." : "Usuario no autenticado.";
                return Unauthorized(new { message = message });
            }
            var refreshTokenCookie = Request.Cookies["Refresh"];
            if (string.IsNullOrEmpty(refreshTokenCookie))
            {
                var message = lang == "en" ? "Unauthenticated user." : "Usuario no autenticado.";
                return Unauthorized(new { message = message });
            }
            var verify = await authService.VerifyUser(Guid.Parse(userIdClaim.Value), refreshTokenCookie);
            if (verify == null || verify == false)
            {
                var message = lang == "en" ? "Unauthenticated user." : "Usuario no autenticado.";
                return Unauthorized(new { message = message });
            }
            Response.Cookies.Append("Authentication", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = false, //true en produccion
                SameSite = SameSiteMode.Lax, //esto tiene que ir en Strict en produccion
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });
            Response.Cookies.Append("Refresh", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = false, //true en produccion
                SameSite = SameSiteMode.Lax, //esto tiene que ir en Strict en produccion
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });
            return Ok();
        }
    }
}

