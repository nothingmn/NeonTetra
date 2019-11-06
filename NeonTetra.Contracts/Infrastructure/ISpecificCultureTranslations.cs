using System.Collections.Generic;
using System.Globalization;

namespace NeonTetra.Contracts.Infrastructure
{
    public interface ISpecificCultureTranslations
    {
        CultureInfo CultureInfo { get; set; }
        int LCID { get; set; }
        IDictionary<string, string> Translations { get; set; }
    }
}