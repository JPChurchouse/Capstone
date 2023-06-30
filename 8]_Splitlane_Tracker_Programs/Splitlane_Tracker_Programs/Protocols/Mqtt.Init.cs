using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

namespace XKarts.Mqtt
{
    public class Mqtt
    {
        // Create a new MQTT client.
        private IMqttClient mqttClient = new MqttFactory().CreateMqttClient();
        private string client_name, broker_address, will_topic, will_payload;
        
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
            this.client_name = client_name;
            this.broker_address = broker_address;
            this.will_topic = will_topic;
            this.will_payload = will_payload;

            _ = Connect();
        }

        private async Task Connect()
        {
            var will = new MqttApplicationMessage()
            {
                Topic = will_topic,
                Payload = Encoding.UTF8.GetBytes(will_payload),
                QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce,
                Retain = true
            };

            // Create TCP based options using the builder.
            var options = new MqttClientOptionsBuilder()
                .WithClientId(client_name)
                .WithTcpServer(broker_address)
                .WithWillMessage(will)
                .Build();

            mqttClient.UseDisconnectedHandler(On_Disconnect);
            mqttClient.UseApplicationMessageReceivedHandler(On_Receive);
            mqttClient.UseConnectedHandler(On_Connected);

            await mqttClient.ConnectAsync(options, CancellationToken.None);
        }


        private Task On_Disconnect(MqttClientDisconnectedEventArgs args)
        {
            OnDisonnected?.Invoke();

            return Task.CompletedTask;
        }

        private Task On_Receive(MqttApplicationMessageReceivedEventArgs args)
        {
            OnReceived?.Invoke(
                new Packet(
                    args.ApplicationMessage.Topic, 
                    Encoding.UTF8.GetString(args.ApplicationMessage.Payload)));

            return Task.CompletedTask;
        }

        private Task On_Connected(MqttClientConnectedEventArgs args)
        {
            OnConnected?.Invoke();

            return Task.CompletedTask;
        }

        private async Task SubscribeTo(string topic)
        {
            if (!mqttClient.IsConnected) return;

            await mqttClient.SubscribeAsync(
                new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .WithAtLeastOnceQoS()
                .WithRetainAsPublished()
                .Build());
            //Console.WriteLine("### SUBSCRIBED ###");
        }

        private async Task Publish(string topic, string message)
        {
            if (!mqttClient.IsConnected) return;

            var packet = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithAtLeastOnceQoS()
                .WithRetainFlag()
                .Build();

            await mqttClient.PublishAsync(packet, CancellationToken.None);
        }

        public bool IsConnected() { return  mqttClient.IsConnected; }
        public bool TryConnect()
        {
            if (mqttClient.IsConnected) return true;

            _ = Connect();

            return IsConnected();
        }
        public void Send(Packet packet)
        {
            _ = Publish(packet.topic, packet.payload);
        }
        public void Subscibe(string topic)
        {
            _ = SubscribeTo(topic);
        }

        public void Disconnect()
        {
            mqttClient.DisconnectAsync(CancellationToken.None);
        }
    }
}
