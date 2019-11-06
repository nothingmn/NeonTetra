using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NeonTetra.Contracts.Infrastructure;

namespace NeonTetra.Core.Infrastructure
{
    public class TcpClient : ITcpClient
    {
        public IServerConnection ServerConnection { get; set; }
        public IPacketFormatter PacketFormatter { get; set; }

        public async Task<byte[]> SendMessage(byte[] message,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Yield();
            var result = default(byte[]);

            try
            {
                //query the server
                using (var client = new System.Net.Sockets.TcpClient(AddressFamily.InterNetwork))
                {
                    client.Client.NoDelay = true;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await client.ConnectAsync(ServerConnection.Host, ServerConnection.Port);
                        if (!cancellationToken.IsCancellationRequested)
                            using (var stream = client.GetStream())
                            {
                                if (PacketFormatter != null) message = PacketFormatter.AddFooter(message);

                                if (!cancellationToken.IsCancellationRequested)
                                {
                                    await stream.WriteAsync(message, 0, message.Length);
                                    await stream.FlushAsync();

                                    if (!cancellationToken.IsCancellationRequested)
                                        using (var memoryStream = new MemoryStream())
                                        {
                                            var readBuffer = new byte[32768];
                                            while (true)
                                            {
                                                var read = await stream.ReadAsync(readBuffer, 0, readBuffer.Length);
                                                if (read > 0) await memoryStream.WriteAsync(readBuffer, 0, read);
                                                if (read <= 0 || readBuffer[read - 1] == 4)
                                                {
                                                    var data = memoryStream.ToArray();
                                                    if (PacketFormatter != null)
                                                        result = PacketFormatter.RemoveFooter(data);
                                                    else
                                                        result = data;
                                                    break;
                                                }
                                            }

                                            memoryStream.Close();
                                            memoryStream.Dispose();
                                        }

                                    stream.Close();
                                    stream.Dispose();
                                }
                            }

                        client.Close();
                        client.Dispose();
                    }
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}