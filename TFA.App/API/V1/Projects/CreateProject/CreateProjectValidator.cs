using FluentValidation;

namespace TFA.App.API.V1.Projects.CreateProject;

public sealed class CreateProjectValidator : AbstractValidator<CreateProject.CreateProjectRequest>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    }
}
