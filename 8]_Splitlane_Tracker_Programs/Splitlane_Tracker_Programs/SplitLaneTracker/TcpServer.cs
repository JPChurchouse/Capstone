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
    public static bool running = true;

    private static string BrokerAddr;

    private static TcpListener listener;

    private static Logging.Logger log;

    public TcpServer(string addr, string file, Logging.Logger l) 
    {
      BrokerAddr = addr;

      log = l;
      page_file = file;
      log.log(page_file);

      listener = new TcpListener(IPAddress.Any, 8080);
      _ = Run();
    }


    #region Main Task
    private static async Task Run()
    {
      running = true;

      listener.Start();
      log.log("HTTP listener started.");

      while (running)
      {
        TcpClient client = await listener.AcceptTcpClientAsync();
        _ = ProcessRequestAsync(client);
      }
      listener.Stop();
    }


    private static async Task ProcessRequestAsync(TcpClient client)
    {
      using (NetworkStream stream = client.GetStream())
      {
        byte[] buffer = new byte[4096];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        string fileContent = await ReadFileAsync(page_file);
        fileContent = fileContent.Replace("INSERT_MQTT_ADDRESS_HERE", BrokerAddr);
        string responseContent = fileContent;
        byte[] responseBytes = Encoding.UTF8.GetBytes(responseContent);

        string response = $"HTTP/1.1 200 OK\r\nContent-Type: text/html\r\nContent-Length: {responseBytes.Length}\r\n\r\n";
        byte[] headerBytes = Encoding.UTF8.GetBytes(response);

        await stream.WriteAsync(headerBytes, 0, headerBytes.Length);
        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
      }

      client.Close();
    }

    private static async Task<string> ReadFileAsync(string filePath)
    {
      try
      {
        return await File.ReadAllTextAsync(filePath);
      }
      catch
      {
        return string.Empty; 
      }
    }
    #endregion
  }
}
