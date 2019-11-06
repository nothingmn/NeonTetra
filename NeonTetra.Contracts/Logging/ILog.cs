using System;
using System.Collections.Generic;

namespace NeonTetra.Contracts.Logging
{
    public interface ILog
    {
        void Debug(string messageTemplate);

        void Debug(Exception exception, string messageTemplate);

        void Debug(string messageTemplate, params object[] propertyValues);

        void Debug(Exception exception, string messageTemplate, params object[] propertyValues);

        void Debug<T>(string messageTemplate, T propertyValue);

        void Debug<T>(Exception exception, string messageTemplate, T propertyValue);

        void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2);

        void Error(string messageTemplate);

        void Error(Exception exception, string messageTemplate);

        void Error(string messageTemplate, params object[] propertyValues);

        void Error(Exception exception, string messageTemplate, params object[] propertyValues);

        void Error<T>(string messageTemplate, T propertyValue);

        void Error<T>(Exception exception, string messageTemplate, T propertyValue);

        void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2);

        void Fatal(string messageTemplate, params object[] propertyValues);

        void Fatal(Exception exception, string messageTemplate);

        void Fatal(Exception exception, string messageTemplate, params object[] propertyValues);

        void Fatal<T>(string messageTemplate, T propertyValue);

        void Fatal<T>(Exception exception, string messageTemplate, T propertyValue);

        void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2);

        void Information(string messageTemplate);

        void Information(string messageTemplate, params object[] propertyValues);

        void Information(Exception exception, string messageTemplate);

        void Information(Exception exception, string messageTemplate, params object[] propertyValues);

        void Information<T>(string messageTemplate, T propertyValue);

        void Information<T>(Exception exception, string messageTemplate, T propertyValue);

        void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2);

        void Event(string eventName, IDictionary<string, string> properties, IDictionary<string, double> metrics);

        void AddToContext(IDictionary<string, string> properties);
    }
}