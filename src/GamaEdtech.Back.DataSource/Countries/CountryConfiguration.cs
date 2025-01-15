using GamaEdtech.Back.Domain.Schools;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GamaEdtech.Back.Domain.Countries;
using GamaEdtech.Back.Domain.Base;


namespace GamaEdtech.Back.DataSource.Contries;

internal class CountryConfiguration : IEntityTypeConfiguration<Country>
{
	public void Configure(EntityTypeBuilder<Country> builder)
	{
		builder.ToTable("Country").HasKey(x => x.Id);

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
			.HasMaxLength(3)
			.IsRequired();
	}
}

