namespace GamaEdtech.Application.Interface
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;

    [Injectable]
    public interface IGlobalService
    {
        Task<ResultData<bool>> VerifyCaptchaAsync(string? captcha);
    }
}
