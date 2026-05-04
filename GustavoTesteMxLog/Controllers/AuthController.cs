using GustavoTesteMxLog.Application.UseCases.Auth.Login;
using GustavoTesteMxLog.Domain.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GustavoTesteMxLog.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var query = new LoginQuery
            (
                Email: request.Email,
                Password: request.Password
            );

        var token = await _mediator.Send(query);

        if (string.IsNullOrWhiteSpace(token))
            return Unauthorized("Usuário não autorizado.");

        return Ok(new { token });
    }
}
