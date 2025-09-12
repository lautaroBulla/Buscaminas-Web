using Backend.Entities;
using Backend.Models;

namespace Backend.Services
{
    public interface IGameService
    {
        Task<GameDto?> CreateGameAsync(Guid userId, GameDto request);
        Task<BestTimesDto> FindBestTimesAsync(Guid userId, int rows, int cols, int mines);
        Task<ResponseDifficulty> FindByDifficultyAsync(Guid? userId, int rows, int cols, int mines, int page, int take, bool orderByTime);
        Task<ResponseDifficulty> FindByDifficultyUserAsync(Guid userId, int rows, int cols, int mines, int page, int take, bool orderByTime);
    }
}
