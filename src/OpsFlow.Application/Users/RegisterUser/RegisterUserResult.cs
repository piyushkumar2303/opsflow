namespace OpsFlow.Application.Users.RegisterUser;

public sealed record RegisterUserResult(
    Guid Id,
    string Email,
    string FirstName,
    string LastName);
