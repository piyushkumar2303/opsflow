namespace OpsFlow.Application.Users.LoginUser;

public sealed record LoginUserResult(
    Guid UserId,
    string Email);
