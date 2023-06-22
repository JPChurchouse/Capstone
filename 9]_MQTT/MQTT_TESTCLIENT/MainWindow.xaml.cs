using MQTTnet.Client;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

using uPLibrary.Networking.M2Mqtt;
using MQTTnet.Server;
using System.Runtime.InteropServices.JavaScript;
using System.Windows.Interop;


namespace MQTT_TESTCLIENT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitMqtt();
        }

        private static uPLibrary.Networking.M2Mqtt.MqttClient? mqttClient;

        private void InitMqtt()
        {
            mqttClient = new uPLibrary.Networking.M2Mqtt.MqttClient("localhost");
            mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;
            mqttClient.Subscribe(new string[] { "test/topic" }, new byte[] { uPLibrary.Networking.M2Mqtt.Messages.MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            mqttClient.Connect("steve");
        }

        private void MqttClient_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                var message = Encoding.UTF8.GetString(e.Message);
                txt_Recieve.Text += message + "\n";
            });
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            if (mqttClient != null && mqttClient.IsConnected)
            {
                mqttClient.Publish("test/topic", Encoding.UTF8.GetBytes(txt_Send.Text));
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }
    }
}
