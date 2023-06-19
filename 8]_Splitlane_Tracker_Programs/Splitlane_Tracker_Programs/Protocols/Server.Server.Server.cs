using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace XKarts.Server
{
    public partial class Server
    {
        private static void RunTcpServer()
        {

            // Specify the IP address and port number
            IPAddress ipAddress = IPAddress.Any;
            int port = Comms.Constants.PortNum;

            // Create a TCP listener
            TcpListener listener = new TcpListener(ipAddress, port);

            try
            {
                // Start listening for incoming connections
                listener.Start();
                log.log($"Server started. Listening on {ipAddress}:{port}");

                while (true)
                {
                    log.log("Waiting for a client connection...");

                    // Accept a client connection
                    TcpClient client = listener.AcceptTcpClient();
                    log.log($"Client connected: {client.Client.RemoteEndPoint}");

                    // Handle the client connection in a separate thread
                    HandleClient(client);
                }
            }
            catch (Exception ex)
            {
                log.log(ex);
            }
            finally
            {
                // Stop listening and clean up
                listener.Stop();
            }
        }
    }
}
