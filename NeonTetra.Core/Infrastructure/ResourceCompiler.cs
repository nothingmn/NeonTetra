using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class ResourceCompiler : IResourceCompiler
    {
        private readonly IRegister _register;
        private readonly IResolve _resolver;

        public ResourceCompiler(IResolve resolver, IRegister register)
        {
            _resolver = resolver;
            _register = register;
        }

        public IList<CultureInfo> SupportedCultures { get; } = new List<CultureInfo>
        {
            new CultureInfo("fr-CA"),
            new CultureInfo("es-MX"),
            new CultureInfo("en-CA"),
            CultureInfo.InvariantCulture
        };

        public void Compile()
        {
            var items = new Dictionary<string, ISpecificCultureTranslations>();
            foreach (var culture in SupportedCultures)
            {
                var diKey = "culture_" + culture.LCID;
                var instance = _resolver.Resolve<ISpecificCultureTranslations>("DEFAULT");
                instance.CultureInfo = culture;
                instance.LCID = culture.LCID;
                instance.Translations = new Dictionary<string, string>();
                items.Add(diKey, instance);
                _register.RegisterInstance(instance, diKey);
            }

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                if (asm.FullName.Contains("NeonTetra"))
                    try
                    {
                        foreach (var resourceName in asm.GetManifestResourceNames())
                            try
                            {
                                var name = resourceName;
                                if (name.EndsWith(".resources", StringComparison.InvariantCultureIgnoreCase))
                                    name = name.Substring(0, name.Length - ".resources".Length);

                                var rm = new ResourceManager(name, asm);

                                foreach (var culture in SupportedCultures)
                                {
                                    var diKey = "culture_" + culture.LCID;
                                    var languageSpecificTranslation = items[diKey];
                                    LoadCacheFromResourceManager(rm, culture, languageSpecificTranslation);
                                }

                                rm.ReleaseAllResources();
                            }
                            catch (Exception /*e1*/)
                            {
                            }
                    }
                    catch (Exception /*e*/)
                    {
                    }
        }

        private void LoadCacheFromResourceManager(ResourceManager manager, CultureInfo culture,
            ISpecificCultureTranslations translations)
        {
            try
            {
                using (var resourceSet = manager.GetResourceSet(culture, true, false))
                {
                    var set = resourceSet?.OfType<DictionaryEntry>();

                    if (set != null)
                        foreach (var item in set)
                            try
                            {
                                translations.Translations.Add(item.Key.ToString(), item.Value.ToString());
                            }
                            catch (Exception)
                            {
                            }
                }
            }
            catch (Exception /*e*/)
            {
            }
        }
    }
}