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
        private string Detection_status;
        private void UpdateDisplay(Services.Tracking.Detection? detection = null)
        {
            // UI thread
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { UpdateDisplay(detection); }));
                return;
            }

            // Redo status labels
            labl_DetectorStatus.Text = Detection_status;
            labl_ServerStatus.Text = Server_status.ToString();

            // Race status
            string race_msg;
            Color race_col;

            switch (Race_status)
            {
                case Status.Initalising:
                    StatusIcon.Icon = Properties.Resources.black;
                    this.Icon = Properties.Resources.black;
                    StatusIcon.Text = "Initalising...";
                    race_col = Color.White;
                    race_msg = "Initalising";
                    break;

                case Status.Available:
                    StatusIcon.Icon = Properties.Resources.white;
                    this.Icon = Properties.Resources.white;
                    StatusIcon.Text = "Available";
                    race_col = Color.White;
                    race_msg = "Available";
                    break;

                case Status.Ready:
                    StatusIcon.Icon = Properties.Resources.yellow;
                    this.Icon = Properties.Resources.yellow;
                    StatusIcon.Text = "Ready";
                    race_col = Color.Yellow;
                    race_msg = "Ready";
                    break;

                case Status.Running:
                    StatusIcon.Icon = Properties.Resources.green;
                    this.Icon = Properties.Resources.green;
                    StatusIcon.Text = "Running";
                    race_col = Color.Lime;
                    race_msg = "Running";
                    break;

                case Status.Complete:
                    StatusIcon.Icon = Properties.Resources.blue;
                    this.Icon = Properties.Resources.blue;
                    StatusIcon.Text = "Complete";
                    race_col = Color.Blue;
                    race_msg = "Complete";
                    break;

                case Status.Error:

                default:
                    StatusIcon.Icon = Properties.Resources.red;
                    this.Icon = Properties.Resources.red;
                    StatusIcon.Text = "Error";
                    race_col = Color.Red;
                    race_msg = "Error";
                    break;
            }

            if (labl_RaceStatus.Text != race_msg)
            {
                labl_RaceStatus.Text = race_msg;
                labl_RaceStatus.ForeColor = race_col;

                race_msg = DateTime.Now.ToString("HH:mm:ss") + " " + race_msg + "\r\n";
                string existing;
                if (txtbx_Output.Text.Length > 500)
                {
                    existing = txtbx_Output.Text.Substring(0, 500);
                }
                else
                {
                    existing = txtbx_Output.Text;
                }
                if (existing.Contains(race_msg)) { return; }
                txtbx_Output.Text = race_msg + existing;
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
        }
    }
}
