using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SplitlaneTracker.Server
{
    #region Ignore
    [System.ComponentModel.DesignerCategory("")]
    public class helloooo { }
    #endregion
    public partial class GUI : Form
    {
        private Status Detection_status = Status.Offline;
        private void UpdateDisplay(Services.Tracking.Detection? detection = null)
        {
            // UI thread
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { UpdateDisplay(detection); }));
                return;
            }


            // Redo status labels
            labl_DetectorStatus.Text = Detection_status.ToString();
            labl_DetectorStatus.ForeColor = 
                Detection_status==Status.Online ?
                Color.White : Color.Red;
            
            labl_ServerStatus.Text = Server_status.ToString();
            labl_RaceStatus.Text = Race_status.ToString();
            StatusIcon.Text = Race_status.ToString();


            // Race status colour and icon
            Color color;
            Icon icon;

            switch (Race_status)
            {
                case Status.Initalising:
                    icon = Properties.Resources.black;
                    color = Color.White;
                    break;

                case Status.Online:
                    icon = Properties.Resources.white;
                    color = Color.White;
                    break;

                case Status.Ready:
                    icon = Properties.Resources.yellow;
                    color = Color.Yellow;
                    break;

                case Status.Running:
                    icon = Properties.Resources.green;
                    color = Color.Lime;
                    break;

                case Status.Complete:
                    icon = Properties.Resources.blue;
                    color = Color.DodgerBlue;
                    break;

                case Status.Offline:
                case Status.Error:
                default:
                    icon = Properties.Resources.red;
                    color = Color.Red;
                    break;
            }

            labl_RaceStatus.ForeColor = color;
            StatusIcon.Icon = icon;
            this.Icon = icon;


            // Update race status to the box
            string sub = "";
            string rstat = Race_status.ToString();
            int index = txtbx_Output.Text.IndexOf("\r\n");
            if (index != -1)
            {
                sub = txtbx_Output.Text.Substring(0, index);
            }

            if (!sub.Contains(rstat))
            {
                rstat = DateTime.Now.ToString("HH:mm:ss") + " " + rstat + "\r\n";

                int len = 200;
                string existing = 
                    txtbx_Output.Text.Length > len ?
                    txtbx_Output.Text.Substring(0, len) :
                    txtbx_Output.Text;

                txtbx_Output.Text = rstat + existing;
            }


            // Add detection info
            if (detection != null)
            {
                string new_msg = 
                    DateTime.Now.ToString("HH:mm:ss") + " " + 
                    detection.Colour + " " + detection.Lane + 
                    "\r\n";

                string existing;
                if (txtbx_Detection.Text.Length > 500)
                {
                    existing = txtbx_Detection.Text.Substring(0, 500);
                }
                else
                {
                    existing = txtbx_Detection.Text;
                }
                txtbx_Detection.Text = new_msg + existing;
            }
            
            UpdateRemoteDisplays();
        }
    }
}
