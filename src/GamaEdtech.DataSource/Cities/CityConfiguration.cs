using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GamaEdtech.Domain.Base;
using GamaEdtech.Domain.Cities;
using GamaEdtech.Domain.States;
using GamaEdtech.Domain.Countries;

namespace GamaEdtech.DataSource.Contries;

internal class CityConfiguration : IEntityTypeConfiguration<City>
{
	public void Configure(EntityTypeBuilder<City> builder)
	{
		builder.ToTable("City").HasKey(x => x.Id);

		builder.Property(x => x.Id)
		.HasConversion(
			id => id.Value,   
			value => new Id(value)
		)
		.HasColumnName("Id")
		.ValueGeneratedOnAdd();

		builder.Property(x => x.Name)
			.HasColumnName("Name")
			.HasMaxLength(100)
			.IsRequired();

		builder.HasOne<State>()
			.WithMany()
			.HasForeignKey(x => x.StateId)
			.IsRequired(false);

		builder.HasOne<Country>()
			.WithMany()
			.HasForeignKey(x => x.CountryId)
			.IsRequired();
	}
}

