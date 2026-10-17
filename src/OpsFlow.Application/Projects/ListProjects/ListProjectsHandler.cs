using FluentValidation;
using OpsFlow.Application.Common.Exceptions;
using OpsFlow.Application.Common.Interfaces;
using OpsFlow.Application.Common.Models;
using OpsFlow.Application.Organizations;

namespace OpsFlow.Application.Projects.ListProjects;

public sealed class ListProjectsHandler
{
    private readonly IProjectRepository _projectRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IValidator<ListProjectsQuery> _validator;

    public ListProjectsHandler(
        IProjectRepository projectRepository,
        IOrganizationRepository organizationRepository,
        IOrganizationMemberRepository organizationMemberRepository,
        ICurrentUser currentUser,
        IValidator<ListProjectsQuery> validator)
    {
        _projectRepository = projectRepository;
        _organizationRepository = organizationRepository;
        _organizationMemberRepository = organizationMemberRepository;
        _currentUser = currentUser;
        _validator = validator;
    }

    public async Task<PagedResult<ProjectListItem>> HandleAsync(
        ListProjectsQuery query,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(query, cancellationToken);

        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
        {
            throw new UnauthorizedException(
                "An authenticated user is required to view projects.");
        }

        var currentUserId = _currentUser.UserId.Value;

        var organizationExists = await _organizationRepository.ExistsAsync(
            query.OrganizationId, cancellationToken);

        if (!organizationExists)
        {
            throw new NotFoundException("Organization was not found.");
        }

        var membership = await _organizationMemberRepository.GetByOrganizationAndUserAsync(
            query.OrganizationId, currentUserId, cancellationToken);

        if (membership is null)
        {
            throw new ForbiddenException(
                "You do not have access to this organization.");
        }

        return await _projectRepository.ListAsync(query, cancellationToken);
    }
}
