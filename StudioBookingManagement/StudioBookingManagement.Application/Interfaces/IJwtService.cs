using System.Security.Claims;
using StudioBookingManagement.Domain.Entities;

namespace StudioBookingManagement.Application.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    bool ValidateToken(string token);
} 