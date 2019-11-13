using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using NeonTetra.Contracts;
using Microsoft.Extensions.Configuration;
using IConfiguration = NeonTetra.Contracts.Configuration.IConfiguration;

namespace NeonTetra.Core.Configuration
{
    public delegate void ConfigurationBuild(IConfiguration configuration);

    public class Configuration : IConfiguration
    {
        public const string AdditionalSettingsPath = @".\Settings";

        public static readonly TimeZoneInfo ReferenceDbTimeZoneInfo =
            TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

        public event ConfigurationBuild OnConfigurationBuild;

        private readonly IDIContainer _container;
        public IConfigurationRoot ConfigurationRoot;

        public Configuration(IDIContainer container)
        {
            _container = container;
        }

        private string GetConfigFile(string environmentHostOrMachineName, string hostingEnvironment = null, bool cloudServices = false, bool isEnvironmentName = true)
        {
            var configRoot = @"Config";

            var location = hostingEnvironment;
            if (!string.IsNullOrEmpty(location) && location.Contains(".") && hostingEnvironment.Contains(location))
                location = hostingEnvironment.Substring(0, location.IndexOf("."));

            if (string.IsNullOrEmpty(environmentHostOrMachineName))
            {
                //5. /Config/appsettings.json
                var path5 = Path.Combine(configRoot, "appsettings.json");
#if TRACE
                Trace.WriteLine(EntryPoint + ": " + path5);
#endif
                return path5;
            }

            if (isEnvironmentName)
            {
                var environmentRoot = Path.Combine(configRoot, environmentHostOrMachineName);

                if (string.IsNullOrEmpty(hostingEnvironment))
                {
                    if (!cloudServices)
                    {
                        //6. /Config/%ENVIRONMENT%/appsettings.%ENVIRONMENT%.json
                        var path6 = Path.Combine(environmentRoot,
                            "appsettings." + environmentHostOrMachineName + ".json");
#if TRACE
                        Trace.WriteLine(EntryPoint + ": " + path6);
#endif
                        return path6;
                    }

                    //7. /Config/%ENVIRONMENT%/appsettings.CloudServices.%ENVIRONMENT%.json
                    var path7 = Path.Combine(environmentRoot,
                        "appsettings.CloudServices." + environmentHostOrMachineName + ".json");
#if TRACE
                    Trace.WriteLine(EntryPoint + ": " + path7);
#endif
                    return path7;
                }

                var hostingPath = Path.Combine(environmentRoot, location);

                if (!cloudServices)
                {
                    //8. /Config/%ENVIRONMENT%/%HOSTINGENVIRONMENT%/appsettings.%HOSTINGENVIRONMENT%.json
                    var path8 = Path.Combine(hostingPath, "appsettings." + hostingEnvironment + ".json");
#if TRACE
                    Trace.WriteLine(EntryPoint + ": " + path8);
#endif
                    return path8;
                }

                //9. /Config/%ENVIRONMENT%/%HOSTINGENVIRONMENT%/appsettings.CloudServices.%HOSTINGENVIRONMENT%.json
                var path9 = Path.Combine(hostingPath, "appsettings.CloudServices." + hostingEnvironment + ".json");
#if TRACE
                Trace.WriteLine(EntryPoint + ": " + path9);
#endif
                return path9;
            }

            //10. /Config/appsettings.%HOST%.json
            //11. /Config/appsettings.machine.%MACHINENAME%.json
            var path = Path.Combine(configRoot, "appsettings." + environmentHostOrMachineName + ".json");
#if TRACE
            Trace.WriteLine(EntryPoint + ": " + path);
#endif
            return path;
        }

