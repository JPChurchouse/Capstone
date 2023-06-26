using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitlaneTracker.Services.Mqtt
{
    public partial class Mqtt
    {
        public bool IsConnected() 
        {
            bool connected = client.IsConnected;
            log.log($"IsConnected(); == {connected}");
            return connected; 
        }
        public bool TryConnect()
        {
            log.log("TryConnect();");

            if (client.IsConnected) return true;

            _ = Connect();

            return IsConnected();
        }
        public void Send(Packet packet)
        {
            log.log($"Send({packet.topic},{packet.payload});");
            _ = Publish(packet.topic, packet.payload);
        }
        public void Subscibe(string topic)
        {
            log.log($"Subscribe({topic});");
            _ = SubscribeTo(topic);
        }

        public void Disconnect()
        {
            log.log("Disconnect();");
            client.DisconnectAsync(CancellationToken.None);
        }
    }
}
