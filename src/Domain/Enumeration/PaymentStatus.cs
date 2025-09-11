namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class PaymentStatus : Enumeration<PaymentStatus, byte>
    {
        [Display]
        public static readonly PaymentStatus Pending = new(nameof(Pending), 0);

        [Display]
        public static readonly PaymentStatus Paid = new(nameof(Paid), 1);

        [Display]
        public static readonly PaymentStatus Failed = new(nameof(Failed), 2);

        public PaymentStatus()
        {
        }

        public PaymentStatus(string name, byte value) : base(name, value)
        {
        }
    }
}