        /// <summary>
        ///     Build environmentHostOrMachineName specific configuration
        ///     Defaults to Development
        /// </summary>
        /// <param name="environment"></param>
        public void Build()
        {
            SetupEntryPoint();

            var root = DetermineCodeBaseRoot();
#if TRACE
            Trace.WriteLine(EntryPoint + ": " + root);
#endif

            //stage 1
            //setup the basic HostingEnviornment/Environment variables from the core/root locations
#if TRACE
            Trace.WriteLine(EntryPoint + ": " + "---------Stage 1 Started---------");
#endif
            InitializeEnvironment(root);

#if TRACE
            Trace.WriteLine(EntryPoint + ": " + "---------Stage 1 Completed---------");
#endif

            //stage 2
            //setup the full copnfiguration environmentHostOrMachineName based on HostingEnviornment/Environment variables which were setup in stage 1.
#if TRACE
            Trace.WriteLine(EntryPoint + ": " + "---------Stage 2 Started---------");
#endif
            BuildConfiguration(root);
            OnConfigurationBuild?.Invoke(this);
#if TRACE
            Trace.WriteLine(EntryPoint + ": " + "---------Stage 2 Completed---------");
#endif
        }

        private string GetConfigForHost(string hostName, string environment = null, string hostingEnvironment = null)
        {
            var configRoot = @"Config";

            var location = hostingEnvironment;
            if (!string.IsNullOrEmpty(location) && location.Contains(".") && hostingEnvironment.Contains(location))
                location = hostingEnvironment.Substring(0, location.IndexOf("."));

            if (string.IsNullOrEmpty(environment))
            {
                // /Config/appsettings.%HOST%.json
                var path = Path.Combine(configRoot, "appsettings." + hostName + ".json");

#if TRACE
                Trace.WriteLine(EntryPoint + ": " + path);
#endif
                return path;
            }
            else
            {
                var path = Path.Combine(Path.Combine(configRoot, environment));

                if (string.IsNullOrEmpty(hostingEnvironment))
                {
                    // /Config/%ENVIRONMENT%/appsettings.%HOST%.json
                    path = Path.Combine(path, "appsettings." + hostName + ".json");

#if TRACE
                    Trace.WriteLine(EntryPoint + ": " + path);
#endif
                    return path;
                }

                path = Path.Combine(Path.Combine(path, location));

                // /Config/%ENVIRONMENT%/%HOSTINGENVRIONMENET%/appsettings.%HOST%.json
                path = Path.Combine(path, "appsettings." + hostName + ".json");

#if TRACE
                Trace.WriteLine(EntryPoint + ": " + path);
#endif
                return path;
            }
        }

        private void BuildConfiguration(string root)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(root);
#if TRACE
            Trace.WriteLine(EntryPoint + ": " + "SetBasePath:" + root);
#endif
            //stage 2.4
            builder.AddEnvironmentVariables();

            builder.AddJsonFile(System.IO.Path.Combine(root, "appsettings.json"), true, false); //appsettings.json

            //stage 2.5
            builder.AddJsonFile(GetConfigFile(null), true, false); //appsettings.json

            //stage 2.6
            builder.AddJsonFile(GetConfigFile(Environment), true, false); //appsettings.%ENVIRONMENT%.json

            //stage 2.7
            builder.AddJsonFile(GetConfigFile(Environment, cloudServices: true), true,
                false); //appsettings.CloudServices.%ENVIRONMENT%.json

            //stage 2.8
            builder.AddJsonFile(GetConfigFile(Environment, HostEnvironment), true,
                false); //appsettings.%HOSTINGENVIRONMENT%.json

            //stage 2.9
            builder.AddJsonFile(GetConfigFile(Environment, HostEnvironment, true), true,
                false); //appsettings.CloudServices.%HOSTINGENVIRONMENT%.json

            //stage 2.10
            var host = System.Environment.GetEnvironmentVariable("Host");
            if (!string.IsNullOrEmpty(host))
            {
                builder.AddJsonFile(GetConfigForHost(host), true, false); //appsettings.%HOST%.json
                builder.AddJsonFile(GetConfigForHost(host, Environment), true, false); //appsettings.%HOST%.json
                builder.AddJsonFile(GetConfigForHost(host, Environment, HostEnvironment), true,
                    false); //appsettings.%HOST%.json
            }

            //stage 2.11
            builder.AddJsonFile(GetConfigFile("machine." + System.Environment.MachineName, isEnvironmentName: false),
                true, false); //appsettings.machine.%MACHINENAME%.json

            SetupVersion();
            SetupUnitTest();
            SetupAppDataDirectory();
            InitializeUnitTestAppSettings(builder);
            InitializeAdditionalSettings(builder, root);

