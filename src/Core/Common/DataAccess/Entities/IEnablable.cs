namespace GamaEdtech.Common.DataAccess.Entities
{
    using GamaEdtech.Common.DataAnnotation.Schema;

    public interface IEnablable
    {
        [NotMapped]
        bool Enabled { get; set; }
    }
}
