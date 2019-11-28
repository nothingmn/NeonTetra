using System.Collections.Generic;
using System.IO;

namespace NeonTetra.Contracts.Configuration
{
    public interface IConfiguration
    {
        string EntryPoint { get; }
        string Environment { get; set; }
        string HostEnvironment { get; set; }
        string Version { get; }
        bool IsFabricHosted { get; }
        bool DebugMode { get; set; }
        bool IsProd { get; }
        bool IsDev { get; }
        bool IsQA { get; }
        bool IsUnitTest { get; }
        bool HasScopedContainer { get; }
        DirectoryInfo RootDirectory { get; }
        DirectoryInfo AppDataDirectory { get; }
        bool IsReady { get; set; }

        T Bind<T>(string sectionName);

        void Build();

        T GetValueOrDefault<T>(string key, T defaultValue);

        T GetValue<T>(string key);

        IList<T> GetValues<T>(string key);
    }
}