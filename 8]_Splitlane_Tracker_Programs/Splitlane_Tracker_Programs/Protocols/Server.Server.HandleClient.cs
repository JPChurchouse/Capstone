using System.Net.Sockets;
using System.Net;
using XKarts;
using System.Text;
using Serilog;

namespace XKarts.Server
{
    public partial class Server
    {
        static string HandleGet(string info)
        {
            log.log($"HandleGet: {info}");
            return "No information to show";
        }

        static void HandlePut(string info)
        {
            log.log($"HandlePut: {info}");
        }

        static void HandleClient(TcpClient client)
        {
            // Get the client stream
            NetworkStream stream = client.GetStream();

            // Read the request from the client
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            log.log($"Received request: {request}");

            // Parse the request method
            string[] requestLines = request.Split('\n');
            string[] requestHead = requestLines[0].Split(' ');
            string requestMethod = requestHead[0].Trim();
            string requestPage = requestHead[1].Trim();

            log.log("Requested page: " + requestPage);

            // Process the request based on the method
            if (requestMethod == "POST")
            {
                // Extract the data from the request (assuming the data is in the body)
                int bodyIndex = Array.IndexOf(requestLines, "\r");
                string requestData = string.Join("\n", requestLines, bodyIndex + 1, requestLines.Length - bodyIndex - 1);
                log.log($"PUT request data: {requestData}");

                // TODO: Handle the PUT request as needed
                HandlePut(requestData);
            }
            else if (requestMethod == "GET")
            {
                // TODO: Handle the GET request as needed
                string information = HandleGet(requestPage);

                // Send a response to the client
                string response = "HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\n";
                response += information;
                byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
                log.log($"Sent response: {response}");
            }
            else
            {
                // Unsupported method
                string response = "HTTP/1.1 405 Method Not Allowed\r\n\r\n";
                byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
                log.log($"Sent response: {response}");
            }
        }
    }
}