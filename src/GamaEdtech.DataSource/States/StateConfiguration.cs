using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GamaEdtech.Domain.States;
using GamaEdtech.Domain.Countries;
using GamaEdtech.Domain.Base;

namespace GamaEdtech.DataSource.Schools;

internal class StateConfiguration : IEntityTypeConfiguration<State>
{
	public void Configure(EntityTypeBuilder<State> builder)
	{
		builder.ToTable("State").HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.HasConversion(
				id => id.Value,
				value => new Id(value)
			)
			.HasColumnName("Id")
			.ValueGeneratedOnAdd();

		builder.Property(x => x.Name)
			.HasColumnName("Name")
			.HasMaxLength(50)
			.IsRequired();

		builder.Property(x => x.Code)
			.HasColumnName("Code")
			.HasMaxLength(5)
			.IsRequired();

		builder.HasOne<Country>()
			.WithMany()
			.HasForeignKey(x => x.CountryId);

	}
}
