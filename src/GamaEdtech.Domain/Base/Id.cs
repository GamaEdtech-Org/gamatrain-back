using CSharpFunctionalExtensions;

namespace GamaEdtech.Domain.Base;

public class Id : ValueObject, IComparable<Id>
{
	public int Value { get; }

	public Id(int value)
	{
		//if(value < 0)
		//	throw new ArgumentOutOfRangeException();

		Value = value;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Value;
	}

	public int CompareTo(Id? other)
	{
		if (other is null) return 1;
		if(Value > other.Value) return 1;
		if(Value < other.Value) return -1;
		return 0;
	}
}
