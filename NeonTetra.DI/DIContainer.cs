using DryIoc;
using NeonTetra.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NeonTetra.DI.Containers;

namespace NeonTetra.DI
{
    public class DIContainer : IDIContainer
    {
        private static List<Assembly> Assemblies;
        private IContainer _internalDryContainer;

        public DIContainer(IContainer internalContainer = null)
        {
            Initialize(internalContainer);
        }

        public object UnderlyingServiceContainer { get; private set; }

        [DebuggerStepThrough]
        public void RegisterInstance(object instance, Type registrationType, string serviceName = null)
        {
            if (string.IsNullOrEmpty(serviceName))
                _internalDryContainer.RegisterInstance(
                    registrationType,
                    instance,
                    ifAlreadyRegistered: IfAlreadyRegistered.Replace);
            else
                _internalDryContainer.RegisterInstance(
                    registrationType,
                    instance,
                    serviceKey: serviceName,
                    ifAlreadyRegistered: IfAlreadyRegistered.Replace);
        }

        [DebuggerStepThrough]
        public T Resolve<T>(string serviceName = null)
        {
            if (string.IsNullOrEmpty(serviceName)) return _internalDryContainer.Resolve<T>();
            return _internalDryContainer.Resolve<T>(serviceName);
        }

        [DebuggerStepThrough]
        public IEnumerable<T> ResolveAllInstances<T>()
        {
            var serviceTypeEnumerable = typeof(IEnumerable<>).MakeGenericType(typeof(T));

            var obj = _internalDryContainer.Resolve(serviceTypeEnumerable, IfUnresolved.ReturnDefault);
            return (IEnumerable<T>)obj;
        }

        [DebuggerStepThrough]
        public void Register(Type serviceType, Type implementationType, string serviceName = null)
        {
            if (string.IsNullOrEmpty(serviceName))
                _internalDryContainer.Register(serviceType, implementationType, Reuse.Transient,
                    Made.Of(FactoryMethod.ConstructorWithResolvableArguments),
                    ifAlreadyRegistered: IfAlreadyRegistered.Replace);
            else
                _internalDryContainer.Register(serviceType, implementationType, Reuse.Transient,
                    Made.Of(FactoryMethod.ConstructorWithResolvableArguments), serviceKey: serviceName,
                    ifAlreadyRegistered: IfAlreadyRegistered.Replace);
        }

        [DebuggerStepThrough]
        public void RegisterDelegate<T>(Type serviceType, Func<T> func, string serviceName = null)
        {
            if (string.IsNullOrEmpty(serviceName))
                _internalDryContainer.RegisterDelegate(serviceType, _ => func, Reuse.Transient, Setup.Default,
                    IfAlreadyRegistered.Replace);
            else
                _internalDryContainer.RegisterDelegate(serviceType, _ => func, Reuse.Transient, Setup.Default,
                    IfAlreadyRegistered.Replace, serviceName);
        }

        [DebuggerStepThrough]
        public void RegisterInstance<T>(T instance, string serviceName = null)
        {
            if (string.IsNullOrEmpty(serviceName))
                _internalDryContainer.UseInstance(instance, ifAlreadyRegistered: IfAlreadyRegistered.Replace);
            else
                _internalDryContainer.UseInstance(instance, serviceKey: serviceName,
                    ifAlreadyRegistered: IfAlreadyRegistered.Replace);
        }

        [DebuggerStepThrough]
        public void Register<I, T>(string name = null) where T : I
        {
            if (string.IsNullOrEmpty(name))
                _internalDryContainer.Register<I, T>(Reuse.Transient,
                    Made.Of(FactoryMethod.ConstructorWithResolvableArguments),
                    ifAlreadyRegistered: IfAlreadyRegistered.Replace);
            else
                _internalDryContainer.Register<I, T>(Reuse.Transient,
                    Made.Of(FactoryMethod.ConstructorWithResolvableArguments), serviceKey: name,
                    ifAlreadyRegistered: IfAlreadyRegistered.Replace);
        }

        [DebuggerStepThrough]
        public void RegisterSingleton<I, T>(string name = null) where T : I
        {
            if (string.IsNullOrEmpty(name))
                _internalDryContainer.Register<I, T>(Reuse.Singleton,
                    Made.Of(FactoryMethod.ConstructorWithResolvableArguments),
                    ifAlreadyRegistered: IfAlreadyRegistered.Replace);
            else
                _internalDryContainer.Register<I, T>(Reuse.Singleton,
                    Made.Of(FactoryMethod.ConstructorWithResolvableArguments), serviceKey: name,
                    ifAlreadyRegistered: IfAlreadyRegistered.Replace);
        }

