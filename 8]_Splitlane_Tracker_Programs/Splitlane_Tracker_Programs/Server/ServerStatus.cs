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
    public class asdf { }
    #endregion
    public partial class GUI : Form
    {

        private Status Server_status = Status.Initalising;
        private void Server_SetStatus(Status stat)
        {
            Server_status = stat;
            log.log($"ServerStatus status = {Server_status}");

            string statusupdate = "initalising";

            switch (Server_status)
            {
                case Status.Initalising:
                    statusupdate = "initalising";
                    break;

                case Status.Available:
                    statusupdate = "available";
                    break;

                case Status.Error:
                default:
                    statusupdate = "offline";
                    break;
            }

            _ = Mqtt_Send(new Services.Mqtt.Packet("status/server", $"{statusupdate}"));
        }
    }
}
