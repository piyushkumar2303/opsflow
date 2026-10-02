using FluentValidation;

namespace OpsFlow.Application.Projects.CreateProject;

public sealed class CreateProjectCommandValidator
    : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(command => command.OrganizationId)
            .NotEmpty();

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.Key)
            .NotEmpty()
            .MaximumLength(20)
            .Matches("^[A-Za-z][A-Za-z0-9]*$")
            .WithMessage(
                "Project key must start with a letter and contain only letters and numbers.");

        RuleFor(command => command.Description)
            .MaximumLength(2000);

        RuleFor(command => command.CreatedByUserId)
            .NotEmpty();
    }
}
