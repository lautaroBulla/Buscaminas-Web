using Backend.Data;
using Backend.Entities;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class GameService(AppDbContext context) : IGameService
    {
        public async Task<GameDto?> CreateGameAsync(Guid userId, GameDto request)
        {
            var game = new Game
            {
                UserId = userId,
                Help = request.Help,
                Seconds = request.Seconds,
                Difficulty = request.Difficulty,
                Rows = request.Rows,
                Cols = request.Cols,
                Mines = request.Mines,
                N3BV = request.N3BV,
                Clicks = request.Clicks,
                Efficiency = request.Efficiency
            };
            context.Games.Add(game);
            await context.SaveChangesAsync();
            return request;
        }

        public async Task<BestTimesDto> FindBestTimesAsync(Guid userId, int rows, int cols, int mines)
        {
            var userBestTimeValue = await context.Games
                .Where(g => g.UserId == userId && g.Rows == rows && g.Cols == cols && g.Mines == mines)
                .OrderBy(g => g.Seconds)
                .Select(g => g.Seconds)
                .FirstOrDefaultAsync();

            var globalBestTimeValue = await context.Games
                .Where(g => g.Rows == rows && g.Cols == cols && g.Mines == mines)
                .OrderBy(g => g.Seconds)
                .Select(g => g.Seconds)
                .FirstOrDefaultAsync();

            return new BestTimesDto
            {
                userBestTime = userBestTimeValue == 0 ? null : userBestTimeValue,
                globalBestTime = globalBestTimeValue == 0 ? null : globalBestTimeValue
            };
        }

        public async Task<ResponseDifficulty> FindByDifficultyAsync(Guid? userId, int rows, int cols, int mines, int page, int take, bool orderByTime)
        {
            var skip = (page - 1) * take;

            var query = context.Games
            .Include(g => g.User)
            .Where(g =>
                g.Rows == rows &&
                g.Cols == cols &&
                g.Mines == mines);

            if (orderByTime)
            {
                query = query
                    .OrderBy(g => g.Seconds)
                    .ThenBy(g => g.Help)
                    .ThenByDescending(g => g.Efficiency);
            }
            else
            {
                query = query
                    .OrderByDescending(g => g.Efficiency)
                    .ThenByDescending(g => g.N3BV)
                    .ThenBy(g => g.Seconds)
                    .ThenBy(g => g.Help);
            }

            var games = await query
                .Skip(skip)
                .Take(take)
                .Select(g => new
                {
                    g.Id,
                    g.Help,
                    g.Seconds,
                    g.CreatedAt,
                    g.Clicks,
                    g.N3BV,
                    g.Efficiency,
                    User = new { g.User.Username }
                })
                .ToListAsync();

            var total = await query.CountAsync();

            var totalPages = (int)Math.Ceiling((double)total / take);
            var myPosition = await findMyPosition(userId, rows, cols, mines);

            return new ResponseDifficulty
            {
                rows = rows,
                cols = cols,
                mines = mines,
                totalPages = totalPages,
                page = page,
                myPosition = myPosition,
                games = games.Select(g => new GamesInfoDto
                {
                    id = g.Id,
                    help = g.Help,
                    seconds = g.Seconds,
                    createdAt = g.CreatedAt,
                    clicks = g.Clicks,
                    n3BV = g.N3BV,
                    efficiency = g.Efficiency,
                    user = new UserInfoDto
                    {
                        username = g.User.Username
                    },
                }).ToList()
            };
        }

        public async Task<ResponseDifficulty> FindByDifficultyUserAsync(Guid userId, int rows, int cols, int mines, int page, int take, bool orderByTime)
        {
            var skip = (page - 1) * take;

            var query = context.Games
            .Include(g => g.User)
            .Where(g =>
                g.Rows == rows &&
                g.Cols == cols &&
                g.Mines == mines &&
                g.UserId == userId);

            if (orderByTime)
            {
                query = query
                    .OrderBy(g => g.Seconds)
                    .ThenBy(g => g.Help)
                    .ThenByDescending(g => g.Efficiency);
            }
            else
            {
                query = query
                    .OrderByDescending(g => g.Efficiency)
                    .ThenByDescending(g => g.N3BV)
                    .ThenBy(g => g.Seconds)
                    .ThenBy(g => g.Help);
            }

            var games = await query
                .Skip(skip)
                .Take(take)
                .Select(g => new
                {
                    g.Id,
                    g.Help,
                    g.Seconds,
                    g.CreatedAt,
                    g.Clicks,
                    g.N3BV,
                    g.Efficiency,
                    User = new { g.User.Username }
                })
                .ToListAsync();

            var total = await query.CountAsync();

            var totalPages = (int)Math.Ceiling((double)total / take);
            var myPosition = await findMyPosition(userId, rows, cols, mines);

            return new ResponseDifficulty
            {
                rows = rows,
                cols = cols,
                mines = mines,
                totalPages = totalPages,
                page = page,
                myPosition = myPosition,
                games = games.Select(g => new GamesInfoDto
                {
                    id = g.Id,
                    help = g.Help,
                    seconds = g.Seconds,
                    createdAt = g.CreatedAt,
                    clicks = g.Clicks,
                    n3BV = g.N3BV,
                    efficiency = g.Efficiency,
                    user = new UserInfoDto
                    {
                        username = g.User.Username
                    },
                }).ToList()
            };
        }

        private async Task<MyPositionDto?> findMyPosition(Guid? userId, int rows, int cols, int mines)
        {
            if (userId == null)
            {
                var total = await context.Games
                    .Where(g => g.Rows == rows && g.Cols == cols && g.Mines == mines)
                    .CountAsync();

                return new MyPositionDto { position = null, total = total };
            }

            var userGame = await context.Games
                .Where(g => g.UserId == userId && g.Rows == rows && g.Cols == cols && g.Mines == mines)
                .OrderBy(g => g.Seconds)
                .ThenBy(g => g.Help)
                .ThenByDescending(g => g.Efficiency)
                .FirstOrDefaultAsync();

            if (userGame == null)
            {
                return new MyPositionDto
                {
                    position = null,
                    total = 0
                };
            }

            var betterGamesCount = await context.Games
                .Where(g =>
                    g.Rows == rows &&
                    g.Cols == cols &&
                    g.Mines == mines &&
                    (g.Seconds < userGame.Seconds ||
                     (g.Seconds == userGame.Seconds && g.Help < userGame.Help) ||
                     (g.Seconds == userGame.Seconds && g.Help == userGame.Help && g.Efficiency > userGame.Efficiency)))
                .CountAsync();

            var totalGames = await context.Games
                .Where(g => g.Rows == rows && g.Cols == cols && g.Mines == mines)
                .CountAsync();

            return new MyPositionDto
            {
                position = betterGamesCount + 1,
                total = totalGames
            };
        }
    }
}
