using GustavoTesteMxLog.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace GustavoTesteMxLog.Application.Services;

[ExcludeFromCodeCoverage]
public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    public Guid UserId =>
        Guid.Parse(accessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value!);
}