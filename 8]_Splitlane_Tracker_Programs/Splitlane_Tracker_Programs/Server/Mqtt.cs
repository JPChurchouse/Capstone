using SplitlaneTracker.Server.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplitlaneTracker.Services;
using SplitlaneTracker.Services.Mqtt;

namespace SplitlaneTracker.Server
{
    internal class Mqtt
    {
        // OnReceived user handler
        public delegate void MqttReceivedHandler(Packet packet);
        public event MqttReceivedHandler? HandleMqttPacket;

        private static Services.Logging.Logger log;
        public Mqtt(Services.Logging.Logger l) 
        {
            log = l;
            log.log("MQTT Initalising");

            client.OnDisonnected += OnDisconnect;
            client.OnConnected += OnConnected;
            client.OnReceived += OnReceived;

            log.log("MQTT Init complete");
        }


        private Services.Mqtt.Mqtt client = new Services.Mqtt.Mqtt(
            "SplitlaneTrackerServer",
            Settings.Default.MqttBrokerAddress,
            "status/server",
            "offline");

        private void OnDisconnect()
        {
            if (disconnect) return;
            while (!client.TryConnect());
        }
        private void OnConnected()
        {
            client.Subscibe("command/race");
            client.Subscibe("command/server");
            client.Subscibe("config");
            client.Subscibe("detect");
        }

        private void OnReceived(Packet packet)
        {
            HandleMqttPacket?.Invoke(packet);
        }

        public void Send(Packet packet)
        {
            client.Send(packet);
        }

        private bool disconnect = false;
        public void Close()
        {
            client.Disconnect();
            disconnect = true;
        }
    }
}
