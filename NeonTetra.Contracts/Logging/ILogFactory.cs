using System;
using System.Collections.Generic;

namespace NeonTetra.Contracts.Logging
{
    public interface ILogFactory
    {
        ILog DefaultLogger { get; }
        void SetDefaultLogger(ILog logger);

        ILog CreateLog(IDictionary<string, string> properties = null);

        ILog CreateLog(string application, string category = null, string name = null);

        ILog CreateLog(Type componentType);
    }
}