using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Net;
using Serilog;
using System.Net.Sockets;

namespace XKarts.Comms
{
    public static class Communicator
    {
        // POST update TO the server
        public static void PostToServer(Logging.Logger log, Command command, string data = "none")
        {
            var ipAddress = Constants.IpAddress;
            var port = Constants.PortNum;

            try
            {
                log.log($"IP and Port: {ipAddress}:{port}");
                using (TcpClient client = new TcpClient())
                {
                    // Connect to the IP address and port
                    client.Connect(ipAddress, port);

                    // Get the network stream for sending/receiving data
                    NetworkStream stream = client.GetStream();

                    // Create the PUT request
                    string request = 
                        $"POST /{command} HTTP/1.1\r\n" +
                        $"Host: {ipAddress}\r\n" +
                        $"Content-Length: {data.Length}\r\n" +
                        $"\r\n" +
                        $"{data}";

                    // Send the PUT request
                    byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                    stream.Write(requestBytes, 0, requestBytes.Length);

                    log.log("POST request sent.");
                }
            }
            catch (Exception ex)
            {
                log.log($"An error occurred: {ex.Message})");
            }
        }

        // GET update FROM server
        public static void GetFromServer(Logging.Logger log, Request command, string data = "none")
        {
            var ipAddress = Constants.IpAddress;
            var port = Constants.PortNum;

            try
            {
                log.log($"IP and Port: {ipAddress}:{port}");
                using (TcpClient client = new TcpClient())
                {
                    // Connect to the IP address and port
                    client.Connect(ipAddress, port);

                    // Get the network stream for sending/receiving data
                    NetworkStream stream = client.GetStream();

                    // Create the PUT request
                    string request = 
                        $"GET /{command} HTTP/1.1\r\n" +
                        $"Host: {ipAddress}\r\n" +
                        $"Content-Length: {data.Length}\r\n" +
                        $"\r\n" +
                        $"{data}";

                    // Send the PUT request
                    byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                    stream.Write(requestBytes, 0, requestBytes.Length);

                    log.log("GET request sent.");
                }
            }
            catch (Exception ex)
            {
                log.log($"An error occurred: {ex.Message})");
            }
        }
    }
}
