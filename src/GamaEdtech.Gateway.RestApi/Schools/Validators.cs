using FluentValidation;
using GamaEdtech.Gateway.RestApi.Controllers;

namespace GamaEdtech.Gateway.RestApi.Schools;

public class RegisterSchoolDtoValidator : AbstractValidator<RegisterSchoolDto>
{
	public RegisterSchoolDtoValidator()
	{
		RuleFor(x => x.Name.InEnglish)
			.NotNull().WithMessage("name.inEnglish is required")
			.NotEmpty().WithMessage("name.inEnglish is required")
			.MaximumLength(100).WithMessage("name.inEnglish is too long");

		RuleFor(x => x.Name.InLocalLanguage)
			.NotNull().WithMessage("name.inLocalLanguage is required")
			.MaximumLength(100).WithMessage("name.inLocalLanguage is too long");

		RuleFor(x => x.Type)
			.Must(type => type == 0 || type == 1);

		RuleFor(x => x.Address.Description)
			.Cascade(CascadeMode.Stop).NotNull().WithMessage("address.description is required")
			.NotEmpty().WithMessage("address.description is required")
			.MaximumLength(500).WithMessage("address.description is too long");

		RuleFor(x => x.Address.ZipCode)
			.Cascade(CascadeMode.Stop).NotNull().WithMessage("address.zipCode is required")
			.NotEmpty().WithMessage("address.zipCode is required")
			.MaximumLength(10).WithMessage("address.zipCode is too long");

		RuleFor(x => x.Address.Location.Latitude)
			.GreaterThanOrEqualTo(-90).WithMessage("address.location.latitude is out of range")
			.LessThanOrEqualTo(90).WithMessage("address.location.latitude is out of range");

		RuleFor(x => x.Address.Location.Longitude)
			.GreaterThanOrEqualTo(-180).WithMessage("address.location.longitude is out of range")
			.LessThanOrEqualTo(180).WithMessage("address.location.longitude is out of range");
	}
}


public class SearchByLocationDtoValidator : AbstractValidator<SearchByLocationDto>
{
	public SearchByLocationDtoValidator()
	{
		RuleFor(x => x.Latitude)
			.GreaterThanOrEqualTo(-90).WithMessage("latitude is out of range")
			.LessThanOrEqualTo(90).WithMessage("latitude is out of range");

		RuleFor(x => x.Longitude)
			.GreaterThanOrEqualTo(-180).WithMessage("longitude is out of range")
			.LessThanOrEqualTo(180).WithMessage("longitude is out of range");
	}
}
