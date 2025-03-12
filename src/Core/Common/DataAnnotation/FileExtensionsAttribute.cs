namespace GamaEdtech.Common.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Core.Extensions.Collections.Generic;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using Microsoft.AspNetCore.StaticFiles;

    using MimeDetective;
    using MimeDetective.Storage;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FileExtensionsAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly Lazy<(IContentInspector ContentInspector, FileExtensionContentTypeProvider ContentTypeProvider)> inspector;

#pragma warning disable CA1019 // Define accessors for attribute arguments
        public FileExtensionsAttribute(string? extensions)
#pragma warning restore CA1019 // Define accessors for attribute arguments
        {
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_FileExtensions);
            Extensions = extensions?.Split(Constants.JoinDelimiter, StringSplitOptions.RemoveEmptyEntries);

            inspector = new(() =>
            {
                var definitions = new MimeDetective.Definitions.ExhaustiveBuilder
                {
                    UsageType = MimeDetective.Definitions.Licensing.UsageType.PersonalNonCommercial,
                }.Build();

                var contentInspector = new ContentInspectorBuilder
                {
                    Definitions = definitions.ScopeExtensions(Extensions!).TrimMeta().TrimDescription().ToImmutableArray(),
                    Parallel = true,
                }.Build();

                return (contentInspector, new FileExtensionContentTypeProvider());
            });
        }

        public string[]? Extensions { get; private set; }

        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return true;
            }

            if (value is IEnumerable<IFormFile> lst)
            {
                foreach (var item in lst)
                {
                    if (!Validate(item))
                    {
                        return false;
                    }
                }

                return true;
            }

            return value is IFormFile file && Validate(file);

            bool Validate(IFormFile file)
            {
                if (!Extensions!.Exists(t => t.Equals(Path.GetExtension(file.FileName).TrimStart('.'), StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }

                _ = inspector.Value.ContentTypeProvider.TryGetContentType(file.FileName, out var contentType);
                if (file.ContentType != contentType)
                {
                    return false;
                }

                var results = inspector.Value.ContentInspector.Inspect(file.OpenReadStream());
                return results.Length == 0 || results.ByFileExtension().Exists(t => Extensions!.Exists(e => t.Extension.Equals(e, StringComparison.OrdinalIgnoreCase)));
            }
        }

        public void AddValidation([NotNull] ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-extensions-extension", string.Join(",", Extensions!)));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name!))!);
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-extensions", Data.Error.FormatMessage(msg)));
        }

        public override string FormatErrorMessage(string name) => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, string.Join(",", Extensions!));
    }
}
