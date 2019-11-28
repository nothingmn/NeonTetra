using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Diagnostics;
using MQTTnet.Protocol;
using MQTTnet.Server;
using NeonTetra.Contracts.Membership;
using System.Linq;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Services.MqttServer
{
    public class MqttBroker
    {
        private IAccountManager _accountManager;
        private IMqttServer _server;

        public async Task StartAsync(IAccountManager accountManager, int port = 1889)
        {
            var options = ConfigBroker(port);
            _accountManager = accountManager;
            _server = new MqttFactory().CreateMqttServer();
            await _server.StartAsync(options);
        }

        public async Task StopAsync()
        {
            await _server.StopAsync();
        }

        private IMqttServerOptions ConfigBroker(int port = 1889)
        {
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(port)
                .WithConnectionValidator(c =>
                {
                    c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                    var userAccount = (_accountManager.Login(c.Username, c.Password))?.Result;
                    if (userAccount != null)
                    {
                        c.ReasonCode = MqttConnectReasonCode.Success;
                    }
                });

            return optionsBuilder.Build();
        }
    }
}