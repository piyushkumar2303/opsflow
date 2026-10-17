using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpsFlow.Api.Contracts.Organizations;
using OpsFlow.Application.Organizations.CreateOrganization;
using OpsFlow.Application.Organizations.ListMyOrganizations;

namespace OpsFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/organizations")]
public sealed class OrganizationsController : ControllerBase
{
    private readonly CreateOrganizationHandler _createOrganizationHandler;
    private readonly ListMyOrganizationsHandler _listMyOrganizationsHandler;

    public OrganizationsController(
        CreateOrganizationHandler createOrganizationHandler,
        ListMyOrganizationsHandler listMyOrganizationsHandler)
    {
        _createOrganizationHandler = createOrganizationHandler;
        _listMyOrganizationsHandler = listMyOrganizationsHandler;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateOrganizationResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create(
        CreateOrganizationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateOrganizationCommand(
            request.Name);

        var result = await _createOrganizationHandler.HandleAsync(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            result);
    }

    [HttpGet("mine")]
    [ProducesResponseType(typeof(IReadOnlyList<MyOrganizationItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var result = await _listMyOrganizationsHandler.HandleAsync(cancellationToken);

        return Ok(result);
    }
}
