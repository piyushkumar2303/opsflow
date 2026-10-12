using FluentValidation;
using OpsFlow.Application.Common.Exceptions;
using OpsFlow.Application.Common.Interfaces;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Users.RegisterUser;

public sealed class RegisterUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RegisterUserCommand> _validator;

    public RegisterUserHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IValidator<RegisterUserCommand> validator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<RegisterUserResult> HandleAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            command,
            cancellationToken);

        var email = command.Email
            .Trim()
            .ToLowerInvariant();

        var firstName = command.FirstName.Trim();
        var lastName = command.LastName.Trim();

        var emailExists = await _userRepository.EmailExistsAsync(
            email,
            cancellationToken);

        if (emailExists)
        {
            throw new ConflictException(
                $"A user with email '{email}' already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            CreatedAtUtc = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            command.Password);

        await _userRepository.AddAsync(
            user,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new RegisterUserResult(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName);
    }
}
