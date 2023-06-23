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
        private string name, address;
        public delegate void OnReceivedEventHandler(Packet packet);
        public event OnReceivedEventHandler? OnReceived;

        public Mqtt(string name, string address)
        {
            this.name = name;
            this.address = address;

            _ = Connect();
        }

        private async Task Connect()
        {
            // Create TCP based options using the builder.
            var options = new MqttClientOptionsBuilder()
                .WithClientId(name)
                .WithTcpServer(address)
                .WithCleanSession()
                .Build();

            mqttClient.UseDisconnectedHandler(On_Disconnect);
            mqttClient.UseApplicationMessageReceivedHandler(On_Receive);
            mqttClient.UseConnectedHandler(On_Connected);

            await mqttClient.ConnectAsync(options, CancellationToken.None);
        }

        private async Task On_Disconnect(MqttClientDisconnectedEventArgs args)
        {
            Console.WriteLine("### DISCONNECTED FROM SERVER ###");
            await Task.Delay(TimeSpan.FromSeconds(5));

            try
            {
                await Connect();
            }
            catch
            {
                Console.WriteLine("### RECONNECTING FAILED ###");
            }
        }

        private Task On_Receive(MqttApplicationMessageReceivedEventArgs args)
        {
            Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
            Console.WriteLine($"+ Topic = {args.ApplicationMessage.Topic}");
            Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(args.ApplicationMessage.Payload)}");
            //Console.WriteLine($"+ QoS = {args.ApplicationMessage.QualityOfServiceLevel}");
            //Console.WriteLine($"+ Retain = {args.ApplicationMessage.Retain}");
            Console.WriteLine();

            var pack = new Packet(args.ApplicationMessage.Topic, Encoding.UTF8.GetString(args.ApplicationMessage.Payload));
            OnReceived?.Invoke(pack);

            return Task.CompletedTask;
        }

        private async Task On_Connected(MqttClientConnectedEventArgs args)
        {
            Console.WriteLine("### CONNECTED WITH SERVER ###");

            await SubscribeTo("test/topic");
        }

        private async Task SubscribeTo(string topic)
        {
            if (!mqttClient.IsConnected) return;

            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
            Console.WriteLine("### SUBSCRIBED ###");
        }

        private async Task Publish(string topic, string message)
        {
            if (!mqttClient.IsConnected) return;

            var packet = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
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
            _ = Publish(packet.topic, packet.message);
        }
    }
}
