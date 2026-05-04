using GustavoTesteMxLog.Domain.Entities;

namespace GustavoTesteMxLog.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
