using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplitlaneTracker.Services;

namespace SplitlaneTracker.Server
{
    internal class ServerStatus
    {
        // OnReceived user handler
        public delegate void SendMqttHandler(Services.Mqtt.Packet packet);
        public event SendMqttHandler? SendMqtt;

        private static Services.Logging.Logger log;

        public ServerStatus(Services.Logging.Logger l)
        {
            log = l;
        }

        private Status status = Status.Initalising;
        public void SetStatus(Status stat)
        {
            status = stat;
            log.log($"ServerStatus status = {status}");

            string statusupdate = "online";

            switch (status)
            {
                case Status.Initalising:
                    statusupdate = "initalising";
                    break;

                case Status.Available:
                    break;

                case Status.Error:
                default:
                    statusupdate = "offline";
                    break;
            }

            SendMqtt?.Invoke(new Services.Mqtt.Packet("status/server", $"{statusupdate}"));
        }
    }
}
