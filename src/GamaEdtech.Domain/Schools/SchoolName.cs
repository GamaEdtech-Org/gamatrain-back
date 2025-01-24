using CSharpFunctionalExtensions;

namespace GamaEdtech.Domain.Schools;

public class SchoolName : ValueObject
{
	public string InEnglish { get; }
	public string InLocalLanguage { get; }

	public SchoolName(string inEnglish, string inLocalLanguage)
	{
		InEnglish = inEnglish;
		InLocalLanguage = inLocalLanguage;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return InEnglish;
		yield return InLocalLanguage;
	}
}
