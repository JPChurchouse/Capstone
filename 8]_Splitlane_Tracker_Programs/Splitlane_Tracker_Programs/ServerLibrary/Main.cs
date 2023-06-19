using Serilog;
using System.Data;
using System.Net.Sockets;
using System.Net;
using XKarts;
using XKarts.Server;

namespace Server
{
    public partial class Server
    {
        // Logger
        private XKarts.Logging.Logger log = new XKarts.Logging.Logger();

        // List of active Karts and their info
        private List<KartStats> KartList = new List<KartStats>();

        // Number of required laps
        private byte Laps_Left, Laps_Right, Laps_Total;



        private void ListenTCP()
        {
            TcpListener listener = null;
            try
            {
                // Set the IP address and port number for the server
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                int port = 6969;

                // Create the TCP listener
                listener = new TcpListener(ipAddress, port);

                // Start listening for client requests
                listener.Start();
                Log.Information("Server started. Listening on {0}:{1}", ipAddress, port);

                while (true)
                {
                    Log.Information("Waiting for client connection...");
                    TcpClient client = listener.AcceptTcpClient();
                    Log.Information("Client connected!");

                    // Process the client request
                    ProcessClientRequest(client);

                    // Close the client connection
                    client.Close();
                    Log.Information("Client connection closed.");
                }
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred: {0}", ex.Message);
            }
            finally
            {
                // Stop listening and clean up
                listener?.Stop();
            }
        }
    }
}