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
using MQTTnet.Client.Subscribing;

namespace SplitlaneTracker.Services.Mqtt
{
    public partial class Mqtt
    {
        public ushort connection_attempts = 3;

        // Connect the client to the MQTT broker
        public async Task<bool> Connect()
        {
            log.log("Mqtt.Connect()");

            // Create WLT packet
            var will = new MqttApplicationMessage()
            {
                Topic = will_topic,
                Payload = Encoding.UTF8.GetBytes(will_payload),
                QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce,
                Retain = true
            };

            // Create options packet
            var options = new MqttClientOptionsBuilder()
                .WithClientId(client_name)
                .WithTcpServer(broker_address)
                .WithWillMessage(will)
                .Build();

            // Connect event handlers
            client.UseDisconnectedHandler(On_Disconnect);
            client.UseApplicationMessageReceivedHandler(On_Receive);
            client.UseConnectedHandler(On_Connected);

            // Make connection (attempt a number of times)
            log.log("ConnectAsync begin");
            for (int i = 0; i < connection_attempts; i++)
            {
                try
                {
                    var result = await client.ConnectAsync(options, CancellationToken.None).ConfigureAwait(false);
                    if (MqttClientConnectResultCode.Success == result.ResultCode)
                    {
                        log.log($"Connection attempt {i+1} of {connection_attempts} succeded");
                        break;
                    }
                    else
                    {
                        log.log($"Connection attempt {i+1} of {connection_attempts} failed");
                    }
                }
                catch (Exception e)
                {
                    log.log(e);
                    return false;
                }
            }

            // Finishing up
            log.log($"Connect() complete: {client.IsConnected}");
            return client.IsConnected;
        }

        // Make a subscription
        public async Task<bool> Subscribe(string topic)
        {
            log.log("Mqtt.SubsribeTo()");
            log.log($"Topic:{topic}");

            // Connection validation
            if (!client.IsConnected)
            {
                log.log("Connection error", Logging.Logger.Type.error);
                return false;
            }

            // Create packet
            var sub = new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .WithAtLeastOnceQoS()
                .WithRetainAsPublished()
                .Build();

            // Make request
            try
            {
                await client.SubscribeAsync(sub);
            }
            // Request failed
            catch (Exception ex)
            {
                log.log(ex);
                return false;
            }

            // Finishing up
            log.log("SubsribeTo complete");
            return true;
        }

        // Publish to the MQTT broker
        public async Task<bool> Publish(Packet packet)
        {
            return await Publish(packet.topic, packet.payload).ConfigureAwait(false);
        }
        public async Task<bool> Publish(string topic, string payload)
        {
            log.log("Mqtt.Publish()");
            log.log($"Topic:{topic}, Payload:{payload}");

            // Connection validation
            if (!client.IsConnected)
            {
                log.log("Connection error", Logging.Logger.Type.error);
                return false;
            }

            // Create packet
            var packet = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithAtLeastOnceQoS()
                .WithRetainFlag()
                .Build();

            // Make request
            try
            {
                await client.PublishAsync(packet, CancellationToken.None);
            }
            // Requset failed
            catch (Exception ex)
            {
                log.log(ex);
                return false;
            }

            // Finishing up
            log.log("Publish complete");
            return true;
        }

        // Disconnect from broker
        public async Task Disconnect()
        {
            log.log("Mqtt.Disconnect()");

            // Manual disconnect - don't reconnect
            stay_connected = false;

            // Make request
            try
            {
                await client.DisconnectAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                log.log(ex);
            }

            // Finishing up
            log.log("Disconnect complete");
            return;
        }
    }
}
