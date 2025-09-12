using Backend.Entities;
using Backend.Models;

namespace Backend.Services
{
    public interface IAuthService
    {
        Task<GetMeDto?> GetMeAsync(Guid userId);
        Task<TokenResponseDto?> RegisterAsync(UserDto request);
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<string?> RefreshAccessTokenAsync(Guid userId, string refreshTokenCookie);
        Task<Boolean?> VerifyUser(Guid userId, string refreshTokenCookie);
    }
}
