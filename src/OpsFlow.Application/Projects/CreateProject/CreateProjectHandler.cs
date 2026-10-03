using OpsFlow.Application.Common.Exceptions;
using OpsFlow.Application.Common.Interfaces;
using OpsFlow.Domain.Entities;
using OpsFlow.Domain.Enums;
using FluentValidation;

namespace OpsFlow.Application.Projects.CreateProject;

public sealed class CreateProjectHandler
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateProjectCommand> _validator;
    private readonly ICurrentUser _currentUser;

    public CreateProjectHandler(
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork,
    IValidator<CreateProjectCommand> validator,
    ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _currentUser = currentUser;
    }

    public async Task<CreateProjectResult> HandleAsync(
    CreateProjectCommand command,
    CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            command,
            cancellationToken);

        if (!_currentUser.IsAuthenticated ||
            _currentUser.UserId is null)
        {
            throw new UnauthorizedException(
                "An authenticated user is required to create a project.");
        }

        var currentUserId = _currentUser.UserId.Value;

        var name = command.Name.Trim();
        var key = command.Key.Trim().ToUpperInvariant();

        var description = string.IsNullOrWhiteSpace(command.Description)
            ? null
            : command.Description.Trim();

        var keyExists = await _projectRepository.KeyExistsAsync(
            command.OrganizationId,
            key,
            cancellationToken);

        if (keyExists)
        {
            throw new ConflictException(
                $"A project with key '{key}' already exists in this organization.");
        }

        var project = new Project
        {
            Id = Guid.NewGuid(),
            OrganizationId = command.OrganizationId,
            Name = name,
            Key = key,
            Description = description,
            Status = ProjectStatus.Active,
            CreatedByUserId = currentUserId,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _projectRepository.AddAsync(
            project,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new CreateProjectResult(
            project.Id,
            project.OrganizationId,
            project.Name,
            project.Key,
            project.Description);
    }
}
