using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace SplitLaneTracker_Server
{
    internal class Program
    {
        static void Main()
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
                Console.WriteLine("Server started. Listening on {0}:{1}", ipAddress, port);

                while (true)
                {
                    Console.WriteLine("Waiting for client connection...");
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Client connected!");
                    
                    // Process the client request
                    ProcessClientRequest(client);

                    // Close the client connection
                    client.Close();
                    Console.WriteLine("Client connection closed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: {0}", ex.Message);
            }
            finally
            {
                // Stop listening and clean up
                listener?.Stop();
            }
        }

        static void ProcessClientRequest(TcpClient client)
        {
            // Get the client stream
            NetworkStream stream = client.GetStream();

            // Read the request from the client
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received request: {0}", request);

            // Parse the request method
            string[] requestLines = request.Split('\n');
            string requestMethod = requestLines[0].Split(' ')[0].Trim();

            // Process the request based on the method
            if (requestMethod == "PUT")
            {
                // Extract the data from the request (assuming the data is in the body)
                int bodyIndex = Array.IndexOf(requestLines, "\r");
                string requestData = string.Join("\n", requestLines, bodyIndex + 1, requestLines.Length - bodyIndex - 1);
                Console.WriteLine("PUT request data: {0}", requestData);

                // TODO: Handle the PUT request as needed
            }
            else if (requestMethod == "GET")
            {
                // TODO: Handle the GET request as needed

                // Send a response to the client
                string response = "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nHello, world!";
                byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
                Console.WriteLine("Sent response: {0}", response);
            }
            else
            {
                // Unsupported method
                string response = "HTTP/1.1 405 Method Not Allowed\r\n\r\n";
                byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
                Console.WriteLine("Sent response: {0}", response);
            }
        }
    }
}
