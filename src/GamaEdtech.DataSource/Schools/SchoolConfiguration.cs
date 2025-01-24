using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GamaEdtech.Domain.Schools;
using GamaEdtech.Domain.Base;
using GamaEdtech.Domain.States;
using GamaEdtech.Domain.Cities;

namespace GamaEdtech.DataSource.Schools;

internal class SchoolConfiguration : IEntityTypeConfiguration<School>
{
	public void Configure(EntityTypeBuilder<School> builder)
	{
		builder.ToTable("School").HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.HasConversion(
				id => id.Value,
				value => new Id(value)
			)
			.HasColumnName("Id")
			.ValueGeneratedOnAdd();

		builder.OwnsOne(x => x.Name, b =>
		{
			b.Property(name => name.InEnglish)
				.HasColumnName("NameInEnglish")
				.HasMaxLength(100)
				.IsRequired();

			b.Property(name => name.InLocalLanguage)
				.HasColumnName("NameInLocalLanguage")
				.HasMaxLength(100)
				.IsRequired();
		});

		builder.OwnsOne(x => x.Address, b =>
		{
			b.Property(address => address.Description)
				.HasColumnName("AddressDescription")
				.HasMaxLength(500)
				.IsRequired();

			b.OwnsOne(address => address.Location, b =>
			{
				b.Property(l => l.Geography)
					.HasColumnType("GEOGRAPHY")
					.HasColumnName("AddressGeography")
					.IsRequired(); // Ensures the column is stored as a spatial type
			});

			b.Property(address => address.ZipCode)
				.HasColumnName("AddressZipCode")
				.HasMaxLength(20)
				.IsRequired();

			b.Property(address => address.StateId)
				.HasColumnName("AddressStateId");

			b.HasOne<State>()
				.WithMany()
				.HasForeignKey(x => x.StateId)
				.IsRequired(false);

			b.Property(address => address.CityId)
				.HasColumnName("AddressCityId");

			b.HasOne<City>()
				.WithMany()
				.HasForeignKey(x => x.CityId)
				.IsRequired();
		});



	}
}
