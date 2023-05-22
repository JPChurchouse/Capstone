using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;

namespace SplitLaneTracker_ClientTEST
{
    public partial class Form1 : Form
    {
        #region BUTTONS

        private void btn_Get_Click(object sender, EventArgs e)
        {
            try
            {
                int port = Int32.Parse(tb_Port.Text);
                SendGetRequest(tb_Address.Text, port);
            }
            catch { }
        }

        private void btn_Post_Click(object sender, EventArgs e)
        {
            try
            {
                int port = Int32.Parse(tb_Port.Text);
                SendPutRequest(tb_Address.Text, port, tb_Message.Text);
            }
            catch { }
        }

        #endregion

        #region REQUESTS

        static bool IsIpAddressReachable(string ipAddress, int port)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    // Connect to the IP address and port
                    client.Connect(ipAddress, port);
                    Log.Debug("Server is available");
                    return true;
                }
            }
            catch (Exception)
            {
                Log.Debug("Server is NOT available");
                return false;
            }
        }

        static void SendGetRequest(string ipAddress, int port)
        {
            Log.Debug("Sending GET request");
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    // Connect to the IP address and port
                    client.Connect(ipAddress, port);

                    // Get the network stream for sending/receiving data
                    NetworkStream stream = client.GetStream();

                    // Send a GET request
                    string request = "GET /get HTTP/1.1\r\nHost: " + ipAddress + "\r\n\r\n";
                    byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                    stream.Write(requestBytes, 0, requestBytes.Length);

                    // Read the response from the server
                    byte[] responseBytes = new byte[4096];
                    int bytesRead = stream.Read(responseBytes, 0, responseBytes.Length);
                    string response = Encoding.ASCII.GetString(responseBytes, 0, bytesRead);
                    Log.Debug("Response:\n{0}", response);
                }
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred: {0}", ex.Message);
            }
        }

        static void SendPutRequest(string ipAddress, int port, string data)
        {
            Log.Debug("Sending PUT request");
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    // Connect to the IP address and port
                    client.Connect(ipAddress, port);

                    // Get the network stream for sending/receiving data
                    NetworkStream stream = client.GetStream();

                    // Create the PUT request
                    string request = $"PUT /put HTTP/1.1\r\nHost: {ipAddress}\r\nContent-Length: {data.Length}\r\n\r\n{data}";

                    // Send the PUT request
                    byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                    stream.Write(requestBytes, 0, requestBytes.Length);

                    Log.Debug("PUT request sent.");
                }
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred: {0}", ex.Message);
            }
        }

        #endregion

        #region LOGGING
        private static string file_Logging;
        private static void InitLog()
        {
            string timenow = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            file_Logging = $"logs\\{timenow}_SplitlaneTracker.log";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File(file_Logging)
                .CreateLogger();
            Log.Information("This programme was developed by J. P. Churchouse");
            Log.Information("Started programme at time: " + timenow);
        }
        public static void OpenLogFile()
        {
            try { Process.Start("explorer.exe", $"/select, {Environment.CurrentDirectory}\\{file_Logging}"); }
            catch { }
        }
        public static void meme()
        {
            if (!Environment.UserName.Contains("hurchouse"))
            {
                try { for (int i = 0; i < 10; i++) { Process.Start("explorer", "https://youtu.be/oHg5SJYRHA0"); } }
                catch { }
            }
        }
        #endregion
        
        public Form1()
        {
            InitializeComponent();

            InitLog();
            meme();
        }
    }
}
