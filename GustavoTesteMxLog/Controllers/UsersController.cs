using Azure.Core;
using GustavoTesteMxLog.Application.UseCases.Users.CreateUser;
using GustavoTesteMxLog.Application.UseCases.Users.DeleteUser;
using GustavoTesteMxLog.Application.UseCases.Users.GetDashboard;
using GustavoTesteMxLog.Application.UseCases.Users.GetUser;
using GustavoTesteMxLog.Application.UseCases.Users.UpdateUser;
using GustavoTesteMxLog.Domain.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GustavoTesteMxLog.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
    {
        try
        {
            var command = new CreateUserCommand
                (
                    Name: request.Name,
                    Email: request.Email,
                    Password: request.Password
                );

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UserRequest request)
    {
        try
        {
            var command = new UpdateUserCommand
                (
                    Id: id,
                    Name: request.Name,
                    Email: request.Email,
                    Password: request.Password
                );

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        try
        {
            var command = new DeleteUserCommand
                (
                    Id: id
                );

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        try
        {
            var query = new GetUserQuery
                (
                    Id: id
                );

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard([FromQuery] DashboardRequest request)
    {
        try
        {
            var query = new GetDashboardQuery
                (
                    Search: request.Search,
                    Page: request.Page,
                    PageSize: request.PageSize
                );

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}