using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplitlaneTracker.Services;

namespace SplitlaneTracker.Server
{
    internal class RaceStatus
    {
        // OnReceived user handler
        public delegate void SendMqttHandler(Services.Mqtt.Packet packet);
        public event SendMqttHandler? SendMqtt;

        private static Services.Logging.Logger log;

        public RaceStatus(Services.Logging.Logger l)
        {
            log = l;
        }


        System.Timers.Timer timer = new System.Timers.Timer(60000);
        private void StartTimer()
        {
            log.log("RaceInfo timer starting");
            timer.Stop();
            timer.Interval = 30000;
            timer.Start();
            timer.Elapsed += Timer_Elapsed;
        }
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            log.log("RaceInfo timeout");
            timer.Stop();
            SetStatus(Status.Available);
        }

        private void StopTimer()
        {
            timer.Stop();
        }

        

        private Status status = Status.Initalising;
        public void SetStatus(Status stat)
        {
            StopTimer();

            status = stat;
            log.log($"RaceStatus status = {status}");

            string statusupdate = "available";

            switch (status)
            {
                case Status.Initalising:
                    statusupdate = "initalising";
                    break;

                case Status.Available:
                    statusupdate = "available";
                    break;

                case Status.Ready:
                    statusupdate = "ready";
                    StartTimer();
                    break;

                case Status.Running:
                    statusupdate = "running";
                    break;

                case Status.Complete:
                    statusupdate = "complete";
                    StartTimer();
                    break;

                case Status.Error:
                default:
                    statusupdate = "error";
                    break;
            }

            SendMqtt?.Invoke(new Services.Mqtt.Packet("status/race", $"{statusupdate}"));
        }
        public Status GetStatus() { return status; }
    }
}
