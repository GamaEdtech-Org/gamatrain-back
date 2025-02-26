namespace GamaEdtech.Common.Data
{
    public class ParameterDto<T>
    {
        public required T Id { get; set; }

        public string? Name { get; set; }

        public bool Selected { get; set; }

        public bool Disabled { get; set; }

        public string? Value { get; set; }

        public string? Group { get; set; }
    }
}
