﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplitlaneTracker.Services;
using SplitlaneTracker.Services.Tracking;

namespace SplitlaneTracker.Server
{
    #region Ignore
    [System.ComponentModel.DesignerCategory("")]
    public class adam { }
    #endregion
    public partial class GUI : Form
    {
        private Services.Tracking.Race.Race myRace = new Services.Tracking.Race.Race();

        public void Race_New(string json)
        {
            log.log("Initalising new Race");
            log.log(json);
            myRace = new Services.Tracking.Race.Race(json);
            Race_SetStatus(Status.Ready);
        }

        public void Race_Start()
        {
            _ = Mqtt_Send(new Services.Mqtt.Packet("command/detector", "start"));
            Race_SetStatus(Status.Running);
        }
        public void Race_Expirey(int span)
        {

        }
        public void Race_Stop()
        {
            _ = Mqtt_Send(new Services.Mqtt.Packet("command/detector", "stop"));
            Race_SetStatus(Status.Complete);

            string dir = Environment.CurrentDirectory + "\\output";
            Directory.CreateDirectory(dir);
            myRace.ExportToFileAsJson(dir);
            myRace.ExportToFileAsText(dir);
            log.log("Current working dir: " + dir);
        }
        public void Race_Cancel()
        {
            Race_SetStatus(Status.Available);
        }
        public void Race_Detection(string json)
        {
            myRace.AddDetection(new Detection(json));
        }
    }
}
