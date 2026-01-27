namespace GamaEdtech.Application.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Core.Extensions.Linq;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Common.Service.Factory;
    using GamaEdtech.Data.Dto.SiteMap;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    using Error = Common.Data.Error;

    public class GlobalService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<GlobalService>> localizer, Lazy<ILogger<GlobalService>> logger
            , Lazy<IGenericFactory<ICaptchaProvider, CaptchaProviderType>> genericFactory, Lazy<IConfiguration> configuration, Lazy<IEnumerable<ISiteMapHandler>> siteMapHandlers, Lazy<IWebHostEnvironment> environment)
        : LocalizableServiceBase<GlobalService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IGlobalService
    {
        public async Task<ResultData<bool>> VerifyCaptchaAsync(string? captcha)
        {
            try
            {
                _ = configuration.Value.GetValue<string?>("Captcha:Type").TryGetFromNameOrValue<CaptchaProviderType, byte>(out var captchaProviderType);

                return await genericFactory.Value.GetProvider(captchaProviderType!)!.VerifyCaptchaAsync(captcha);
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = new[] { new Error { Message = exc.Message }, } };
            }
        }

        public async Task<ResultData<ListDataSource<SiteMapDto>>> GetSiteMapsAsync(ListRequestDto<SiteMap>? requestDto = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<SiteMap>().GetManyQueryable(requestDto?.Specification).FilterListAsync(requestDto?.PagingDto);
                var lst = await result.List.Select(t => new SiteMapDto
                {
                    Id = t.Id,
                    Priority = t.Priority,
                    ChangeFrequency = t.ChangeFrequency,
                    IdentifierId = t.IdentifierId!.Value,
                    ItemType = t.ItemType,
                }).ToListAsync();
                return new(OperationResult.Succeeded) { Data = new() { List = lst, TotalRecordsCount = result.TotalRecordsCount } };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<long>> ManageSiteMapAsync([NotNull] ManageSiteMapRequestDto requestDto)
        {
            try
            {
                if (requestDto.ChangeFrequency is null && !requestDto.Priority.HasValue)
                {
                    return new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["ChangeFrequencyAndPriorityCanNotBeNull"] },],
                    };
                }

                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<SiteMap>();
                SiteMap? siteMap = null;

                if (requestDto.Id.HasValue)
                {
                    siteMap = await repository.GetAsync(requestDto.Id.Value);
                    if (siteMap is null)
                    {
                        return new(OperationResult.NotFound)
                        {
                            Errors = [new() { Message = Localizer.Value["SiteMapNotFound"] },],
                        };
                    }

                    siteMap.Priority = requestDto.Priority;
                    siteMap.ChangeFrequency = requestDto.ChangeFrequency;

                    _ = repository.Update(siteMap);
                }
                else
                {
                    siteMap = await repository.GetAsync(t => t.IdentifierId == requestDto.IdentifierId && t.ItemType == requestDto.ItemType);
                    if (siteMap is null)
                    {
                        siteMap = new SiteMap
                        {
                            ChangeFrequency = requestDto.ChangeFrequency,
                            IdentifierId = requestDto.IdentifierId,
                            ItemType = requestDto.ItemType,
                            Priority = requestDto.Priority,
                        };
                        repository.Add(siteMap);
                    }
                    else
                    {
                        siteMap.Priority = requestDto.Priority;
                        siteMap.ChangeFrequency = requestDto.ChangeFrequency;

                        _ = repository.Update(siteMap);
                    }

                }

                _ = await uow.SaveChangesAsync();

                return new(OperationResult.Succeeded) { Data = siteMap.Id };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<bool>> RemoveSiteMapAsync([NotNull] ISpecification<SiteMap> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<SiteMap>();
                var siteMap = await repository.GetAsync(specification);
                if (siteMap is null)
                {
                    return new(OperationResult.NotFound)
                    {
                        Data = false,
                        Errors = [new() { Message = Localizer.Value["SiteMapNotFound"] },],
                    };
                }

                repository.Remove(siteMap);
                _ = await uow.SaveChangesAsync();
                return new(OperationResult.Succeeded) { Data = true };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> GenerateSiteMapAsync()
        {
            try
            {
                var dir = Path.Combine(environment.Value.WebRootPath, "sitemap");
                if (!Directory.Exists(dir))
                {
                    _ = Directory.CreateDirectory(dir);
                }
                var oldFiles = Directory.GetFiles(dir);
                foreach (var file in oldFiles)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var data = await uow.GetRepository<SiteMap>().GetManyQueryable().Select(t => new
                {
                    t.IdentifierId,
                    t.Priority,
                    t.ChangeFrequency,
                    t.ItemType,
                }).ToListAsync();

                List<SiteMapItemDto> nodes = [];
                foreach (var handler in siteMapHandlers.Value)
                {
                    var lst = await handler.GetSiteMapDataAsync();
                    if (lst.Data is not null)
                    {
                        for (var j = 0; j < lst.Data.Count; j++)
                        {
                            var node = lst.Data[j];
                            var item = data.Find(t => t.IdentifierId == node.Id && t.ItemType == node.ItemType);
                            if (item?.ChangeFrequency is not null)
                            {
                                node.ChangeFrequency = item.ChangeFrequency;
                            }

                            if (item?.Priority is not null)
                            {
                                node.Priority = item.Priority.Value;
                            }

                            nodes.Add(node);
                        }
                    }
                }
                var chunks = nodes.Chunk(50000);
                var i = 0;
                StringBuilder sb = new();
                _ = sb.Append("<sitemapindex xmlns=\"https://www.example.com/schemas/sitemap/0.84\">");
                foreach (var item in chunks)
                {
                    i++;
                    _ = sb.AppendFormat(@"
<sitemap>
    <loc>https://gamatrain.com/sitemap/sitemap{0}.xml</loc>
</sitemap>
", i);

                    StringBuilder nested = new();
                    _ = nested.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
                    for (var j = 0; j < item.Length; j++)
                    {
                        _ = nested.AppendFormat(@"
<url>
    <loc>https://gamatrain.com/{0}/{1}/{2}</loc>
    <lastmod>{3}</lastmod>
    <changefreq>{4}</changefreq>
    <priority>{5}</priority>
</url>
", item[j].ItemType.Identifier, item[j].Id, item[j].Title.Slugify(), item[j].LastModifyDate, item[j].ChangeFrequency.Name.ToLowerInvariant(), item[j].Priority);
                    }
                    _ = nested.Append("</urlset>");
                    await File.WriteAllTextAsync(Path.Combine(dir, $"sitemap{i}.xml"), nested.ToString());
                }
                _ = sb.Append("</sitemapindex>");

                await File.WriteAllTextAsync(Path.Combine(dir, "sitemap.xml"), sb.ToString());

                return new(OperationResult.Succeeded) { Data = true };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = new[] { new Error { Message = exc.Message }, } };
            }
        }
    }
}
