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
        private IHashProvider _userHashProvider;
        private IMqttServer _server;
        private IList<IUser> _users;

        public async Task StartAsync(IHashProvider userHashProvider, int port = 1889, IList<IUser> users = null)
        {
            _users = users;
            _userHashProvider = userHashProvider;
            var options = ConfigBroker(port);

            _server = new MqttFactory().CreateMqttServer();
            await _server.StartAsync(options);
        }

        public async Task StopAsync()
        {
            await _server.StopAsync();
        }

        private bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return false;
            var user = (from u in _users where u.UserName.Equals(username) select u)?.FirstOrDefault();
            if (user == null) return false;
            if (string.IsNullOrEmpty(user.Password)) return false;
            var hashedPassword = System.Convert.ToBase64String(_userHashProvider.Hash(System.Text.Encoding.UTF8.GetBytes(password)));
            if (hashedPassword.Equals(user.Password)) return true;
            return false;
        }

        private IMqttServerOptions ConfigBroker(int port = 1889)
        {
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(port)
                .WithConnectionValidator(c =>
                {
                    if (!ValidateUser(c.Username, c.Password))
                    {
                        c.ReturnCode = MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword;
                    }
                    else
                    {
                        c.ReturnCode = MqttConnectReturnCode.ConnectionAccepted;
                    }
                });

            return optionsBuilder.Build();
        }
    }
}