using Microsoft.AspNetCore.Mvc;
using OpsFlow.Api.Contracts.Projects;
using OpsFlow.Application.Projects.CreateProject;

namespace OpsFlow.Api.Controllers;

[ApiController]
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
    [ProducesResponseType<CreateProjectResult>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreateProjectResult>> CreateAsync(
        CreateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProjectCommand(
            request.OrganizationId,
            request.Name,
            request.Key,
            request.Description);

        var result = await _createProjectHandler.HandleAsync(
            command,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            result);
    }
}
