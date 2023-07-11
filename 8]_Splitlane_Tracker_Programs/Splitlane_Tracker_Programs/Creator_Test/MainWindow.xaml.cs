﻿using Serilog;
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
using SplitlaneTracker.Services;
using System.Net.Sockets;
using SplitlaneTracker.Services.Tracking;
using System.IO;

namespace TestPlatform
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static SplitlaneTracker.Services.Logging.Logger log = new SplitlaneTracker.Services.Logging.Logger();
        public MainWindow()
        {
            InitializeComponent();
            log.open();
        }
        private string json = "asdf";


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _ = thing();
        }
        private async Task thing() { 
            var client = new SplitlaneTracker.Services.Mqtt.Mqtt(log,"tester","localhost","test","offline");
            await client.Connect();

            string init = @"{""KartList"":[{""Colour"":""red"",""Number"":17,""DetectionList"":[]},{""Colour"":""green"",""Number"":23,""DetectionList"":[]},{""Colour"":""blue"",""Number"":9,""DetectionList"":[]}],""RequiredLaps"":[4,4,10]}";
            await client.Publish("raceinfo", init);

            await Task.Delay(1000);

            await client.Publish("command/race","start");

            await Task.Delay(1000);
            /*
            await client.Publish("detect", @"{""Time"": 1687638447,""Colour"": ""red"",""Lane"": ""left""}");
            await Task.Delay(300);*/

            // Colours are "red", "green", "blue" - "yellow" for live addition testing
            // Lanes are "left, "right"

            string[] colours = { "red", "green", "blue", "yellow" };
            string[] lanes = { "left", "right" };

            Random random = new Random();
            for (int i = 0; i < 50; i++)
            {
                int col = random.Next(4);
                int lan = random.Next(2);
                int del = random.Next(3000);

                string info = "{" + $"\"Time\": {TimeNow()},\"Colour\": \"{colours[col]}\",\"Lane\": \"{lanes[lan]}\"" + "}";
                log.log(info);

                await client.Publish("detect", info);
                await Task.Delay(del);
            }


            await Task.Delay(1000);

            await client.Publish("command/race", "end");

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string json = @"{""KartList"":[{""Colour"":""red"",""Number"":""17"",""Name"":null,""DetectionList"":[{""Time"":1687638447,""Colour"":""red"",""Lane"":""left"",""TimeReadable"":""12:47:18.447""},{""Time"":1687638447,""Colour"":""red"",""Lane"":""left"",""TimeReadable"":""12:47:18.447""},{""Time"":1687638447,""Colour"":""red"",""Lane"":""right"",""TimeReadable"":""12:47:18.447""},{""Time"":1687638447,""Colour"":""red"",""Lane"":""left"",""TimeReadable"":""12:47:18.447""}],""NextExpectedDetection"":0},{""Colour"":""green"",""Number"":""23"",""Name"":null,""DetectionList"":[{""Time"":1687638447,""Colour"":""green"",""Lane"":""left"",""TimeReadable"":""12:47:18.447""},{""Time"":1687638447,""Colour"":""green"",""Lane"":""left"",""TimeReadable"":""12:47:18.447""}],""NextExpectedDetection"":0},{""Colour"":""blue"",""Number"":""9"",""Name"":null,""DetectionList"":[{""Time"":1687638447,""Colour"":""blue"",""Lane"":""right"",""TimeReadable"":""12:47:18.447""},{""Time"":1687638447,""Colour"":""blue"",""Lane"":""left"",""TimeReadable"":""12:47:18.447""}],""NextExpectedDetection"":0}],""RequiredLaps"":[8,8,20]}";

            var myRace = new SplitlaneTracker.Services.Tracking.Race.Race();
            myRace.InitFromJson(json);

            string dir = Environment.CurrentDirectory + "\\output";
            Directory.CreateDirectory(dir);
            myRace.ExportToFileAsJson(dir);
            myRace.ExportToFileAsText(dir);
            log.log("Current working dir: " + dir);
        }
        /*
        private static SplitlaneTracker.Services.Tcp.TcpServer tCPsERVER =
            new SplitlaneTracker.Services.Tcp.TcpServer(
                "localhost",
                8080,
                Environment.CurrentDirectory +
                "\\RaceDisplayPage.html", log);*/
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //tCPsERVER.Send("ping");

        }

        private long TimeNow()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }

}
