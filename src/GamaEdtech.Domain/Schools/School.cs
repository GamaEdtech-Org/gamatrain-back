using CSharpFunctionalExtensions;
using GamaEdtech.Domain.Base;

namespace GamaEdtech.Domain.Schools;

public class School : AggregateRoot
{
	public SchoolName Name { get; private set; }
	public SchoolType Type { get; private set; }
	public Address Address { get; private set; }

	protected School() { }

	public School(SchoolName name, SchoolType type, Address address)
	{
		Name = name;
		Type = type;
		Address = address;
	}
}


public enum SchoolType
{
	Public,
	Private
}