namespace GamaEdtech.Common.Converter
{
    public interface IJsonConverter
    {
        bool IgnoreOnExport { get; }

        string? Convert(object value);
    }
}
