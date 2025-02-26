namespace GamaEdtech.Common.ModelBinding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using GamaEdtech.Common.Data.Enumeration;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class EnumerationQueryStringModelBinder<TEnum, TKey> : IModelBinder
        where TEnum : Enumeration<TKey>
        where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext);

            var enumerationName = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            if (enumerationName.Length == 0)
            {
                // 100000 is just temporary
                List<string> list = [];
                for (var i = 0; i < 100000; i++)
                {
                    var tmp = bindingContext.ValueProvider.GetValue(bindingContext.FieldName + $"[{i}]");
                    if (tmp.Length == 0)
                    {
                        break;
                    }

                    list.AddRange(tmp.Values);
                }

                if (list.Count > 0)
                {
                    enumerationName = new ValueProviderResult(new Microsoft.Extensions.Primitives.StringValues([.. list]));
                }
            }

            List<TEnum> lst = new(enumerationName.Length);
            foreach (var item in enumerationName)
            {
                if (item.TryGetFromNameOrValue<TEnum, TKey>(out var result))
                {
                    lst.Add(result!);
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    var msg = Resources.GlobalResource.Validation_AttemptedValueIsInvalidAccessor;
                    bindingContext.ModelState.AddModelError(bindingContext.FieldName, string.Format(msg, item, bindingContext.ModelName));
                    return Task.CompletedTask;
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
