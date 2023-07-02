using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SplitlaneTracker.Services.Tcp
{
    public class TcpServer
    {
        private static string page_file = "";
        private static bool running = true;

        //public static string WsIpAddress = "http://localhost:8082/";

        private static int Port;
        private static string IpAddress;
        private static string FullAddress_Http;
        private static string FullAddress_Ws;

        private static HttpListener HttpListener;
        private static List<WebSocket> WsClients = new List<WebSocket>();

        private static Logging.Logger log;

        public TcpServer(string addr, int port, string file, Logging.Logger l) 
        {
            IpAddress = addr;
            Port = port;
            FullAddress_Http = $"http://{IpAddress}:{Port}/";
            FullAddress_Ws = $"ws://{IpAddress}:{Port}/";

            log = l;
            page_file = file;
            log.log(page_file);

            HttpListener = new HttpListener();
            _ = Run();
        }


        #region Main Task
        private static async Task Run()
        {
            running = true;

            HttpListener.Prefixes.Add(FullAddress_Http);
            HttpListener.Start();
            log.log("HTTP listener started.");

            while (running)
            {
                // Wait for a client to connect
                HttpListenerContext context = await HttpListener.GetContextAsync();

                // Handle the HTTP request
                if (context.Request.IsWebSocketRequest)
                {
                    HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                    WebSocket webSocket = webSocketContext.WebSocket;

                    // Add the WebSocket client to the list
                    WsClients.Add(webSocket);

                    // Handle the WebSocket connection in a separate task
                    _ = HandleWebSocketConnection(webSocket);
                }
                else
                {
                    // Handle regular HTTP request
                    await HandleHttpRequestAsync(context);
                }
            }
        }
        #endregion

        #region WebSockets

        private static async Task HandleWebSocketConnection(WebSocket webSocket)
        {
            try
            {
                byte[] receiveBuffer = new byte[1024];

                while (webSocket.State == WebSocketState.Open)
                {
                    // Receive data from the client
                    WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

                    if (receiveResult.MessageType == WebSocketMessageType.Text)
                    {
                        // Process received WebSocket message
                        string receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);
                        Console.WriteLine($"Received WebSocket message: {receivedData}");

                        // Send the message to all connected clients
                        await SendToAllClients(receivedData);
                    }
                    else if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        // Handle close message
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);

                        // Remove the WebSocket client from the list
                        WsClients.Remove(webSocket);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the WebSocket connection
                Console.WriteLine($"WebSocket error: {ex.Message}");

                // Remove the WebSocket client from the list
                WsClients.Remove(webSocket);
            }
            finally
            {
                // Clean up resources
                webSocket.Dispose();
            }
        }

        private static async Task SendToAllClients(string message)
        {
            // Send the message to all connected clients
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            foreach (WebSocket client in WsClients)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
        public void Send(string payload)
        {
            _ = SendToAllClients(payload);
        }

        #endregion

        #region Http

        private static async Task HandleHttpRequestAsync(HttpListenerContext context)
        {
            // Read the requested file (e.g., HTML page)
            //string filePath = context.Request.Url.AbsolutePath.TrimStart('/');
            string fileContent = await ReadFileAsync(page_file);
            fileContent = fileContent.Replace("INSERT_WS_IP_ADDRESS_HERE", FullAddress_Ws);

            // Send the file content as the HTTP response
            byte[] buffer = Encoding.UTF8.GetBytes(fileContent);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }

        #endregion

        #region Helpers
        private static async Task<string> ReadFileAsync(string filePath)
        {
            // Read the file content asynchronously
            return await File.ReadAllTextAsync(filePath);
        }
        #endregion
    }
}
