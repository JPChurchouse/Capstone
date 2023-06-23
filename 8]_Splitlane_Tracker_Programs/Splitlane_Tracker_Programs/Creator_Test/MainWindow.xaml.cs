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
            List<XKarts.RaceInfo.Kart> list = new List<XKarts.RaceInfo.Kart> ();
            list.Add(new XKarts.RaceInfo.Kart(03, Colour.Blue));
            list.Add(new XKarts.RaceInfo.Kart(01, Colour.Red));
            list.Add(new XKarts.RaceInfo.Kart(02, Colour.Green));
            var steve = new XKarts.RaceInfo.Race(list,05,15,0);

            string JSON = steve.GenerateJsonString();
            log.log(JSON);

            //SendPostRequest( XKarts.Comms.Constants.IpAddress.ToString(), XKarts.Comms.Constants.PortNum, JSON);
            XKarts.Comms.Communicator.PostToServer(log, XKarts.Comms.Command.NewRaceInfo, JSON);

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }

}
