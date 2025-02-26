namespace GamaEdtech.Common.Identity
{
    using GamaEdtech.Common.DataAnnotation;

    using Microsoft.AspNetCore.Identity;

    [Injectable]
    public interface IApiDataProtectorTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser>
        where TUser : class
    {
    }
}