            ConfigurationRoot = builder.Build();

#if TRACE
            BuildAndDumpConfig(ConfigurationRoot);
#endif

            _container.RegisterInstance(ConfigurationRoot, typeof(IConfigurationRoot));

            IsReady = true;
        }

        private void BuildAndDumpConfig(IConfigurationRoot root)
        {
            Trace.WriteLine(EntryPoint + ": " + "----------Configuration Build Result---------");
            var flattenDictionary = new SortedDictionary<string, string>();
            DumpConfigSection(flattenDictionary, root.GetChildren());
            foreach (var s in flattenDictionary)
                if (!string.IsNullOrEmpty(s.Value))
                    Trace.WriteLine(EntryPoint + ": " + s.Key + "=" + s.Value);
            Trace.WriteLine(EntryPoint + ": " + "/----------Configuration Build Result---------");
        }

        private void DumpConfigSection(SortedDictionary<string, string> flattenDictionary,
            IEnumerable<IConfigurationSection> section)
        {
            foreach (var s in section)
            {
                var key = s.Path + ": " + s.Key;
                var value = s.Value;
                if (!string.IsNullOrEmpty(key) && !flattenDictionary.ContainsKey(key) && !string.IsNullOrEmpty(value))
                {
                    if (s.Path == s.Key) key = s.Key;
                    flattenDictionary.Add(key, value);
                }

                var kids = s.GetChildren();
                if (kids != null) DumpConfigSection(flattenDictionary, kids);
            }
        }

        private void InitializeUnitTestAppSettings(ConfigurationBuilder builder)
        {
            if (IsUnitTest)
            {
                var unitTestJson = "appsettings.UnitTest.json";
                builder.AddJsonFile(unitTestJson, true, true);
            }
        }

        private void InitializeAdditionalSettings(ConfigurationBuilder builder, string root)
        {
            var additionalSettingsPath = Path.Combine(root, AdditionalSettingsPath);
            if (!Directory.Exists(additionalSettingsPath)) return;

            foreach (var path in Directory.EnumerateFiles(additionalSettingsPath, "*.json", SearchOption.AllDirectories)
            ) builder.AddJsonFile(path, true, true);
        }

        private void InitializeEnvironment(string root)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(root);
            //state 1.1
            builder.AddEnvironmentVariables();

            //stage 1.2
            builder.AddJsonFile(GetConfigFile(Environment), true, true); //appsettings.json
            //stage 1.2 -- Second part, is to also add the root appsettings.json from the individual project
            builder.AddJsonFile("appsettings.json", true, true); //appsettings.json

            var tempConfig = builder.Build();

            //stage 1.3, default to OnPrem.DEV
            HostEnvironment = tempConfig["HostEnvironment"];
            if (string.IsNullOrEmpty(HostEnvironment)) HostEnvironment = "OnPrem.DEV";

            Environment = HostEnvironment
                .Substring(HostEnvironment.IndexOf(".", StringComparison.InvariantCultureIgnoreCase) + 1)
                .ToUpperInvariant();

            if (HostEnvironment.StartsWith("Cloud", StringComparison.InvariantCultureIgnoreCase))
                HostEnvironment = "Azure." + Environment;

            //if this throws, you probably have the HostEnvironment setup incorrectly.
            if (string.IsNullOrEmpty(Environment)) throw new NullReferenceException("Environment");

#if TRACE
            Trace.WriteLine(EntryPoint + " HostEnvironment:" + HostEnvironment);
            Trace.WriteLine(EntryPoint + " Environment:" + Environment);
#endif
        }

        private string DetermineCodeBaseRoot()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new Uri(codeBase, UriKind.RelativeOrAbsolute);
            var dllInRoot = new FileInfo(uri.LocalPath);
            var root = dllInRoot.DirectoryName;
            if (string.IsNullOrEmpty(root)) throw new Exception("Could not determine root directory");
            _container.RegisterInstance(root, typeof(string), "root_directory");
            RootDirectory = new DirectoryInfo(root);
            _container.RegisterInstance(RootDirectory, typeof(DirectoryInfo), "root_path");

#if TRACE
            Trace.WriteLine(EntryPoint + ":  Configuration base path set to:" + root);
