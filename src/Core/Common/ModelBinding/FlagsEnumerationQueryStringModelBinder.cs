namespace GamaEdtech.Common.ModelBinding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using GamaEdtech.Common.Data.Enumeration;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class FlagsEnumerationQueryStringModelBinder<TFlagsEnum> : IModelBinder
        where TFlagsEnum : FlagsEnumeration<TFlagsEnum>, new()
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext);

            var enumerationName = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            if (enumerationName.Length == 0)
            {
                // 100000 is just temporary
                List<string> lst = [];
                for (var i = 0; i < 100000; i++)
                {
                    var tmp = bindingContext.ValueProvider.GetValue(bindingContext.FieldName + $"[{i}]");
                    if (tmp.Length == 0)
                    {
                        break;
                    }

                    lst.AddRange(tmp.Values);
                }

                if (lst.Count > 0)
                {
                    enumerationName = new ValueProviderResult(new Microsoft.Extensions.Primitives.StringValues([.. lst]));
                }
            }

            TFlagsEnum? result = null;
            foreach (var item in enumerationName)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                var flagsEnum = item.FromName<TFlagsEnum>();
                if (flagsEnum is not null)
                {
                    result = result is null ? flagsEnum : result | flagsEnum;
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    var msg = Resources.GlobalResource.Validation_AttemptedValueIsInvalidAccessor;
                    bindingContext.ModelState.AddModelError(bindingContext.FieldName, string.Format(msg, item, bindingContext.ModelName));
                    return Task.CompletedTask;
                }
            }

            bindingContext.Result = ModelBindingResult.Success(result);

            return Task.CompletedTask;
        }
    }
}
