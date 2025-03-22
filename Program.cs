using System.Net.Sockets;
using TunnelServer.Libraries.ClientHandler;
using TunnelServer.Libraries.Logger;
using TunnelServer.Libraries.Setting;
using TunnelServer.Libraries.TcpServer;

namespace TunnelServer
{
    class Program
    {
        static readonly Logger _logger = new();
        static SettingLoader? _setting;

        static async Task Main(string[] args)
        {
            try
            {
                // Update App Settings
                _setting = new("settings.json", _logger);
                _setting.UpdateSettings();

                // Set Logger Setting
                _logger.Setting(_setting);

                // Tcp server
                TcpServer tcpServer = new(_setting, _logger);
                tcpServer.Start();

                await HoldAsync(tcpServer);
            }
            catch (Exception error)
            {
                _logger.Error(error.Message);
            }
        }

        static async Task HoldAsync(TcpServer tcpServer)
        {
            TcpClient tcpClient = await tcpServer.AcceptClientAsync();

            ClientHandler clientHandler = new(tcpClient, _setting!, _logger);
            _ = Task.Run(() => clientHandler.HandleAsync());

            await HoldAsync(tcpServer);
        }
    }
}
