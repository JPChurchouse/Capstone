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
    #region Ignore
    [System.ComponentModel.DesignerCategory("")]
    public class ddddd { }
    #endregion
    public partial class GUI : Form
    {
        private async Task<bool> Mqtt_Init()
        {
            log.log("MQTT Initalising");

            Mqtt_Client.OnDisonnected += Mqtt_OnDisconnect;
            Mqtt_Client.OnConnected += Mqtt_OnConnected;
            Mqtt_Client.OnReceived += Mqtt_OnReceived;

            var result = await Mqtt_Client.Connect();

            log.log("MQTT Init complete");
            return result;
        }


        private Services.Mqtt.Mqtt Mqtt_Client = new Services.Mqtt.Mqtt(
            log,
            "SplitlaneTrackerServer",
            Settings.Default.MqttBrokerAddress,
            "status/server",
            "offline");

        private Task Mqtt_OnDisconnect()
        {
            return Task.CompletedTask;
        }
        private async Task Mqtt_OnConnected()
        {
            await Mqtt_Client.Subscribe("command/race");
            await Mqtt_Client.Subscribe("command/server");
            await Mqtt_Client.Subscribe("raceinfo");
            await Mqtt_Client.Subscribe("detect");
            return;
        }

        private Task Mqtt_OnReceived(Packet packet)
        {
            string topic = packet.topic;
            string message = packet.payload;
            log.log($"New MQTT message received: {topic},{message}");

            // Server command
            if (topic.Contains("command/server"))
            {
                log.log("command/server");
                NewServerCommand(message);
            }

            // New race config
            else if (topic.Contains("raceinfo"))
            {
                log.log("raceinfo");
                Race_New(message);
            }

            // Race command
            else if (topic.Contains("command/race"))
            {
                log.log("command/race");
                NewRaceCommand(message);
            }

            // Detection
            else if (topic.Contains("detect"))
            {
                log.log("detect");
                Race_Detection(message);
            }

            // Unrecognised
            else
            {
                log.log($"Unable to process message: {topic},{message}");
            }

            return Task.CompletedTask;
        }

        private Task Mqtt_Send(Packet packet)
        {
            _ = Mqtt_Client.Publish(packet);

            return Task.CompletedTask;
        }

        private Task Mqtt_Close()
        {
            _ = Mqtt_Client.Disconnect();

            return Task.CompletedTask;
        }
    }
}
