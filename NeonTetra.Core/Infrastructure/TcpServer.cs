using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts.Configuration;
using NeonTetra.Contracts.Infrastructure;
using NeonTetra.Contracts.Logging;
using NeonTetra.Contracts.Serialization;

namespace NeonTetra.Core.Infrastructure
{
    public class TcpServer : ITcpServer
    {
        private static long Counter;
        private readonly ICaptureAndAggregate _captureAndAggregate;
        private readonly IConfiguration _configuration;
        private readonly ISerializer _serializer;

        private Func<byte[], CancellationToken, Task<byte[]>> _onReceived;
        private CancellationToken _waitToken;

        private readonly TimeSpan readTimeout = TimeSpan.FromSeconds(3);
        private TcpListener tcpListener;

        public TcpServer(IConfiguration configuration, ISerializer serializer, ICaptureAndAggregate captureAndAggregate)
        {
            _configuration = configuration;
            _serializer = serializer;
            _captureAndAggregate = captureAndAggregate;

            if (configuration.IsDev) readTimeout = TimeSpan.FromSeconds(60);
            _captureAndAggregate.Start(Capture);
        }

        public bool Started { get; set; }

        public IServerConnection ServerConnection { get; set; }
        public IPacketFormatter PacketFormatter { get; set; }
        public ILog Log { get; set; }

        public async Task Start(CancellationToken waitToken, Func<byte[], CancellationToken, Task<byte[]>> onReceived)
        {
            if (Started) return;

            Started = true;

            _waitToken = waitToken;
            _onReceived = onReceived;
            tcpListener = TcpListener.Create(ServerConnection.Port);
            //tcpListener.Server.LingerState = new LingerOption(true, 0);
            tcpListener.Start();

            await Task.Factory.StartNew(async t =>
            {
                while (!waitToken.IsCancellationRequested)
                    try
                    {
                        await Accept(ReceiveMessage);
                    }
                    catch (Exception)
                    {
                    }
            }, waitToken, TaskCreationOptions.LongRunning);
        }

        private double Capture()
        {
            return Counter;
        }

        private async Task<byte[]> ReceiveMessage(byte[] data)
        {
            Interlocked.Increment(ref Counter);
            if (_onReceived != null) return await _onReceived(data, _waitToken);
            return null;
        }

        private async Task Accept(Func<byte[], Task<byte[]>> callback)
        {
            //run this through Task.Run so we can respond to the waitToken if its called for.
            using (var client = await Task.Run(() => tcpListener.AcceptTcpClientAsync(), _waitToken))
            {
                if (_waitToken.IsCancellationRequested) return;
                client.Client.NoDelay = true;
                //client.Client.LingerState = new LingerOption(true, 0);
                var readBuffer = new byte[32768];

                using (var stream = client.GetStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        while (true)
                            using (var readTokenSource = new CancellationTokenSource(readTimeout))
                            {
                                readTokenSource.Token.Register(() =>
                                {
                                    if (memoryStream != null)
                                    {
                                        memoryStream.Close();
                                        memoryStream.Dispose();
                                    }

                                    if (stream != null)
                                    {
                                        stream.Close();
                                        stream.Dispose();
                                    }

                                    if (client != null)
                                    {
                                        //if (client.Client != null) client.Client.Disconnect(false);
                                        client.Close();
                                        client.Dispose();
                                    }

                                    readBuffer = null;
                                });

                                var read = await stream.ReadAsync(readBuffer, 0, readBuffer.Length,
                                    readTokenSource.Token);

                                if (read > 0)
                                {
                                    if (readBuffer[0] == 63)
                                    {
                                        var payload = _serializer.Serialize(new
                                        {
                                            _configuration?.DebugMode,
                                            _configuration?.Environment,
                                            _configuration?.Version,
                                            Owner = _onReceived?.GetMethodInfo()?.DeclaringType?.FullName,
                                            Counter,
                                            Environment.WorkingSet,
                                            Environment.MachineName,
                                            OnlineSince = _captureAndAggregate.StartDateTimeOffset,
                                            OnlineDurationInSeconds =
                                                (DateTimeOffset.UtcNow - _captureAndAggregate.StartDateTimeOffset)
                                                .TotalSeconds,
                                            TotalAverage = _captureAndAggregate.TotalAverage(),
                                            Last5SecondsAverage = _captureAndAggregate.Last5SecondsAverage()
                                        });
                                        await stream.WriteAsync(payload, 0, payload.Length, _waitToken);
                                        await stream.FlushAsync(_waitToken);
                                        break;
                                    }

                                    if (read > 0) await memoryStream.WriteAsync(readBuffer, 0, read, _waitToken);

                                    if (read <= 0 || readBuffer[read - 1] == PacketFormatter?.PacketTerminator)
                                    {
                                        var data = memoryStream.ToArray();
                                        if (PacketFormatter != null) data = PacketFormatter.RemoveFooter(data);

                                        //Client is done sending, process the packet
                                        var result = await callback(data);
                                        if (result != null && result.Any())
                                        {
                                            if (PacketFormatter != null) result = PacketFormatter.AddFooter(result);
                                            await stream.WriteAsync(result, 0, result.Length, _waitToken);
                                            await stream.FlushAsync(_waitToken);
                                        }

                                        result = null;
                                        data = null;
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                        memoryStream.Close();
                        memoryStream.Dispose();
                    }

                    stream.Close();
                    stream.Dispose();
                }

                readBuffer = null;
                client.Client.Disconnect(false);
                client.Close();
                client.Dispose();
            }
        }
    }
}