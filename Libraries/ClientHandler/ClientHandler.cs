using System.Net.Sockets;
using TunnelServer.Libraries.Setting;

namespace TunnelServer.Libraries.ClientHandler
{
    public class ClientHandler
    {
        private readonly TcpClient _client;
        private readonly SettingLoader _setting;
        private readonly Logger.Logger _logger;

        public ClientHandler(TcpClient client, SettingLoader setting, Logger.Logger logger)
        {
            _client = client;
            _setting = setting;
            _logger = logger;
        }

        public async Task HandleAsync()
        {
            using (_client)
            {
                try
                {
                    using TcpClient remoteClient = new();
                    await remoteClient.ConnectAsync(_setting.Server!.Ip!, _setting.Server!.Port);
                    _logger.Success($"Accept connection from {_client.Client.RemoteEndPoint} => {_setting.Server!.Ip!}:{_setting.Server!.Port}");

                    using NetworkStream clientStream = _client.GetStream();
                    using NetworkStream remoteStream = remoteClient.GetStream();

                    Task t1 = PipeData(clientStream, remoteStream);
                    Task t2 = PipeData(remoteStream, clientStream);

                    await Task.WhenAny(t1, t2);
                }
                catch (Exception error)
                {
                    _logger.Error($"{error.Message}");
                }
            }
        }

        private async Task PipeData(NetworkStream input, NetworkStream output)
        {
            byte[] buffer = new byte[8192];
            int bytesRead;
            try
            {
                while ((bytesRead = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await output.WriteAsync(buffer, 0, bytesRead);
                    await output.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Problem in transfer data: {ex.Message}");
            }
        }
    }
}