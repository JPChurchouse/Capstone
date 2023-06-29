using SplitlaneTracker.Services.Tracking;
using SplitlaneTracker.Services.Mqtt;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using SplitlaneTracker.Server.Properties;
using SplitlaneTracker.Services.Tracking.Race;
using SplitlaneTracker.Services;
using MQTTnet.Adapter;
using System.Threading.Tasks;
using SplitlaneTracker.Services.Logging;

namespace SplitlaneTracker.Server
{
    public partial class GUI : Form
    {
        #region Initalisation

        private static Logger log = new Logger();


        public GUI()
        {
            InitializeComponent();

            _ = Initalise();
        }

        // Initalise server features async
        private async Task Initalise()
        {
            log.log("Initalising");

            // Establish communications
            var result = await Mqtt_Init();

            // Set statuses as available
            Server_SetStatus(Status.Available);
            Race_SetStatus(Status.Available);

            // Hide the window
            SetWindowVisbile(false);

            log.open();
            log.log("Initalisation complete");
            //Mqtt_Client.OpenLog();

            return;
        }
        #endregion

        #region Command Processing

        // New race command processing
        private void NewRaceCommand(string command)
        {
            log.log($"Processing new race command: {command}");

            // Start the race with existing race params
            if (command.Contains("start"))
            {
                log.log("NewRaceCommand - start");
                Race_Start();
            }

            //  Set the expirey of the current race
            else if (command.Contains("expires"))
            {
                log.log("NewRaceCommand - expires");
                string num = command.Substring(command.IndexOf(":") + 1);
                if (Int32.TryParse(num, out int value))
                {
                    Race_Expirey(value);
                }
            }

            // Conclude the current race
            else if (command.Contains("end"))
            {
                log.log("NewRaceCommand - end");
                Race_Stop();
            }

            // Cancel the current race info
            else if (command.Contains("cancel"))
            {
                log.log("NewRaceCommand - cancel");
                Race_Cancel();
            }

            // Unrecongnised
            else
            {
                log.log($"NewRaceCommand unrecongised - {command}");
            }
        }

        // New server command processing
        private void NewServerCommand(string command)
        {
            log.log($"Processing new server command: {command}");

            // Start the race with existing race params
            if (command.Contains("terminate"))
            {
                log.log("NewServerCommand - terminate");
                _ = Terminate();
            }

            // Unrecongnised
            else
            {
                log.log($"NewServerCommand unrecongised - {command}");
            }
        }

        // Terminate the program cleanly by disconnecting first
        private async Task Terminate()
        {
            try
            {
                Server_SetStatus(Status.Error);
                Race_SetStatus(Status.Error);
                await Task.Delay(1000);
            }
            catch { }
            finally
            {
                _ = Mqtt_Close();
            }
        }

        #endregion

        #region Window Visibility
        private void StatusIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SetWindowVisbile(true);
        }

        private void ServerGui_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.log("FormClosing");
            if (e.CloseReason == CloseReason.UserClosing)
            {
                log.log("Reason == user");
                SetWindowVisbile(false);
                e.Cancel = true;
                return;
            }
        }

        private void ServerGui_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ = Terminate();
        }

        private void SetWindowVisbile(bool visible = true)
        {
            if (visible)
            {
                this.Show();
                log.log("Setting Window to Show");
            }
            else
            {
                this.Hide();
                log.log("Setting Window to Hide");
            }
        }
        #endregion

        #region Time
        private long TimeNow()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        #endregion

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            log.open();
        }
    }
}