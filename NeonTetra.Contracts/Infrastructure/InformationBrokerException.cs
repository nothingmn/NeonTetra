using System;

namespace NeonTetra.Contracts.Infrastructure
{
    public class InformationBrokerException : ApplicationException
    {
        public InformationBrokerException(Exception innerException, string messageTemplate,
            params object[] messageParameters)
            : base(string.Format(messageTemplate, messageParameters), innerException)
        {
            MessageTemplate = messageTemplate;
            MessageParameters = messageParameters;
        }

        public InformationBrokerException(string messageTemplate, params object[] messageParameters)
            : this(null, messageTemplate, messageParameters)
        {
        }

        public string MessageTemplate { get; }

        public object[] MessageParameters { get; }
    }
}