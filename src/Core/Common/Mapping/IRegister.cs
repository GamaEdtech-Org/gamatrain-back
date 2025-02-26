namespace GamaEdtech.Common.Mapping
{
    using System.Diagnostics.CodeAnalysis;

    public interface IRegister
    {
        void Register([NotNull] TypeAdapterConfig config);
    }
}