#endif

            return root;
        }

        private void SetupAppDataDirectory()
        {
            if (IsUnitTest)
            {
                //unit testing
                AppDataDirectory = new DirectoryInfo(Path.Combine(RootDirectory.Parent.Parent.FullName, "Test_Data"));
            }
            else
            {
                if (RootDirectory.FullName.ToLowerInvariant().Contains(@"SfDevCluster".ToLowerInvariant()))
                {
                    //development cluster, local machines
                    var devClusterRoot = RootDirectory.FullName.Substring(0,
                        RootDirectory.FullName.IndexOf("SfDevCluster", StringComparison.InvariantCultureIgnoreCase) +
                        "SfDevCluster".Length);
                    AppDataDirectory = new DirectoryInfo(Path.Combine(devClusterRoot, "App_Data"));
                    if (!Directory.Exists(AppDataDirectory.FullName))
                        Directory.CreateDirectory(AppDataDirectory.FullName);
                }
                else
                {
                    //non-development cluster, prod, qa, uat...
                    //temp fix to move the app data folder to a drive with available space
                    //TODO: ROBC DEREKB
                    if (HostEnvironment.Contains("Azure"))
                        AppDataDirectory = new DirectoryInfo(@"c:\App_Data");
                    else
                        AppDataDirectory = new DirectoryInfo(Path.Combine(RootDirectory.Parent.FullName, "App_Data"));
                    if (!Directory.Exists(AppDataDirectory.FullName))
                        Directory.CreateDirectory(AppDataDirectory.FullName);
                }
            }

            _container.RegisterInstance(AppDataDirectory, typeof(DirectoryInfo), "App_Data");
#if TRACE
            Trace.WriteLine(EntryPoint + ": App Data path is:" + AppDataDirectory);
#endif
        }

        public bool IsFabricHosted { get; private set; }

        private void SetupUnitTest()
        {
            var processName = Process.GetCurrentProcess().ProcessName;

            IsUnitTest = processName == "VSTestHost"
                         || processName.StartsWith(
                             "vstest.executionengine") //it can be vstest.executionengine.x86 or vstest.executionengine.x86.clr20
                         || processName.StartsWith("testhost.x86")
                         || processName.Contains("QualityTools.UnitTestFramework") //ms test
                         || processName.StartsWith("QTAgent") //QTAgent32 or QTAgent32_35
                         || processName.StartsWith("JetBrains.ReSharper.TaskRunner") //jetbrains
                         || processName.StartsWith("nunit") //nunit3-console
                         || processName.StartsWith("xunit") //xunit-console
                ;

            var sfAppName = System.Environment.GetEnvironmentVariable("Fabric_ApplicationName");
            IsFabricHosted = !string.IsNullOrEmpty(sfAppName);

#if TRACE
            Trace.WriteLine(EntryPoint + ": IsUnitTest:" + IsUnitTest);
            Trace.WriteLine(EntryPoint + ": IsFabricHosted:" + IsFabricHosted);
            Trace.WriteLine(EntryPoint + ": Service Fabric AppName:" + sfAppName);
#endif
        }

        private void SetupVersion()
        {
            var versionProvider = _container.Resolve<IVersionProvider>();
            Version = versionProvider.GetVersion();
#if TRACE
            Trace.WriteLine(EntryPoint + ": " + Version);
#endif
        }

        private void SetupEntryPoint()
        {
            var cmd = System.Environment.CommandLine;
            EntryPoint = cmd;

            if (!string.IsNullOrEmpty(cmd))
                try
                {
                    if (cmd.StartsWith("\"")) cmd = cmd.Substring(1);
                    if (cmd.EndsWith("\"")) cmd = cmd[0..^1];
                    if (File.Exists(cmd))
                    {
                        var file = new FileInfo(cmd);
                        EntryPoint = file.Name;
                    }
                }
                catch (Exception)
                {
                    //swallow, defaults to cmd above.
                }

#if TRACE
            Trace.WriteLine("Entry Point:" + EntryPoint);
#endif
        }

        public T GetValueOrDefault<T>(string key, T defaultValue)
        {
            var input = ConfigurationRoot[key];
            if (input != null)
                return ConvertValue<T>(input);

            return defaultValue;
        }

        /// <summary>
        ///     Binds to the root config, will return any top level properties that match
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Bind<T>()
        {
            var o = _container.Resolve<T>();
            if (o == null) return default;

            ConfigurationRoot.Bind(o, Activate);
            return o;
        }

        public string EntryPoint { get; private set; }

        /// <summary>
        ///     Bind to a sub section
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public T Bind<T>(string sectionName)
        {
            var o = _container.Resolve<T>();
            if (o == null) return default;

            ConfigurationRoot.GetSection(sectionName).Bind(o, Activate);
            return o;
        }

        /// <summary>
        ///     Bind to a section, for an instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public T Bind<T>(string sectionName, T instance)
        {
            ConfigurationRoot.GetSection(sectionName).Bind(instance, Activate);
            return instance;
        }

        /// <summary>
        ///     Bind the root, to a given instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public T Bind<T>(T instance)
        {
            ConfigurationRoot.Bind(instance, Activate);
            return instance;
        }

        public T GetValue<T>(string key)
        {
            var input = ConfigurationRoot[key];
            if (!string.IsNullOrEmpty(input)) return ConvertValue<T>(input);
            return default;
        }

        private object Activate(Type type)
        {
            if (_container.IsRegistered(type) || type.GenericTypeArguments.Length > 0 &&
                type.GenericTypeArguments.All(t => _container.IsRegistered(t))) return _container.Resolve(type);

            if (type.IsArray) return Array.CreateInstance(type.GetElementType(), 0);

            return Activator.CreateInstance(type);
        }

        private T ConvertValue<T>(string value, string path = null)
        {
            if (typeof(T).IsInterface)
            {
                var o = _container.Resolve<T>();
                foreach (var p in typeof(T).GetProperties())
                    if (p.CanWrite)
                    {
                        var getValueGeneric = GetType().GetMethod("GetValue").MakeGenericMethod(p.PropertyType);
                        var propertyValue = getValueGeneric.Invoke(this, new[] { string.Concat(path, ": ", p.Name) });
                        p.SetValue(o, propertyValue);
                    }

                return o;
            }

            if (value != null)
            {
                var t = typeof(T);

                t = Nullable.GetUnderlyingType(t) ?? t;

                if (typeof(T).IsEnum) return (T)Enum.Parse(t, value, true);

                if (t == typeof(Guid)) return (T)Convert.ChangeType(new Guid(value), typeof(Guid));

                if (t == typeof(DateTime) || t == typeof(DateTimeOffset))
                {
                    var dt = new DateTime();
                    if (DateTime.TryParse(value, out dt))
                    {
                        if (t == typeof(DateTimeOffset))
                        {
                            var dto = new DateTimeOffset(dt);
                            return (T)(object)dto;
                        }

                        return (T)(object)dt;
                    }
                }
                else
                {
                    return (T)Convert.ChangeType(value, t);
                }
            }

            return default;
        }

        public IList<T> GetValues<T>(string key)
        {
            var list = new List<T>();
            var section = ConfigurationRoot.GetSection(key);
            if (section == null) return default;

            foreach (var s in section.GetChildren()) list.Add(ConvertValue<T>(s?.Value, s.Path));
            return list;
        }

        public string HostEnvironment { get; set; }

        public string Environment { get; set; }

        public string Version { get; private set; }

        public bool IsProd => Environment.Equals("PROD", StringComparison.InvariantCultureIgnoreCase);
        public bool IsDev => Environment.Equals("Dev", StringComparison.InvariantCultureIgnoreCase);
        public bool IsQA => Environment.Equals("QA", StringComparison.InvariantCultureIgnoreCase);

        public bool IsUnitTest { get; private set; }

        public bool HasScopedContainer => GetValueOrDefault("FeatureFlags:EnableRequestScopeMiddleware", true);

        public DirectoryInfo RootDirectory { get; private set; }

        public DirectoryInfo AppDataDirectory { get; private set; }

        public bool IsReady { get; set; }

#if DEBUG
        public bool DebugMode { get; set; } = true;

#else
        public bool DebugMode { get; set; } = false;
#endif
    }
}