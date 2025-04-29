namespace GamaEdtech.Common.ModelBinding
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using GamaEdtech.Common.Data.Enumeration;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class EnumerationQueryStringModelBinder(Type modelType) : IModelBinder
    {
        public Task BindModelAsync([NotNull] ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            string?[] lst = [valueProviderResult.FirstValue];
            var single = true;
            var type = modelType;
            if (modelType.HasElementType)
            {
                type = modelType.GetElementType()!;
                lst = [.. valueProviderResult.Values];
                single = false;
            }
            else if (typeof(System.Collections.IEnumerable).IsAssignableFrom(modelType))
            {
                type = modelType.GenericTypeArguments[0];
                lst = [.. valueProviderResult.Values];
                single = false;
            }

            var all = EnumerationExtensions.GetAll(type);
            if (all is null)
            {
                return Task.CompletedTask;
            }


            var values = new List<object>();
            for (var i = 0; i < lst.Length; i++)
            {
                var item = lst[i];
                var valid = false;
#pragma warning disable S3267 // Loops should be simplified with "LINQ" expressions
                foreach (var child in all)
                {
                    if (child!.ToString()!.Equals(item, StringComparison.OrdinalIgnoreCase))
                    {
                        values!.Add(child);
                        valid = true;
                        break;
                    }
                }
#pragma warning restore S3267 // Loops should be simplified with "LINQ" expressions

                if (!valid)
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    var msg = Resources.GlobalResource.Validation_AttemptedValueIsInvalidAccessor;
                    bindingContext.ModelState.AddModelError(bindingContext.FieldName, string.Format(msg, item, modelName));
                    return Task.CompletedTask;
                }
            }

            object? model = values;
            if (single)
            {
                model = values.FirstOrDefault();
            }
            else if (modelType.HasElementType)
            {
                model = values.ToArray();
            }

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
