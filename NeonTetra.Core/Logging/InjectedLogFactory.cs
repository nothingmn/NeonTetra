using System;
using System.Collections.Generic;
using NeonTetra.Contracts;
using NeonTetra.Contracts.Logging;

namespace NeonTetra.Core.Logging
{
    public class InjectedLogFactory : ILogFactory
    {
        private readonly IRegister _register;
        private readonly IResolve _resolver;

        public InjectedLogFactory(IResolve resolver, IRegister register)
        {
            _resolver = resolver;
            _register = register;
        }

        public ILog DefaultLogger
        {
            get
            {
                if (_resolver.IsRegistered<ILog>("Log")) return _resolver.Resolve<ILog>("Log");
                var log = CreateLog();
                SetDefaultLogger(log);
                return log;
            }
        }

        public void SetDefaultLogger(ILog logger)
        {
            if (logger != null) _register.RegisterInstance(logger, "Log");
        }

        public ILog CreateLog(IDictionary<string, string> properties = null)
        {
            var log = _resolver.ResolveAllInstances<ILog>();
            var bLogger = new BroadcastLogger(log, _resolver);
            if (_resolver.IsRegistered<Dictionary<string, string>>("LoggingContext"))
            {
                var defaultProperties = _resolver.Resolve<Dictionary<string, string>>("LoggingContext");
                bLogger.AddToContext(defaultProperties);
            }

            bLogger.AddToContext(properties);
            return bLogger;
        }

        public ILog CreateLog(string application, string category = null, string name = null)
        {
            ILog log = null;
            if (!string.IsNullOrEmpty(name) && _resolver.IsRegistered<ILog>(name)) log = _resolver.Resolve<ILog>(name);
            if (log == null && _resolver.IsRegistered<ILog>(application + "_" + category))
                log = _resolver.Resolve<ILog>(application + "_" + category);
            if (log == null)
            {
                var properties = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(application)) properties.Add("Application", application);
                if (!string.IsNullOrEmpty(category)) properties.Add("Category", category);
                if (!string.IsNullOrEmpty(name)) properties.Add("Name", name);
                log = CreateLog(properties);
                if (!string.IsNullOrEmpty(name))
                    _register.RegisterInstance(log, typeof(ILog), name);
                else
                    _register.RegisterInstance(log, typeof(ILog), application + "_" + category);
            }

            return log;
        }

        public ILog CreateLog(Type componentType)
        {
            var componentName = componentType.FullName;
            var logName = "Log_" + componentName;

            if (_resolver.IsRegistered<ILog>(logName)) return _resolver.Resolve<ILog>(logName);

            var properties = new Dictionary<string, string>
            {
                {"Component", componentName}
            };

            var log = CreateLog(properties);
            _register.RegisterInstance(log, logName);
            return log;
        }
    }
}