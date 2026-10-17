using OpsFlow.Domain.Enums;

namespace OpsFlow.Application.Organizations.ListMyOrganizations;

public sealed record MyOrganizationItem(
    Guid Id,
    string Name,
    OrganizationRole Role);
