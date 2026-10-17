using FluentValidation;
using OpsFlow.Application.Common.Exceptions;
using OpsFlow.Application.Common.Interfaces;
using OpsFlow.Domain.Entities;
using OpsFlow.Domain.Enums;

namespace OpsFlow.Application.Organizations.CreateOrganization;

public sealed class CreateOrganizationHandler
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateOrganizationCommand> _validator;

    public CreateOrganizationHandler(
        IOrganizationRepository organizationRepository,
        IOrganizationMemberRepository organizationMemberRepository,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork,
        IValidator<CreateOrganizationCommand> validator)
    {
        _organizationRepository = organizationRepository;
        _organizationMemberRepository = organizationMemberRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<CreateOrganizationResult> HandleAsync(
        CreateOrganizationCommand command,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(
            command,
            cancellationToken);

        if (!_currentUser.IsAuthenticated ||
            _currentUser.UserId is null)
        {
            throw new UnauthorizedException(
                "An authenticated user is required to create an organization.");
        }

        var userId = _currentUser.UserId.Value;
        var now = DateTime.UtcNow;

        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            Name = command.Name.Trim(),
            CreatedAtUtc = now
        };

        var organizationMember = new OrganizationMember
        {
            Id = Guid.NewGuid(),
            OrganizationId = organization.Id,
            UserId = userId,
            Role = OrganizationRole.Admin,
            JoinedAtUtc = now
        };

        await _organizationRepository.AddAsync(
            organization,
            cancellationToken);

        await _organizationMemberRepository.AddAsync(
            organizationMember,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new CreateOrganizationResult(
            organization.Id,
            organization.Name);
    }
}
