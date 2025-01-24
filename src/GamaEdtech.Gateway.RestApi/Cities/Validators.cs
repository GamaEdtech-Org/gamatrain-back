using FluentValidation;

namespace GamaEdtech.Gateway.RestApi.Cities;

public class AddCityDtoValidator : AbstractValidator<AddCityDto>
{
	public AddCityDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotNull().WithMessage("name is required")
			.NotEmpty().WithMessage("name is required")
			.MaximumLength(100).WithMessage("name is too long");
	}
}

public class EditCityInfoDtoValidator : AbstractValidator<EditCityInfoDto>
{
	public EditCityInfoDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotNull().WithMessage("name is required")
			.NotEmpty().WithMessage("name is required")
			.MaximumLength(100).WithMessage("name is too long");
	}
}