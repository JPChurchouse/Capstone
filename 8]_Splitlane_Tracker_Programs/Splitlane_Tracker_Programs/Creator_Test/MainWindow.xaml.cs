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
using SplitLaneTracker.Services;
using System.Net.Sockets;
using SplitLaneTracker.Services.Tracking;

namespace TestPlatform
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static SplitLaneTracker.Services.Logging.Logger log = new SplitLaneTracker.Services.Logging.Logger();
        public MainWindow()
        {
            InitializeComponent();
        }
        private string json = "asdf";


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Detection detection = new Detection(1687638447, "red","left");
            json = detection.GetJson();

            log.log($"Time:{detection.Time},Colour:{detection.Colour},Lane:{detection.Lane}");
            log.log(json);
            log.open();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Detection det = new Detection(json);

            log.log($"Time:{det.Time},Colour:{det.Colour},Lane:{det.Lane}");
            log.open();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string asdf = @"{""KartList"":[{""Colour"":""red"",""Number"":17,""DetectionList"":[]},{""Colour"":""green"",""Number"":23,""DetectionList"":[]},{""Colour"":""blue"",""Number"":9,""DetectionList"":[]}],""RequiredLaps"":[8,8,20]}";

            SplitLaneTracker.Services.Tracking.Race.Race race = new SplitLaneTracker.Services.Tracking.Race.Race(asdf);

            if (race == null)
            {
                log.log("failed to convert to obj");
                return;
            }

            log.log($"Laps: {race.RequiredLaps[0]},{race.RequiredLaps[1]},{race.RequiredLaps[2]}");
            foreach (Kart kart in race.KartList)
            {
                log.log($"{kart.Number},{kart.Colour}");
            }
            log.open();
        }
    }

}
