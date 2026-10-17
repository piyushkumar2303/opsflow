using Microsoft.AspNetCore.Mvc;
using OpsFlow.Api.Contracts.Projects;
using OpsFlow.Application.Projects.CreateProject;
using Microsoft.AspNetCore.Authorization;
using OpsFlow.Application.Projects.GetProjectById;
using OpsFlow.Application.Common.Models;
using OpsFlow.Application.Projects.ListProjects;
using OpsFlow.Domain.Enums;

namespace OpsFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/projects")]
public sealed class ProjectsController : ControllerBase
{
    private readonly CreateProjectHandler _createProjectHandler;
    private readonly GetProjectByIdHandler _getProjectByIdHandler;
    private readonly ListProjectsHandler _listProjectsHandler;

    public ProjectsController(
        CreateProjectHandler createProjectHandler,
        GetProjectByIdHandler getProjectByIdHandler,
        ListProjectsHandler listProjectsHandler)
    {
        _createProjectHandler = createProjectHandler;
        _getProjectByIdHandler = getProjectByIdHandler;
        _listProjectsHandler = listProjectsHandler;
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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetProjectByIdResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetProjectByIdQuery(id);

        var result = await _getProjectByIdHandler.HandleAsync(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ProjectListItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> List(
        [FromQuery] Guid organizationId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] ProjectStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = new ListProjectsQuery(
            organizationId,
            pageNumber,
            pageSize,
            searchTerm,
            status);

        var result = await _listProjectsHandler.HandleAsync(query, cancellationToken);

        return Ok(result);
    }
}
