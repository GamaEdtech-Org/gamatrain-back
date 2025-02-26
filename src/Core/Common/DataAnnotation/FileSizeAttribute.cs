namespace GamaEdtech.Common.DataAnnotation
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;

    using GamaEdtech.Common.Core;

    using GamaEdtech.Common.Core.Extensions.Collections.Generic;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FileSizeAttribute : ValidationAttribute, IClientModelValidator
    {
        public FileSizeAttribute(long maximumLength)
        {
            ErrorMessageResourceName = nameof(Resources.GlobalResource.Validation_FileSize);
            MaximumLength = maximumLength;
        }

        public long MaximumLength { get; }

        public long? MaximumTotalLength { get; set; }

        public override bool IsValid(object? value) => value is null
|| (value is IEnumerable<IFormFile> lst
                ? (!MaximumTotalLength.HasValue || !(lst.Where(t => t is not null).Sum(t => t.Length) > MaximumTotalLength))
&& lst.All(t => t.Length <= MaximumLength)
                : value is IFormFile file && file.Length <= MaximumLength);

        public void AddValidation([NotNull] ClientModelValidationContext context)
        {
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-filesize-maxsize", MaximumLength.ToString()));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name!))!);
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-filesize", Data.Error.FormatMessage(msg)));

            if (MaximumTotalLength.HasValue)
            {
                _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-filesize-maxsizetotal", MaximumTotalLength.Value.ToString()));
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, FormatBytes(MaximumLength));

            static string FormatBytes(long bytes)
            {
                if (bytes >= 0x1000000000000000)
                {
                    return ((double)(bytes >> 50) / 1024).ToString("0.### EB");
                }

                if (bytes >= 0x4000000000000)
                {
                    return ((double)(bytes >> 40) / 1024).ToString("0.### PB");
                }

                var tb = bytes >= 0x10000000000;
                if (tb)
                {
                    return ((double)(bytes >> 30) / 1024).ToString("0.### TB");
                }

                var gb = bytes >= 0x40000000;
                if (gb)
                {
                    return ((double)(bytes >> 20) / 1024).ToString("0.### GB");
                }

                var mb = bytes >= 0x100000;
                if (mb)
                {
                    return ((double)(bytes >> 10) / 1024).ToString("0.### MB");
                }

                var kb = bytes >= 0x400;
                return kb ? ((double)bytes / 1024).ToString("0.###") + " KB"
                    : bytes.ToString("0 Bytes");
            }
        }
    }
}
