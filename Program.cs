using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TunnelServer.Libraries.Logger;

class Program
{
    const string localHost = "0.0.0.0";
    const int localPort = 1001;
    const string remoteHost = "167.235.156.113";
    const int remotePort = 1001;

    static Logger logger = new Logger();

    static async Task Main(string[] args)
    {
        try
        {
            // Create and run tcp socket listener
            TcpListener listener = new TcpListener(IPAddress.Parse(localHost), localPort);
            listener.Start();
            logger.Success($"Tunnel server listening on {localHost}:{localPort} port (Force Stop CTRL + C)");

            while (true)
            {
                // Accept connected tcp client
                TcpClient client = await listener.AcceptTcpClientAsync();

                // Accept connection on multi threads
                _ = Task.Run(() => HandleClient(client));
            }
        }
        catch (Exception error)
        {
            logger.Error(error.Message);
        }
    }

    static async Task HandleClient(TcpClient client)
    {
        using (client)
        {
            try
            {
                logger.Log("New connection created");
                using TcpClient remoteClient = new TcpClient();
                await remoteClient.ConnectAsync(remoteHost, remotePort);
                logger.Success($"Connection to {remoteHost}:{remotePort} created");

                // Get streams
                using NetworkStream clientStream = client.GetStream();
                using NetworkStream remoteStream = remoteClient.GetStream();

                // Parallel run read and write between client and remote client
                Task t1 = PipeData(clientStream, remoteStream);
                Task t2 = PipeData(remoteStream, clientStream);

                await Task.WhenAny(t1, t2);
            }
            catch (Exception ex)
            {
                logger.Error($"Error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Transfer data from input to output
    /// </summary>
    /// <param name="input"></param>
    /// <param name="output"></param>
    /// <returns></returns>
    static async Task PipeData(NetworkStream input, NetworkStream output)
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
            logger.Error($"Problem in transfer data: {ex.Message}");
        }
    }
}