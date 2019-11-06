using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class DomainDataLoader : IDomainDataLoader
    {
        public async Task<IDictionary<string, byte[]>> LoadManifestStreams(string assemblyFullFilePath, string assemblyName, bool isolate = true)
        {
            IDictionary<string, byte[]> items = new ConcurrentDictionary<string, byte[]>();

            var file = new FileInfo(assemblyFullFilePath);
            if (file.Exists)
            {
                var domain = AppDomain.CurrentDomain;
                if (isolate)
                {
                    domain = AppDomain.CreateDomain("loader");
                }

                domain.Load(File.ReadAllBytes(file.FullName));
                var asm = (from a in domain.GetAssemblies()
                           where a.FullName.IndexOf(assemblyName, StringComparison.InvariantCultureIgnoreCase) >= 0
                           select a)?.FirstOrDefault();

                IList<Task> tasks = new List<Task>();

                foreach (var name in asm.GetManifestResourceNames())
                    tasks.Add(
                        Task.Factory.StartNew(fn =>
                        {
                            try
                            {
                                var n = fn.ToString();

                                using (var res = asm.GetManifestResourceStream(n))
                                {
                                    using (var stm = new MemoryStream())
                                    {
                                        res.CopyTo(stm);
                                        if (stm.CanSeek && stm.Position > 0) stm.Seek(0, SeekOrigin.Begin);
                                        items.Add(n, stm.ToArray());
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }, name)
                    );
                await Task.WhenAll(tasks.ToArray());
                if (isolate)
                {
                    AppDomain.Unload(domain);
                }
            }

            return items;
        }
    }
}