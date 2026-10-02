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

    public CreateProjectHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateProjectCommand> validator)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<CreateProjectResult> HandleAsync(
        CreateProjectCommand command,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            command,
            cancellationToken);

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
            CreatedByUserId = command.CreatedByUserId,
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
