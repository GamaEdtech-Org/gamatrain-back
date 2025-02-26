namespace GamaEdtech.Common.DataAccess.Entities
{
    using System;

    using GamaEdtech.Common.DataAnnotation.Schema;

    public interface IIdentifiable<TKey>
        where TKey : IEquatable<TKey>
    {
        [NotMapped]
        TKey Id { get; set; }
    }
}
