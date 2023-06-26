using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

namespace SplitlaneTracker.Services.Mqtt
{
    public partial class Mqtt
    {
        // Create a new MQTT client.
        private IMqttClient client = new MqttFactory().CreateMqttClient();
        private string client_name, broker_address, will_topic, will_payload;

        private Logging.Logger log = new Logging.Logger();

        // OnReceived user handler
        public delegate void OnReceivedEventHandler(Packet packet);
        public event OnReceivedEventHandler? OnReceived;

        // OnConnected user handler
        public delegate void OnConnectedEventHandler();
        public event OnConnectedEventHandler? OnConnected;

        // OnDisonnected user handler
        public delegate void OnDisonnectedEventHandler();
        public event OnDisonnectedEventHandler? OnDisonnected;

        public Mqtt(string client_name, string broker_address, string will_topic, string will_payload)
        {
            log.log("Initalising MQTT");

            this.client_name = client_name;
            this.broker_address = broker_address;
            this.will_topic = will_topic;
            this.will_payload = will_payload;

            _ = Connect();
        }
    }
}
