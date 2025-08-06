namespace GamaEdtech.Application.Interface
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Domain.Entity;

    public interface IReferralService
    {
        Task<ResultData<bool>> CreateRefrralUserAsync(ReferralUser referralUser);
    }
}
