namespace GamaEdtech.Infrastructure.Provider.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.HttpProvider;
    using GamaEdtech.Common.Infrastructure;
    using GamaEdtech.Data.Dto.Game;
    using GamaEdtech.Data.Dto.Identity;
    using GamaEdtech.Data.Dto.Provider.Core;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using SkiaSharp.QrCode.Image;

    using static GamaEdtech.Common.Core.Constants;

    public sealed class CoreProvider(Lazy<IConfiguration> configuration, Lazy<IHttpProvider> httpProvider, Lazy<IStringLocalizer<CoreProvider>> localizer
        , Lazy<ILogger<CoreProvider>> logger)
        : InfrastructureBase<CoreProvider>(httpProvider, localizer, logger), ICoreProvider
    {
        public async Task<ResultData<bool>> ValidateTestAsync([NotNull] TestTimeRequestDto requestDto)
        {
            try
            {
                var response = await HttpProvider.Value.GetAsync<IHttpRequest, CoreTestInformationResponse, IHttpRequest>(new()
                {
                    Uri = string.Format(configuration.Value.GetValue<string>("Core:Test")!, requestDto.TestId),
                    Request = null,
                });
                if (response is null)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["GeneralError"], }] };
                }

                if (response.Status != 1 || response.Data is null)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["TestNotFound"], }] };
                }

                var valid = response.Data.AnswerId == requestDto.SubmissionId;
                return new(OperationResult.Succeeded)
                {
                    Data = valid,
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<ExamResultResponseDto>> GetExamResultAsync([NotNull] ExamResultRequestDto requestDto)
        {
            try
            {
                var response = await HttpProvider.Value.GetAsync<IHttpRequest, CoreExamResultResponse, IHttpRequest>(new()
                {
                    Uri = string.Format(configuration.Value.GetValue<string>("Core:ExamResult")!, requestDto.ExamId),
                    Request = null,
                    HeaderParameters = [("Authorization", $"Bearer {requestDto.SecretKey}")],
                });
                if (response is null)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["GeneralError"], }] };
                }

                if (response.Status != 1 || response.Data?.AnswerStats?.Total is null)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["ExamNotFound"], }] };
                }

                ExamResultResponseDto result = new()
                {
                    Invalid = response.Data.AnswerStats.Total.False,
                    Valid = response.Data.AnswerStats.Total.True,
                    NoAnswer = response.Data.AnswerStats.Total.NoAnswer,
                    Total = response.Data.AnswerStats.Total.Num,
                    Percent = response.Data.AnswerStats.Total.Percent,
                };
                return new(OperationResult.Succeeded)
                {
                    Data = result,
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<ExamInformationResponseDto>> GetExamInformationAsync([NotNull] ExamInformationRequestDto requestDto)
        {
            try
            {
                var response = await HttpProvider.Value.GetAsync<IHttpRequest, CoreExamInformationResponse, IHttpRequest>(new()
                {
                    Uri = string.Format(configuration.Value.GetValue<string>("Core:ExamInfo")!, requestDto.ExamId),
                    Request = null,
                    HeaderParameters = [("Authorization", $"Bearer {requestDto.SecretKey}")],
                });

                if (response is null)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["GeneralError"], }] };
                }

                if (response.Data?.Exam is null)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["ExamNotFound"], }] };
                }

                var qr = QRCodeImageBuilder.GetPngBytes($"https://core.gamatrain.com/azmoon/detail/{response.Data.Exam.Code}");

                ExamInformationResponseDto result = new()
                {
                    Exam = new()
                    {
                        Title = response.Data.Exam.Title,
                        TestsCount = response.Data.Exam.TestsCount.ValueOf<int>(),
                        StartDate = response.Data.Exam.StartDate,
                        EndDate = response.Data.Exam.EndDate,
                        ExamTime = response.Data.Exam.ExamTime,
                        ExamType = response.Data.Exam.ExamType,
                        Type = response.Data.Exam.Type,
                        ScoreType = response.Data.Exam.ScoreType,
                        QrCode = $"data:img/png;base64, {Convert.ToBase64String(qr)}",
                    },
                    Tests = response.Data?.Tests?.Select(t => new ExamInformationResponseDto.TestDto
                    {
                        Question = t.Question,
                        QuestionFile = t.QuestionFile,
                        OptionA = t.OptionA,
                        OptionAFile = t.OptionAFile,
                        OptionB = t.OptionB,
                        OptionBFile = t.OptionBFile,
                        OptionC = t.OptionC,
                        OptionCFile = t.OptionCFile,
                        OptionD = t.OptionD,
                        OptionDFile = t.OptionDFile,
                    }).ToList(),
                };
                return new(OperationResult.Succeeded)
                {
                    Data = result,
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<UserInformationResponseDto>> GetUserInformationAsync([NotNull] UserInformationRequestDto requestDto)
        {
            try
            {
                var response = await HttpProvider.Value.GetAsync<IHttpRequest, CoreUserInformationResponse, IHttpRequest>(new()
                {
                    Uri = configuration.Value.GetValue<string>("Core:UserInfo"),
                    Request = null,
                    HeaderParameters = [("Authorization", $"Bearer {requestDto.Token}")],
                });
                if (response is null)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["GeneralError"], }] };
                }

                if (response.Data is null)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["InvalidToken"], }] };
                }

                UserInformationResponseDto data = new()
                {
                    FirstName = response.Data.FirstName,
                    LastName = response.Data.LastName,
                    PhoneNumber = response.Data.Phone,
                    Gender = MapGender(response.Data.Sex),
                    Grade = response.Data.Grade.ValueOf<int?>(),
                    CoreId = response.Data.CoreId.ValueOf<int?>(),
                    Group = response.Data.Group,
                };
                if (!string.IsNullOrEmpty(response.Data.Avatar))
                {
                    var avatar = await HttpProvider.Value.GetByteArrayAsync<IHttpRequest, IHttpRequest>(new()
                    {
                        Uri = response.Data.Avatar,
                        Request = null,
                    });
                    if (avatar is not null)
                    {
                        data.Avatar = $"data:image/{Path.GetExtension(response.Data.Avatar).Trim('.')};base64,{Convert.ToBase64String(avatar)}";
                    }
                }

                return new(OperationResult.Succeeded)
                {
                    Data = data,
                };

                static GenderType? MapGender(string? sex) => sex switch
                {
                    "1" => GenderType.Male,
                    "2" => GenderType.Female,
                    _ => null,
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }
    }
}
