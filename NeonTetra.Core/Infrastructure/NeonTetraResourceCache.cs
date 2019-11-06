using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class NeonTetraResourceCache : DynamicObject, INeonTetraResources
    {
        private readonly ISpecificCultureTranslations _defaultResources;
        private readonly IList<ISpecificCultureTranslations> _resourceCaches;

        public NeonTetraResourceCache(IEnumerable<ISpecificCultureTranslations> resourceCaches)
        {
            if (resourceCaches == null) throw new ArgumentNullException("resourceCaches");
            _resourceCaches = (from r in resourceCaches
                               where r.CultureInfo != null && r.Translations != null && r.Translations.Any()
                               select r).ToList();
            _defaultResources =
                (from r in _resourceCaches where r != null && r.LCID == CultureInfo.InvariantCulture.LCID select r)
                ?.FirstOrDefault();
        }

        public ISpecificCultureTranslations TranslationsForCulture(string culture)
        {
            return (from c in _resourceCaches
                    where
                        c.CultureInfo != null &&
                        (
                            c.CultureInfo.LCID.ToString().Equals(culture)
                            || c.CultureInfo.DisplayName.Equals(culture, StringComparison.InvariantCultureIgnoreCase)
                            || c.CultureInfo.Name.Equals(culture, StringComparison.InvariantCultureIgnoreCase)
                            || c.CultureInfo.EnglishName.Equals(culture, StringComparison.InvariantCultureIgnoreCase)
                            || c.CultureInfo.NativeName.Equals(culture, StringComparison.InvariantCultureIgnoreCase)
                            || c.CultureInfo.ThreeLetterISOLanguageName.Equals(culture,
                                StringComparison.InvariantCultureIgnoreCase)
                            || c.CultureInfo.ThreeLetterWindowsLanguageName.Equals(culture,
                                StringComparison.InvariantCultureIgnoreCase)
                            || c.CultureInfo.TwoLetterISOLanguageName.Equals(culture,
                                StringComparison.InvariantCultureIgnoreCase)
                        )
                    select c)?.FirstOrDefault();
        }

        public IEnumerable<ISpecificCultureTranslations> AllTranslations()
        {
            return _resourceCaches;
        }

        public Task<string> GetStringAsync(string key, CultureInfo cultureInfoOverride = null)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            var lcid = cultureInfoOverride?.LCID ?? Thread.CurrentThread.CurrentCulture.LCID;
            if (_resourceCaches == null) return Task.FromResult(key);
            var selectedTranslations = (from t in _resourceCaches where t.LCID == lcid select t)?.FirstOrDefault();

            if (selectedTranslations != null)
                if (selectedTranslations.Translations.ContainsKey(key))
                    return Task.FromResult(selectedTranslations.Translations[key]);

            if (_defaultResources != null && _defaultResources.Translations != null &&
                _defaultResources.Translations.ContainsKey(key))
                return Task.FromResult(_defaultResources.Translations[key]);

            return Task.FromResult(key);
        }

        public string GetString(string key)
        {
            return GetStringDefaultLocale(key);
        }

        private async Task<string> GetStringInternal(string key, CultureInfo mainThreadCultureInfo = null)
        {
            return await GetStringAsync(key, mainThreadCultureInfo ?? Thread.CurrentThread.CurrentCulture);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var key = binder.Name;
            result = GetStringDefaultLocale(key);
            return true;
        }

        private string GetStringDefaultLocale(string key)
        {
            var mainThreadCultureInfo = Thread.CurrentThread.CurrentCulture;
            return Task.Factory.StartNew(obj => { return GetStringInternal(key, mainThreadCultureInfo).Result; },
                mainThreadCultureInfo).Result;
        }
    }
}