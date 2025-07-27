using StudioBookingManagement.Application.DTOs;
using StudioBookingManagement.Application.DTOs.Auth;

namespace StudioBookingManagement.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<AuthResponseDto?> RefreshTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken = default);
    Task<bool> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<UserDto?> GetUserProfileAsync(int userId, CancellationToken cancellationToken = default);
} 