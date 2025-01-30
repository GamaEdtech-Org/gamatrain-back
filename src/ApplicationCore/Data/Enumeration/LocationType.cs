namespace GamaEdtech.Backend.Data.Enumeration
{
    using Farsica.Framework.Data.Enumeration;
    using Farsica.Framework.DataAnnotation;

    public sealed class LocationType : Enumeration<byte>
    {
        [Display(Name = "Country")]
        public static readonly LocationType Country = new(nameof(Country), 0, null);

        [Display(Name = "State")]
        public static readonly LocationType State = new(nameof(State), 1, Country);

        [Display(Name = "City")]
        public static readonly LocationType City = new(nameof(City), 2, State);

        public LocationType? ValidParentLocationType { get; }

        public LocationType()
        {
        }

        public LocationType(string name, byte value, LocationType? validParentLocationType)
            : base(name, value) => ValidParentLocationType = validParentLocationType;
    }
}
