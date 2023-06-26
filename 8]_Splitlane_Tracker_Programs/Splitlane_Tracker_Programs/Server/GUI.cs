using SplitlaneTracker.Services.Logging;
using SplitlaneTracker.Services.Tracking;
using SplitlaneTracker.Services.Mqtt;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using SplitlaneTracker.Server.Properties;
using SplitlaneTracker.Services.Tracking.Race;
using SplitlaneTracker.Services;

namespace SplitlaneTracker.Server
{
    public partial class GUI : Form
    {
        #region Initalisation

        private static Logger log = new Logger();

        private static RaceInfo RaceInfo;
        private static RaceStatus RaceStatus;
        private static Mqtt MqttClient;
        private static ServerStatus ServerStatus;


        public GUI()
        {
            log.log("Initalising");
            // Initalise the UI first
            InitializeComponent();

            // Establish communications
            log.log("MqttClient");
            MqttClient = new Mqtt(log);
            MqttClient.HandleMqttPacket += ReceivedMqtt;

            // Initalis the server status
            log.log("ServerStatus");
            ServerStatus = new ServerStatus(log);
            ServerStatus.SetStatus(Status.Initalising);
            ServerStatus.SendMqtt += SendMqtt;

            // Initalise the race status
            log.log("RaceStatus");
            RaceStatus = new RaceStatus(log);
            RaceStatus.SetStatus(Status.Initalising);
            RaceStatus.SendMqtt += SendMqtt;

            // Initalise the race info
            log.log("RaceInfo");
            RaceInfo = new RaceInfo(log);
            RaceInfo.SendMqtt += SendMqtt;

            // Availablility
            log.log("Availibility");
            RaceStatus.SetStatus(Status.Available);
            ServerStatus.SetStatus(Status.Available);

            // Hide the window
            SetWindowVisbile(false);

            log.open();
            log.log("Initalisation complete");
        }

        private void SendMqtt(Packet packet)
        {
            MqttClient.Send(packet);
        }

        private void ReceivedMqtt(Packet packet)
        {
            string topic = packet.topic;
            string message = packet.payload;
            log.log($"New MQTT message received: {topic},{message}");

            // Server command
            if (topic.Contains("command/server"))
            {
                log.log("command/server");
                NewServerCommand(message);
            }

            // New race config
            else if (topic.Contains("raceinfo"))
            {
                log.log("raceinfo");
                RaceInfo.New(message);
            }

            // Race command
            else if (topic.Contains("command/race"))
            {
                log.log("command/race");
                NewRaceCommand(message);
            }

            // Detection
            else if (topic.Contains("detect"))
            {
                log.log("detect");
                RaceInfo.Detection(message);
            }

            // Unrecognised
            else
            {
                log.log($"Unable to process message: {topic},{message}");
            }
        }

        private void NewRaceCommand(string command)
        {
            log.log($"Processing new race command: {command}");

            // Start the race with existing race params
            if (command.Contains("start"))
            {
                log.log("NewRaceCommand - start");
                RaceInfo.Start();
                RaceStatus.SetStatus(Status.Running);
            }

            //  Set the expirey of the current race
            else if (command.Contains("expires"))
            {
                log.log("NewRaceCommand - expires");
                string num = command.Substring(command.IndexOf(":") + 1);
                if(Int32.TryParse(num, out int value))
                {
                    RaceInfo.Expirey(value);
                }
            }

            // Conclude the current race
            else if (command.Contains("end"))
            {
                log.log("NewRaceCommand - end");
                RaceInfo.Stop();
                RaceStatus.SetStatus(Status.Complete);
            }

            // Cancel the current race info
            else if (command.Contains("cancel"))
            {
                log.log("NewRaceCommand - cancel");
                RaceInfo.Cancel();
                RaceStatus.SetStatus(Status.Available);
            }

            // Unrecongnised
            else
            {
                log.log($"NewRaceCommand unrecongised - {command}");
            }
        }

        private void NewServerCommand(string command)
        {
            log.log($"Processing new server command: {command}");

            // Start the race with existing race params
            if (command.Contains("terminate"))
            {
                log.log("NewServerCommand - terminate");
                Terminate();
            }

            // Unrecongnised
            else
            {
                log.log($"NewServerCommand unrecongised - {command}");
            }

        }

        private async void Terminate()
        {
            try
            {
                RaceStatus.SetStatus(Status.Error);
                ServerStatus.SetStatus(Status.Error);

                await Task.Delay(300);
            }
            catch { }
            finally 
            { 
                MqttClient.Close();
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
            Terminate();
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
    }
}