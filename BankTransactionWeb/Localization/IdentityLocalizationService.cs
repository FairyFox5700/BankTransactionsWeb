﻿using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BankTransaction.Web.Localization
{
    public class IdentityLocalizationService
    {
        private readonly IStringLocalizer stringLocalizer;
        public IdentityLocalizationService(IStringLocalizerFactory localizerFactory)
        {
            var type = typeof(IdentityResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            stringLocalizer = localizerFactory.Create("IdentityResource", assemblyName.Name);
        }
        public LocalizedString GetLocalizedHtmlString(string key)
        {
            return stringLocalizer[key];
        }
        public LocalizedString GetLocalizedHtmlString(string key, string parameter)
        {
            return stringLocalizer[key, parameter];
        }
    }

    internal class IdentityResource
    {
    }
}
