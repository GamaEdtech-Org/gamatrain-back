namespace GamaEdtech.Application.Interface
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;

    [Injectable]
    public interface IReferralService
    {
        Task<ResultData<string>> CreateRefrralUserAsync();
    }
}
