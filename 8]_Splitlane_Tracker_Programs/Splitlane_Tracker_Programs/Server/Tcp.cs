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

    private void UpdateRemoteDisplays()
    {
      string info = myRace.GetDisplayInfoAsJson();
      log.log(info);
      _ = Mqtt_Client.Publish("display", info);
    }
  }
}
