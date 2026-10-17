namespace OpsFlow.Application.Users.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password);
