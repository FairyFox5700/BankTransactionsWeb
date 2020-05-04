using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BankTransaction.Web.Localization
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly ConcurrentDictionary<string, IStringLocalizer> cache = new ConcurrentDictionary<string, IStringLocalizer>();

        private readonly string resourcesRelativePath;

        public JsonStringLocalizerFactory(IOptions<LocalizationOptions> localizationOptions, ILoggerFactory loggerFactory)
        {
            if (localizationOptions == null)
            {
                throw new ArgumentNullException(nameof(localizationOptions));
            }
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
            resourcesRelativePath = localizationOptions.Value.ResourcesPath ?? string.Empty;
            if (!string.IsNullOrEmpty(resourcesRelativePath))
            {
                resourcesRelativePath = resourcesRelativePath.Replace(Path.AltDirectorySeparatorChar, '.')
                    .Replace(Path.DirectorySeparatorChar, '.') + ".";
            }
            this.loggerFactory = loggerFactory;
        }
        
        public IStringLocalizer Create(Type resourceSource)
        {
            var resourceType = resourceSource.GetTypeInfo();
            var cultureInfo = CultureInfo.CurrentUICulture;
            var resourceName = String.Empty;
            if (string.IsNullOrEmpty(resourcesRelativePath))
            {
                resourceName = resourceType.FullName;
            }
            else
            {
               var assemblyName = new AssemblyName(resourceType.Assembly.FullName).Name;
               resourceName =  resourcesRelativePath + TrimPrefix(resourceType.FullName, assemblyName + ".");
            }
            resourceName = $"{resourceName}.{cultureInfo.Name}.Res.json";
            return GetCachedLocalizator(resourceName, resourceType.Assembly, cultureInfo);
        }
        protected  string GetResourcePrefix(string location, string baseName, string resourceLocation)
        {
            // Re-root the base name if a resources path is set
            return location + "." + resourceLocation + TrimPrefix(baseName, location + ".");
        }
        private static string TrimPrefix(string name, string prefix)
        {
            if (name.StartsWith(prefix, StringComparison.Ordinal))
            {
                return name.Substring(prefix.Length);
            }

            return name;
        }
        public IStringLocalizer Create(string baseName, string location)
        {
            var cultureInfo = CultureInfo.CurrentUICulture;
            var resourceName = $"{baseName}.Res.json";
            return GetCachedLocalizator(resourceName, Assembly.GetEntryAssembly(), cultureInfo);
        }

        private IStringLocalizer GetCachedLocalizator(string resourceName, Assembly assembly, CultureInfo cultureInfo)
        {
            string key = GetCackeKey(resourceName, assembly, cultureInfo);
            return cache.GetOrAdd(key, new JsonStringLocalizer(resourceName, assembly, cultureInfo, loggerFactory.CreateLogger<JsonStringLocalizer>()));

        }

        private string GetCackeKey(string resourceName, Assembly assembly, CultureInfo cultureInfo)
        {
            return assembly.FullName +"_"+ resourceName +"_" + cultureInfo.Name;
        }

       
    }
}
