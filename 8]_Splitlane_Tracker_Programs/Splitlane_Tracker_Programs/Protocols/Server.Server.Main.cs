using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace XKarts.Server
{
    public partial class Server
    {
        // Logger
        private static Logging.Logger log = new Logging.Logger();

        // List of active Karts and their info
        private List<KartStats> KartList = new List<KartStats>();

        // Number of required laps
        private byte Laps_Left, Laps_Right, Laps_Total;


        private static void ListenTCP()
        {
            log.log("TCP Server initalising");

            // Specify the desired hostname and port
            string hostname = Comms.Constants.HostName;
            int port = Comms.Constants.PortNum;

            try
            {
                // Resolve the hostname to an IP address
                IPAddress ipAddress = ResolveIpAddress(hostname);

                // Start the TCP server
                TcpListener server = new TcpListener(ipAddress, port);
                server.Start();

                log.log("Server started. Listening on: " + ipAddress + ":" + port);

                while (true)
                {
                    // Accept client connections
                    TcpClient client = server.AcceptTcpClient();
                    log.log("Client connected.", Logging.Logger.Type.info);

                    // Handle client communication in a separate thread
                    HandleClient(client);
                }
            }
            catch (Exception ex)
            {
                log.log(ex);
                throw;
            }
        }

        

        private static IPAddress ResolveIpAddress(string hostname)
        {
            IPAddress? ipAddress = null;

            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
                ipAddress = hostEntry.AddressList[0]; // Get the first resolved IP address
            }
            catch
            {
                throw;
            }

            if (ipAddress == null)
            {
                throw new NullReferenceException("Failed to resolve IP address from hostname");
            }

            return ipAddress;
        }
    }
}
