namespace GamaEdtech.Presentation.Api.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Presentation.ViewModel.ApplicationSettings;

    using Microsoft.AspNetCore.Mvc;

    [Common.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    [Permission(Roles = [nameof(Role.Admin)])]
    [Display(Name = "Application Settings")]
    public class ApplicationSettingsController(Lazy<ILogger<ApplicationSettingsController>> logger, Lazy<IApplicationSettingsService> applicationSettingsService)
        : ApiControllerBase<ApplicationSettingsController>(logger)
    {
        [HttpGet, Produces(typeof(ApiResponse<ApplicationSettingsViewModel>))]
        [Display(Name = "Application Settings List")]
        public async Task<IActionResult<ApplicationSettingsViewModel>> GetSettings()
        {
            try
            {
                var result = await applicationSettingsService.Value.GetApplicationSettingsAsync();
                return Ok<ApplicationSettingsViewModel>(new(result.Errors)
                {
                    Data = result.Data is null ? null : new()
                    {
                        GridPageSize = result.Data.GridPageSize,
                        DefaultTimeZoneId = result.Data.DefaultTimeZoneId,
                        SchoolContributionPoints = result.Data.SchoolContributionPoints,
                        SchoolImageContributionPoints = result.Data.SchoolImageContributionPoints,
                        SchoolCommentContributionPoints = result.Data.SchoolCommentContributionPoints,
                        PostContributionPoints = result.Data.PostContributionPoints,
                        SchoolIssuesContributionPoints = result.Data.SchoolIssuesContributionPoints,
                        RemoveSchoolImageContributionPoints = result.Data.RemoveSchoolImageContributionPoints,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ApplicationSettingsViewModel> { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }

        [HttpPut, Produces(typeof(ApiResponse<bool>))]
        [Display(Name = "Edit Application Settings")]
        public async Task<IActionResult<bool>> UpdateSettings([NotNull] ApplicationSettingsViewModel request)
        {
            try
            {
                var result = await applicationSettingsService.Value.ModifyApplicationSettingsAsync(new()
                {
                    GridPageSize = request.GridPageSize,
                    DefaultTimeZoneId = request.DefaultTimeZoneId,
                    SchoolContributionPoints = request.SchoolContributionPoints,
                    SchoolImageContributionPoints = request.SchoolImageContributionPoints,
                    SchoolCommentContributionPoints = request.SchoolCommentContributionPoints,
                    PostContributionPoints = request.PostContributionPoints,
                    SchoolIssuesContributionPoints = request.SchoolIssuesContributionPoints,
                    RemoveSchoolImageContributionPoints = request.RemoveSchoolImageContributionPoints,
                });
                return Ok<bool>(new(result.Errors) { Data = result.Data });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<bool>(new(new Error { Message = exc.Message }));
            }
        }
    }
}
