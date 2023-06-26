using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplitlaneTracker.Services;
using SplitlaneTracker.Services.Tracking;

namespace SplitlaneTracker.Server
{
    internal class RaceInfo
    {
        // OnReceived user handler
        public delegate void SendMqttHandler(Services.Mqtt.Packet packet);
        public event SendMqttHandler? SendMqtt;

        private static Services.Logging.Logger log;

        public RaceInfo(Services.Logging.Logger l)
        {
            log = l;
        }


        private Services.Tracking.Race.Race myRace = new Services.Tracking.Race.Race();

        public void New(string json)
        {
            log.log("Initalising new RaceInfo");
            myRace = new Services.Tracking.Race.Race(json);
        }

        public void Start()
        {

        }
        public void Expirey(int span)
        {

        }
        public void Stop()
        {

        }
        public void Cancel()
        {

        }
        public void Detection(string json)
        {

        }
    }
}
