using System.Collections.Generic;
using System.Globalization;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class SpecificCultureTranslations : ISpecificCultureTranslations
    {
        public int LCID { get; set; }
        public IDictionary<string, string> Translations { get; set; }
        public CultureInfo CultureInfo { get; set; }
    }
}