using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplitlaneTracker.Services;

namespace SplitlaneTracker.Server
{
    #region Ignore
    [System.ComponentModel.DesignerCategory("")]
    public class bob { }
    #endregion
    public partial class GUI : Form
    {
        System.Timers.Timer Race_Timer = new System.Timers.Timer(60000);
        private void Race_Timer_Start()
        {
            log.log("RaceInfo timer starting");
            Race_Timer_Stop();
            Race_Timer.Interval = 30000;
            Race_Timer.Start();
            Race_Timer.Elapsed += Race_Timer_Stop;
        }
        private void Race_Timer_Stop(object sender, System.Timers.ElapsedEventArgs e)
        {
            log.log("RaceInfo timeout");
            Race_Timer_Stop();
            Race_SetStatus(Status.Available);
        }

        private void Race_Timer_Stop()
        {
            Race_Timer.Stop();
        }

        

        private Status Race_status = Status.Initalising;
        private void Race_SetStatus(Status stat)
        {
            Race_Timer_Start();

            Race_status = stat;
            log.log($"RaceStatus status = {Race_status}");

            string statusupdate = "initalising";

            switch (Race_status)
            {
                case Status.Initalising:
                    statusupdate = "initalising";
                    break;

                case Status.Available:
                    statusupdate = "available";
                    break;

                case Status.Ready:
                    statusupdate = "ready";
                    Race_Timer_Start();
                    break;

                case Status.Running:
                    statusupdate = "running";
                    break;

                case Status.Complete:
                    statusupdate = "complete";
                    Race_Timer_Start();
                    break;

                case Status.Error:
                default:
                    statusupdate = "error";
                    break;
            }

            _ = Mqtt_Send(new Services.Mqtt.Packet("status/race", $"{statusupdate}"));
        }
    }
}
