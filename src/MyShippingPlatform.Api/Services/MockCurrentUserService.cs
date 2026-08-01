using MyShippingPlatform.Application.Common.Interfaces;

namespace MyShippingPlatform.Api.Services;

/// <summary>
/// Mock user service for local testing and pipeline validation.
/// </summary>
public class MockCurrentUserService : ICurrentUserService
{
    public string? UserId => "admin-user-001";

    // Simulating an RBAC setup where this user has the approve permission
    public IEnumerable<string> Permissions => new[] { "shipment:read", "shipment:approve" };
}
