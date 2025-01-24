using FluentValidation;

namespace GamaEdtech.Gateway.RestApi.Countries;

public class AddCountryDtoValidator : AbstractValidator<AddCountryDto>
{
	public AddCountryDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotNull().WithMessage("name is required")
			.NotEmpty().WithMessage("name is required")
			.MaximumLength(50).WithMessage("name is too long");

		RuleFor(x => x.Code)
			.NotNull().WithMessage("code is required")
			.NotEmpty().WithMessage("code is required")
			.Matches("^[A-Z]{2,3}$").WithMessage("code is invalid");
	}
}

public class EditCountryInfoDtoValidator : AbstractValidator<EditCountryInfoDto>
{
	public EditCountryInfoDtoValidator()
	{
		RuleFor(x => x.Name)
			.NotNull().WithMessage("name is required")
			.NotEmpty().WithMessage("name is required")
			.MaximumLength(50).WithMessage("name is too long");

		RuleFor(x => x.Code)
			.NotNull().WithMessage("code is required")
			.NotEmpty().WithMessage("code is required")
			.Matches("^[A-Z]{2,3}$").WithMessage("code is invalid");
	}
}