        [DebuggerStepThrough]
        public object Resolve(Type type, string serviceName = null)
        {
            if (string.IsNullOrEmpty(serviceName))
                return _internalDryContainer.Resolve(type, IfUnresolved.ReturnDefault);
            return _internalDryContainer.Resolve(type, serviceName, IfUnresolved.ReturnDefault);
        }

        public bool IsRegistered<T>(string name = null)
        {
            return _internalDryContainer.IsRegistered<T>(name);
        }

        public bool IsRegistered(Type type)
        {
            return _internalDryContainer.IsRegistered(type);
        }

        public void Unregister(Type serviceType, string name = null)
        {
            _internalDryContainer.Unregister(serviceType, name);
        }

        public void Unregister<T>(string name = null)
        {
            _internalDryContainer.Unregister<T>(name);
        }

        public Type GetTypeByModuleName(string module)
        {
            Type type = null;

            if (!string.IsNullOrEmpty(module) && module.ToLowerInvariant() != "unknown")
            {
                string assemblyName = null;
                string typeName;
                if (module.Contains(","))
                {
                    assemblyName = module.Substring(module.IndexOf(",") + 1).Trim();
                    typeName = module.Substring(0, module.IndexOf(","));
                }
                else
                {
                    typeName = module;
                }

                type = GetTypeByAssemblyAndTypeNames(assemblyName, typeName);
            }

            return type;
        }

        public Type GetTypeByAssemblyAndTypeNames(string assemblyName, string typeName)
        {
            Type type = null;

            if (!string.IsNullOrEmpty(assemblyName))
            {
                var assembly = (from a in Assemblies where a.FullName.StartsWith(assemblyName + ", ") select a)
                    .FirstOrDefault();

                if (assembly == null) assembly = Assembly.Load(assemblyName);

                if (assembly != null)
                {
                    type = (from t in assembly.ExportedTypes where t.FullName == typeName select t)
                        .FirstOrDefault();
                    if (type == null && !string.IsNullOrEmpty(typeName)) type = assembly.GetType(typeName);
                }
            }
            else if (!string.IsNullOrEmpty(typeName))
            {
                type = Type.GetType(typeName, false);
            }

            return type;
        }

        public void LoadRegistrationModule(string module)
        {
            var type = GetTypeByModuleName(module);
            if (type != null) LoadRegistrationModule(type);
        }

        public void LoadRegistrationModule(Type module)
        {
            //override with a config specified one
            var tmp = Activator.CreateInstance(module) as IRegistrationContainer;
            tmp?.Register(this);
        }

        public void InjectRegistrationModule(Type module)
        {
            LoadRegistrationModule(module);
        }

        public void InjectRegistrationModule(string moduleName)
        {
            LoadRegistrationModule(moduleName);
        }

        public IEnumerable<object> ResolveAllInstances(Type type)
        {
            return _internalDryContainer.ResolveMany(type);
        }

        private void Initialize(IContainer internalContainer)
        {
            //var verify = false;
            if (internalContainer == null)
            {
                internalContainer = new Container(Configure);
                //verify = true;
            }

            if (Assemblies == null) Assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            _internalDryContainer = internalContainer;
            UnderlyingServiceContainer = _internalDryContainer;

            //if (verify) _internalDryContainer.Validate(registration => !registration.ServiceType.IsOpenGeneric());

            new CoreServicesRegistrationContainer().Register(this);
            new ConfigurationRegistrationContainer().Register(this);
            new LoggingRegistrationContainer().Register(this);
        }

        private Rules Configure(Rules rules)
        {
            rules = rules // Searches for public constructor with most resolvable parameters or throws
                    .With(FactoryMethod.ConstructorWithResolvableArguments)
                    // Don't throw exception while registering IDisposable
                    .WithoutThrowOnRegisteringDisposableTransient()
                    .WithoutThrowIfDependencyHasShorterReuseLifespan()
                    .WithoutImplicitCheckForReuseMatchingScope()
                    .WithoutThrowOnRegisteringDisposableTransient()
                    .WithTrackingDisposableTransients()
                    .WithFactorySelector(Rules.SelectLastRegisteredFactory())
                    .WithoutEagerCachingSingletonForFasterAccess()
                ;
            return rules;
        }
    }
}