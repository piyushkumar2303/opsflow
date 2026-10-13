using FluentValidation;
using OpsFlow.Application.Common.Exceptions;
using OpsFlow.Application.Common.Interfaces;

namespace OpsFlow.Application.Users.LoginUser;

public sealed class LoginUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<LoginUserCommand> _validator;

    public LoginUserHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IValidator<LoginUserCommand> validator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _validator = validator;
    }

    public async Task<LoginUserResult> HandleAsync(
        LoginUserCommand command,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            command,
            cancellationToken);

        var email = command.Email
            .Trim()
            .ToLowerInvariant();

        var user = await _userRepository.GetByEmailAsync(
            email,
            cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedException(
                "Invalid email or password.");
        }

        var passwordIsValid = _passwordHasher.VerifyPassword(
            user,
            user.PasswordHash,
            command.Password);

        if (!passwordIsValid)
        {
            throw new UnauthorizedException(
                "Invalid email or password.");
        }

        return new LoginUserResult(
            user.Id,
            user.Email);
    }
}
