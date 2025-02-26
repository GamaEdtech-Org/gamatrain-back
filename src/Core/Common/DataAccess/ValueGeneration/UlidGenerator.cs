namespace GamaEdtech.Common.DataAccess.ValueGeneration
{
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.ValueGeneration;

    using NUlid;

    public class UlidGenerator : ValueGenerator<Ulid>
    {
        public override bool GeneratesTemporaryValues => false;

        public override Ulid Next(EntityEntry entry) => Ulid.NewUlid();
    }
}
