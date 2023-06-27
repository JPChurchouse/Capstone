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
    public class asdff { }
    #endregion
    public partial class GUI : Form
    {
        enum Status
        {
            Initalising,
            Available,
            Ready,
            Running,
            Error,
            Complete
        }

        private void UpdateGui(Status status)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { UpdateGui(status); }));
                return;
            }
            string info;
            Color color;

            switch (status)
            {
                case Status.Initalising:
                    StatusIcon.Icon = Properties.Resources.black;
                    this.Icon = Properties.Resources.black;
                    StatusIcon.Text = "Initalising...";
                    color = Color.White;
                    info = "Initalising";
                    break;

                case Status.Available:
                    StatusIcon.Icon = Properties.Resources.white;
                    this.Icon = Properties.Resources.white;
                    StatusIcon.Text = "Available";
                    color = Color.White;
                    info = "Available";
                    break;

                case Status.Ready:
                    StatusIcon.Icon = Properties.Resources.yellow;
                    this.Icon = Properties.Resources.yellow;
                    StatusIcon.Text = "Ready";
                    color = Color.Yellow;
                    info = "Ready";
                    break;

                case Status.Running:
                    StatusIcon.Icon = Properties.Resources.green;
                    this.Icon= Properties.Resources.green;
                    StatusIcon.Text = "Running";
                    color = Color.Lime;
                    info = "Running";
                    break;

                case Status.Complete:
                    StatusIcon.Icon = Properties.Resources.blue;
                    this.Icon = Properties.Resources.blue;
                    StatusIcon.Text = "Complete";
                    color = Color.Blue;
                    info = "Complete";
                    break;

                case Status.Error:
                
                default:
                    StatusIcon.Icon = Properties.Resources.red;
                    this.Icon = Properties.Resources.red;
                    StatusIcon.Text = "Error";
                    color = Color.Red;
                    info = "Error";
                    break;
            }
            
            labl_RaceStatus.Text    = "Race  : " + info;
            labl_RaceStatus.ForeColor = color;

            info = DateTime.Now.ToString("HH:mm:ss") + " " + info + "\r\n";
            string existing;
            if (txtbx_Output.Text.Length > 500)
            {
                existing = txtbx_Output.Text.Substring(0, 500);
            }
            else
            {
                existing = txtbx_Output.Text;
            }
            if (existing.Contains(info)) { return; }
            txtbx_Output.Text = info + existing;
        }
    }
}
