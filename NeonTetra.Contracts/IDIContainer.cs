using System;
using System.Collections.Generic;

namespace NeonTetra.Contracts
{
    public interface IRegister
    {
        void Register(Type serviceType, Type implementationType, string serviceName = null);

        void Register<I, T>(string name = null) where T : I;

        void RegisterInstance<T>(T instance, string serviceName = null);

        void RegisterSingleton<I, T>(string name = null) where T : I;

        void RegisterInstance(object instance, Type registrationType, string serviceName = null);

        void InjectRegistrationModule(Type module);

        void InjectRegistrationModule(string module);

        void LoadRegistrationModule(string module);

        void LoadRegistrationModule(Type module);

        void RegisterDelegate<T>(Type serviceType, Func<T> func, string serviceName = null);

    }

    public interface IUnregister
    {
        void Unregister(Type serviceType, string name = null);

        void Unregister<T>(string name = null);
    }

    public interface IResolve
    {
        T Resolve<T>(string serviceName = null);

        object Resolve(Type type, string serviceName = null);

        IEnumerable<T> ResolveAllInstances<T>();

        IEnumerable<object> ResolveAllInstances(Type type);

        bool IsRegistered<T>(string name = null);

        bool IsRegistered(Type type);

        Type GetTypeByModuleName(string module);

        Type GetTypeByAssemblyAndTypeNames(string assemblyName, string typeName);
    }


    public interface IDIContainer : IResolve, IRegister, IUnregister
    {
        object UnderlyingServiceContainer { get; }
    }
}