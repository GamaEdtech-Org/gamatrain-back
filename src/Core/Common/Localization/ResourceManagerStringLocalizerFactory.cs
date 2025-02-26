namespace GamaEdtech.Common.Localization
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using System.Resources;

    using GamaEdtech.Common.Core;

    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class ResourceManagerStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IResourceNamesCache resourceNamesCache = new ResourceNamesCache();
        private readonly ConcurrentDictionary<string, ResourceManagerStringLocalizer> localizerCache = new();
        private readonly string? resourcesRelativePath;
        private readonly ILoggerFactory loggerFactory;

        public ResourceManagerStringLocalizerFactory(IOptions<LocalizationOptions> localizationOptions, ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(localizationOptions);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            resourcesRelativePath = localizationOptions.Value.ResourcesPath ?? string.Empty;
            this.loggerFactory = loggerFactory;

            if (!string.IsNullOrEmpty(resourcesRelativePath))
            {
                resourcesRelativePath = resourcesRelativePath.Replace(Path.AltDirectorySeparatorChar, '.')
                    .Replace(Path.DirectorySeparatorChar, '.') + ".";
            }
        }

        public IStringLocalizer Create(Type? resourceSource)
        {
            ArgumentNullException.ThrowIfNull(resourceSource);

            var typeInfo = resourceSource.GetTypeInfo();
            var baseName = GetResourcePrefix(typeInfo).PrepareResourcePath()!;

            var assemblyName = typeInfo.Assembly.FullName.PrepareResourcePath()!;
            var assembly = Assembly.Load(assemblyName!);

            return localizerCache.GetOrAdd(baseName!, _ => CreateResourceManagerStringLocalizer(assembly, baseName));
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            ArgumentNullException.ThrowIfNull(baseName);
            ArgumentNullException.ThrowIfNull(location);

            baseName = GetResourcePrefix(baseName, location).PrepareResourcePath()!;
            location = location.PrepareResourcePath()!;

            return localizerCache.GetOrAdd($"B={baseName},L={location}", _ =>
            {
                var assemblyName = new AssemblyName(location);
                var assembly = Assembly.Load(assemblyName);

                return CreateResourceManagerStringLocalizer(assembly, baseName);
            });
        }

        protected virtual ResourceManagerStringLocalizer CreateResourceManagerStringLocalizer(Assembly assembly, [NotNull] string baseName) => new(
                new ResourceManager(baseName, assembly),
                assembly,
                baseName,
                resourceNamesCache,
                loggerFactory.CreateLogger<ResourceManagerStringLocalizer>());

        protected virtual string? GetResourcePrefix(TypeInfo typeInfo)
        {
            ArgumentNullException.ThrowIfNull(typeInfo);

            return GetResourcePrefix(typeInfo, GetRootNamespace(typeInfo.Assembly), GetResourcePath(typeInfo.Assembly));
        }

        protected virtual string? GetResourcePrefix(TypeInfo typeInfo, string? baseNamespace, string? resourcesRelativePath)
        {
            ArgumentNullException.ThrowIfNull(typeInfo);
            ArgumentNullException.ThrowIfNull(baseNamespace);
            ArgumentException.ThrowIfNullOrEmpty(typeInfo.FullName);

            if (string.IsNullOrEmpty(resourcesRelativePath))
            {
                return typeInfo.FullName;
            }
            else
            {
                // This expectation is defined by dotnet's automatic resource storage.
                // We have to conform to "{RootNamespace}.{ResourceLocation}.{FullTypeName - RootNamespace}".
                return baseNamespace + "." + resourcesRelativePath + TrimPrefix(typeInfo.FullName, baseNamespace + ".");
            }
        }

        protected virtual string? GetResourcePrefix(string baseResourceName, string? baseNamespace)
        {
            ArgumentNullException.ThrowIfNull(baseResourceName);
            ArgumentNullException.ThrowIfNull(baseNamespace);

            var assemblyName = new AssemblyName(baseNamespace);
            var assembly = Assembly.Load(assemblyName);
            var rootNamespace = GetRootNamespace(assembly);
            var resourceLocation = GetResourcePath(assembly);
            var locationPath = rootNamespace + "." + resourceLocation;

            baseResourceName = locationPath + TrimPrefix(baseResourceName, baseNamespace + ".");

            return baseResourceName;
        }

        protected virtual string? GetResourcePrefix(string location, [NotNull] string baseName, string? resourceLocation) => location + "." + resourceLocation + TrimPrefix(baseName, location + ".");

        protected virtual ResourceLocationAttribute? GetResourceLocationAttribute([NotNull] Assembly assembly) => assembly.GetCustomAttribute<ResourceLocationAttribute>();

        protected virtual RootNamespaceAttribute? GetRootNamespaceAttribute([NotNull] Assembly assembly) => assembly.GetCustomAttribute<RootNamespaceAttribute>();

        private static string? TrimPrefix(string name, string prefix) => name.StartsWith(prefix, StringComparison.Ordinal) ? name[prefix.Length..] : name;

        private string? GetRootNamespace(Assembly assembly)
        {
            var rootNamespaceAttribute = GetRootNamespaceAttribute(assembly);
            return rootNamespaceAttribute is not null ? rootNamespaceAttribute.RootNamespace : assembly.GetName().Name;
        }

        private string? GetResourcePath(Assembly assembly)
        {
            var resourceLocationAttribute = GetResourceLocationAttribute(assembly);

            // If we don't have an attribute assume all assemblies use the same resource location.
            var resourceLocation = resourceLocationAttribute is null
                ? resourcesRelativePath
                : resourceLocationAttribute.ResourceLocation + ".";
            resourceLocation = resourceLocation!
                .Replace(Path.DirectorySeparatorChar, '.')
                .Replace(Path.AltDirectorySeparatorChar, '.');

            return resourceLocation;
        }
    }
}
