namespace GamaEdtech.Common.ModelBinding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using GamaEdtech.Common.Core;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    using NUlid;

    public class UlidQueryStringModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext);

            var values = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            List<Ulid> lst = new(values.Length);
            foreach (var item in values)
            {
                var ulid = item.ValueOf<Ulid?>();
                if (!ulid.HasValue)
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    var msg = Resources.GlobalResource.Validation_AttemptedValueIsInvalidAccessor;
                    bindingContext.ModelState.AddModelError(bindingContext.FieldName, string.Format(msg, item, bindingContext.ModelName));
                    return Task.CompletedTask;
                }

                lst.Add(ulid.Value);
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
