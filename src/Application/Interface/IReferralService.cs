namespace GamaEdtech.Application.Interface
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.UserReferral;

    [Injectable]
    public interface IReferralService
    {
        Task<ResultData<string>> CreateReferralUserAsync();
        Task<ResultData<ListDataSource<UserReferralDto>>> GetAllUsersReferralAsync();
    }
}
