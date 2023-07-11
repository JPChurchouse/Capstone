using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitlaneTracker.Server
{
  #region Ignore
  [System.ComponentModel.DesignerCategory("")]
  public class qwerty { }
  #endregion
  public partial class GUI : Form
  {
    private static Services.Tcp.TcpServer TcpServer =
      new Services.Tcp.TcpServer(
      Properties.Settings.Default.MqttBrokerAddress,
      Environment.CurrentDirectory + "\\RaceDisplayPage.html", 
      log);

    private async Task UpdateRemoteDisplays()
    {
      string info = "[{\"Number\": \"Waiting\",\"Left\": \"for\",\"Right\": \"new\",\"Total\": \"race\"}]";

      if (Race_status == Status.Running || Race_status == Status.Ready) 
        info = myRace.GetDisplayInfoAsJson();

      log.log(info);
      await Mqtt_Client.Publish("display", info);
    }
  }
}
