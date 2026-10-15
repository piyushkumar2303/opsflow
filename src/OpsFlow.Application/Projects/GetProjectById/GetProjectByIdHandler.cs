using FluentValidation;
using OpsFlow.Application.Common.Exceptions;
using OpsFlow.Application.Common.Interfaces;
using OpsFlow.Application.Organizations;

namespace OpsFlow.Application.Projects.GetProjectById;

public sealed class GetProjectByIdHandler
{
    private readonly IProjectRepository _projectRepository;
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IValidator<GetProjectByIdQuery> _validator;

    public GetProjectByIdHandler(
        IProjectRepository projectRepository,
        IOrganizationMemberRepository organizationMemberRepository,
        ICurrentUser currentUser,
        IValidator<GetProjectByIdQuery> validator)
    {
        _projectRepository = projectRepository;
        _organizationMemberRepository = organizationMemberRepository;
        _currentUser = currentUser;
        _validator = validator;
    }

    public async Task<GetProjectByIdResult> HandleAsync(
        GetProjectByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(query, cancellationToken);

        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
        {
            throw new UnauthorizedException(
                "An authenticated user is required to view a project.");
        }

        var project = await _projectRepository.GetByIdAsync(
            query.ProjectId,
            cancellationToken);

        if (project is null)
        {
            throw new NotFoundException(
                "Project was not found.");
        }

        var membership = await _organizationMemberRepository.GetByOrganizationAndUserAsync(
            project.OrganizationId,
            _currentUser.UserId.Value,
            cancellationToken);

        if (membership is null)
        {
            throw new ForbiddenException(
                "You do not have access to this project.");
        }

        return new GetProjectByIdResult(
            project.Id,
            project.OrganizationId,
            project.Name,
            project.Key,
            project.Description,
            project.Status,
            project.CreatedByUserId,
            project.CreatedAtUtc,
            project.UpdatedAtUtc);
    }
}
