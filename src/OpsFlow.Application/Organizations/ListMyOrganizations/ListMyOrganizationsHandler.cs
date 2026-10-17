using OpsFlow.Application.Common.Exceptions;
using OpsFlow.Application.Common.Interfaces;

namespace OpsFlow.Application.Organizations.ListMyOrganizations;

public sealed class ListMyOrganizationsHandler
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly ICurrentUser _currentUser;

    public ListMyOrganizationsHandler(
        IOrganizationRepository organizationRepository,
        ICurrentUser currentUser)
    {
        _organizationRepository = organizationRepository;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<MyOrganizationItem>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
        {
            throw new UnauthorizedException(
                "An authenticated user is required to view organizations.");
        }

        var currentUserId = _currentUser.UserId.Value;

        return await _organizationRepository.ListOrganizationsForUserAsync(
            currentUserId, cancellationToken);
    }
}
