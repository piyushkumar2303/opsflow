using Microsoft.AspNetCore.Mvc;
using OpsFlow.Api.Contracts.Projects;
using OpsFlow.Application.Projects.CreateProject;
using Microsoft.AspNetCore.Authorization;

namespace OpsFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/projects")]
public sealed class ProjectsController : ControllerBase
{
    private readonly CreateProjectHandler _createProjectHandler;

    public ProjectsController(
        CreateProjectHandler createProjectHandler)
    {
        _createProjectHandler = createProjectHandler;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateProjectResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
    CreateProjectRequest request,
    CancellationToken cancellationToken)
    {
        var command = new CreateProjectCommand(
            request.OrganizationId,
            request.Name,
            request.Key,
            request.Description);

        var result = await _createProjectHandler.HandleAsync(command, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, result);
    }
}
