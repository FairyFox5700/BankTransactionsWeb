using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankTransaction.Web.Localization
{
    public class JsonStringLocalizer : IStringLocalizer
    {
      //  List<JsonLocalization> localization = new List<JsonLocalization>();
       // private readonly Lazy<Dictionary<string, string>> _fallbackResources;
        private readonly Lazy<Dictionary<string, string>> localization;
        public ILogger<JsonStringLocalizer> Logger;

        public string ResourceName { get; }
        public Assembly Assembly { get; }
        public CultureInfo CultureInfo { get; }
      
        public JsonStringLocalizer(string resourceName, Assembly assembly, CultureInfo cultureInfo, ILogger<JsonStringLocalizer> logger)
        {
            ResourceName = resourceName ?? throw new ArgumentNullException(nameof(resourceName));
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            CultureInfo = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            localization = new Lazy<Dictionary<string, string>>(ReadResources(resourceName, assembly, cultureInfo.Parent, logger));

        }

        private  Dictionary<string, string> ReadResources(string resourceName, Assembly resourceAssembly, CultureInfo cultureInfo, ILogger<JsonStringLocalizer> logger)
        {
          //for test purposes only
            var names = this.GetType().Assembly.GetManifestResourceNames();
            var currenAssembyNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            var name = resourceAssembly.GetName().Name;
            resourceName = name + "." + resourceName; ;
            //resourceName.Replace(name, "");
            var stream = resourceAssembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                logger.LogInformation($"Resource '{resourceName}' not found for '{cultureInfo.Name}'.");
                return new Dictionary<string, string>();
            }
            string json;
            using (var reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
        public LocalizedString this[string name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                if (TryGetString(name, out string value))
                {
                    return new LocalizedString(name, value, resourceNotFound: false);
                }
                return new LocalizedString(name, value, resourceNotFound: true);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                if (TryGetString(name, out string value))
                {
                    return new LocalizedString(name, String.Format(value, arguments), resourceNotFound: false);
                }
                return new LocalizedString(name, String.Format(value, arguments), resourceNotFound: true);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return localization.Value.Select(r => new LocalizedString(r.Key, r.Value));
        }



        private bool TryGetString(string name, out string value)
        {
           return localization.Value.TryGetValue(name, out value);
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            //Obsolete API
            throw new System.NotSupportedException();
        }

    }
}
