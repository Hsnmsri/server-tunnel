namespace TunnelServer.Libraries.Setting
{
    public class AppConfig
    {
        public LoggingConfig? Logging { get; set; }
        public LocalConfig? Local { get; set; }
        public ServerConfig? Server { get; set; }
    }
}