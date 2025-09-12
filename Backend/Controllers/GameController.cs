using Backend.Entities;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("games")]
    [ApiController]
    public class GameController(IGameService gameService) : ControllerBase
    {
        [Authorize(Policy = "Authenticated")]
        [HttpPost]
        public async Task<ActionResult<GameDto>> Create(
            GameDto request,
            [FromHeader(Name = "lang")] string lang
            )
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                var message = lang == "en" ? "Unauthenticated user." : "Usuario no autenticado.";
                return Unauthorized(new { message = message });
            }

            var game = await gameService.CreateGameAsync(Guid.Parse(userIdClaim.Value), request);
            if (game == null)
            {
                var message = lang == "en" ? "Could not create game." : "No se pudo crear el juego.";   
                return BadRequest(new { message = message });
            }

            return Ok(game);
        }

        [Authorize(Policy = "Authenticated")]
        [HttpGet("bestTimes")]
        public async Task<ActionResult> FindBestTimes(
            [FromQuery] int rows,
            [FromQuery] int cols,
            [FromQuery] int mines,
            [FromHeader(Name = "lang")] string lang
            )
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                var message = lang == "en" ? "Unauthenticated user." : "Usuario no autenticado.";
                return Unauthorized(new { message = message });
            }

            return Ok(await gameService.FindBestTimesAsync(Guid.Parse(userIdClaim.Value), rows, cols, mines));
        }

        [HttpGet("difficulty")]
        public async Task<ActionResult> FindByDifficulty(
            [FromQuery] int rows,
            [FromQuery] int cols,
            [FromQuery] int mines,
            [FromQuery] int page,
            [FromQuery] int take,
            [FromQuery] bool orderByTime
            )
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            return Ok(await gameService.FindByDifficultyAsync(
                userIdClaim is null ? null : Guid.Parse(userIdClaim.Value),
                rows, cols, mines, page, take, orderByTime));
        }

        [Authorize(Policy = "Authenticated")]
        [HttpGet("difficultyUser")]
        public async Task<ActionResult> FindByDifficultyUser(
            [FromQuery] int rows,
            [FromQuery] int cols,
            [FromQuery] int mines,
            [FromQuery] int page,
            [FromQuery] int take,
            [FromQuery] bool orderByTime,
            [FromHeader(Name = "lang")] string lang
            )
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                var message = lang == "en" ? "Unauthenticated user." : "Usuario no autenticado.";
                return Unauthorized(new { message = message });
            }
            return Ok(await gameService.FindByDifficultyUserAsync(
                Guid.Parse(userIdClaim.Value),
                rows, cols, mines, page, take, orderByTime));
        }
    }
}
