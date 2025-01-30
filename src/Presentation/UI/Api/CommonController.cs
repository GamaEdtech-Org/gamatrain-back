//namespace GamaEdtech.Backend.UI.Web.Api
//{
//    using System.Reflection;

//    using Asp.Versioning;

//    using Farsica.Framework.Core;
//    using Farsica.Framework.Data;
//    using Farsica.Framework.DataAnnotation;
//    using GamaEdtech.Backend.Data.ViewModel.Common;
//    using GamaEdtech.Backend.Shared.Service;

//    using Microsoft.AspNetCore.Authorization;
//    using Microsoft.AspNetCore.Mvc;

//    [Route("api/v{version:apiVersion}/[controller]")]
//    [ApiVersion("1.0")]
//    public class CommonController(Lazy<ILogger<CommonController>> logger, Lazy<IApplicationSettingsService> applicationSettingsService) : ApiControllerBase<CommonController>(logger)
//    {
//        [HttpGet("settings"), Produces(typeof(ApiResponse<SettingsViewModel>))]
//        [AllowAnonymous]
//        public async Task<IActionResult> Settings() => Ok(new ApiResponse<SettingsViewModel>
//        {
//            Data = new()
//            {
//                Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
//                Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
//                GridPageSize = (await applicationSettingsService.Value.GetApplicationSettingsAsync()).Data?.GridPageSize ?? 10,
//            }
//        });
//    }
//}
