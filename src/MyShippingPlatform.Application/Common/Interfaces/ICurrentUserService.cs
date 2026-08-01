

namespace MyShippingPlatform.Application.Common.Interfaces;

/// <summary>
/// Provides access to the current authenticated user's context.
/// Implemented in the API/Infrastructure layer using HttpContext.
/// </summary>
public interface ICurrentUserService
{
    string? UserId { get; }
    IEnumerable<string> Permissions { get; }
}
