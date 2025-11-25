using Backend.Entities;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController(IAuthService authService) : ControllerBase
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
    }
}
