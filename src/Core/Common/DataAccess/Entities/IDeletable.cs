namespace GamaEdtech.Common.DataAccess.Entities
{
    using GamaEdtech.Common.DataAnnotation.Schema;

    public interface IDeletable
    {
        [NotMapped]
        bool Deleted { get; set; }
    }
}
