using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Threading.Tasks;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface INeonTetraResources : IDynamicMetaObjectProvider
    {
        ISpecificCultureTranslations TranslationsForCulture(string culture);

        IEnumerable<ISpecificCultureTranslations> AllTranslations();

        Task<string> GetStringAsync(string key, CultureInfo cultureInfoOverride = null);

        string GetString(string key);
    }
}