using System.Net;
using System.Net.Sockets;
using TunnelServer.Libraries.Setting;

namespace TunnelServer.Libraries.TcpServer
{
    public class TcpServer
    {
        private readonly TcpListener _listener;
        private readonly SettingLoader _setting;
        private readonly Logger.Logger _logger;

        public TcpServer(SettingLoader setting, Logger.Logger logger)
        {
            _setting = setting;
            _logger = logger;
            _listener = new TcpListener(IPAddress.Parse(_setting.Local!.Ip!), _setting.Local!.Port);
        }

        public void Start()
        {
            _listener.Start();
            _logger.Success($"Tunnel server listening on {_setting.Local!.Ip!}:{_setting.Local!.Port} port (Force Stop CTRL + C)");
        }

        public async Task<TcpClient> AcceptClientAsync()
        {
            return await _listener.AcceptTcpClientAsync();
        }
    }
}