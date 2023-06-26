using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitlaneTracker.Services.Mqtt
{
    public partial class Mqtt
    {
        private async Task Connect()
        {
            log.log("Connect");

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

            client.UseDisconnectedHandler(On_Disconnect);
            client.UseApplicationMessageReceivedHandler(On_Receive);
            client.UseConnectedHandler(On_Connected);

            await client.ConnectAsync(options, CancellationToken.None);
        }


        private Task On_Disconnect(MqttClientDisconnectedEventArgs args)
        {
            log.log("On_Disconnect");

            OnDisonnected?.Invoke();

            return Task.CompletedTask;
        }

        private Task On_Receive(MqttApplicationMessageReceivedEventArgs args)
        {
            log.log("On_Receive");

            OnReceived?.Invoke(
                new Packet(
                    args.ApplicationMessage.Topic,
                    Encoding.UTF8.GetString(args.ApplicationMessage.Payload)));

            return Task.CompletedTask;
        }

        private Task On_Connected(MqttClientConnectedEventArgs args)
        {
            log.log("On_Connected");

            OnConnected?.Invoke();

            return Task.CompletedTask;
        }

        private async Task SubscribeTo(string topic)
        {
            log.log("SubsribeTo");

            if (!client.IsConnected) return;

            await client.SubscribeAsync(
                new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .WithAtLeastOnceQoS()
                .WithRetainAsPublished()
                .Build());
            //Console.WriteLine("### SUBSCRIBED ###");
        }

        private async Task Publish(string topic, string message)
        {
            log.log("Publish");

            if (!client.IsConnected) return;

            var packet = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithAtLeastOnceQoS()
                .WithRetainFlag()
                .Build();

            await client.PublishAsync(packet, CancellationToken.None);
        }
    }
}
