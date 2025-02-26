namespace GamaEdtech.Common.ModelBinding
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.TimeZone;

    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.DependencyInjection;

    public class DateTimeOffsetQueryStringModelBinder : IModelBinder
    {
        public Task BindModelAsync([NotNull] ModelBindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext);

            var values = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            List<DateTimeOffset> lst = new(values.Length);
            foreach (var item in values)
            {
                var dateTime = item.ValueOf<DateTime?>();
                if (!dateTime.HasValue)
                {
                    var msg = Resources.GlobalResource.Validation_AttemptedValueIsInvalidAccessor;
                    bindingContext.Result = ModelBindingResult.Failed();
                    bindingContext.ModelState.AddModelError(bindingContext.FieldName, string.Format(msg, item, bindingContext.ModelName));
                    return Task.CompletedTask;
                }

                if (dateTime.Value.Kind == DateTimeKind.Unspecified)
                {
                    lst.Add(new DateTimeOffset(dateTime.Value, GetTimeSpan()));
                }
                else
                {
                    lst.Add(item.ValueOf<DateTimeOffset>());
                }

                TimeSpan GetTimeSpan()
                {
                    var timeZoneId = bindingContext.HttpContext?.User.FindFirstValue(Constants.TimeZoneIdClaim) ?? Constants.UtcTimeZoneId;
                    return bindingContext.HttpContext?.RequestServices.GetRequiredService<ITimeZoneProvider>().GetTimeZones()?.FirstOrDefault(t => t.Id == timeZoneId)?.BaseUtcOffset ?? Constants.BaseUtcOffset;
                }
            }

            var isEnumerable = typeof(System.Collections.IEnumerable).IsAssignableFrom(bindingContext.ModelType);
            if (lst.Count > 0)
            {
                bindingContext.Result = isEnumerable ? ModelBindingResult.Success(lst) : ModelBindingResult.Success(lst[0]);
            }

            return Task.CompletedTask;
        }
    }
}
