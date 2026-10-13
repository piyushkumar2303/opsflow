using Microsoft.AspNetCore.Mvc;
using OpsFlow.Api.Contracts.Authentication;
using OpsFlow.Application.Users.RegisterUser;

namespace OpsFlow.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly RegisterUserHandler _registerUserHandler;

    public AuthController(
        RegisterUserHandler registerUserHandler)
    {
        _registerUserHandler = registerUserHandler;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterUserResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        var result = await _registerUserHandler.HandleAsync(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            result);
    }
}
