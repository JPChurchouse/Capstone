using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XKarts;
using XKarts.Logging;
using XKarts.Identifier;
using System.Net.Sockets;

namespace TestPlatform
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Logger log = new Logger();
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var colour = Colour.Red;
            ulong integer = 0xFF0000;

            log.log($"Testing RED: integer=={integer}, colour=={colour}, colasint=={(ulong)colour},equal=={(ulong)colour==integer}");

            log.open();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var steve = new XKarts.Creator.Race();
            steve.AddKart(new Kart(01, Colour.Red));
            steve.AddKart(new Kart(02, Colour.Green));
            steve.AddKart(new Kart(03, Colour.Blue));
            steve.Laps_Left = 05;
            steve.Laps_Right = 15;

            string JSON = steve.GenerateJsonString();
            log.log(JSON);

            SendPostRequest(
                XKarts.Comms.Constants.IpAddress.ToString(),
                XKarts.Comms.Constants.PortNum,
                JSON);

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }


        static void SendPostRequest(string ipAddress, int port, string data)
        {
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
                    string request = $"POST / HTTP/1.1\r\nHost: {ipAddress}\r\nContent-Length: {data.Length}\r\n\r\n{data}";

                    // Send the PUT request
                    byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                    stream.Write(requestBytes, 0, requestBytes.Length);

                    Console.WriteLine("PUT request sent.");
                }
            }
            catch (Exception ex)
            {
                log.log($"An error occurred: {ex.Message})");
            }
        }
    }

}
