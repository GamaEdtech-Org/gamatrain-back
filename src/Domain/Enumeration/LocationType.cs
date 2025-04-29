namespace GamaEdtech.Domain.Enumeration
{
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAnnotation;

    public sealed class LocationType : Enumeration<LocationType, byte>
    {
        [Display]
        public static readonly LocationType Country = new(nameof(Country), 0, null);

        [Display]
        public static readonly LocationType State = new(nameof(State), 1, Country);

        [Display]
        public static readonly LocationType City = new(nameof(City), 2, State);

        public LocationType? ValidParentLocationType { get; }

        public LocationType()
        {
        }

        public LocationType(string name, byte value, LocationType? validParentLocationType)
            : base(name, value) => ValidParentLocationType = validParentLocationType;
    }
}
