using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplitlaneTracker.Services;
using SplitlaneTracker.Services.Tracking;
using SplitlaneTracker.Services.Tracking.Race;

namespace SplitlaneTracker.Server
{
  #region Ignore
  [System.ComponentModel.DesignerCategory("")]
  public class adam { }
  #endregion
  public partial class GUI : Form
  {
    private Race myRace = new Race();

    public void Race_New(string json = "")
    {
      log.log("Initalising new Race");
      log.log(json);

      bool ready = false;

      if (json == "" || json == null)
      {
        myRace.InitBlank(
          Properties.Settings.Default.Laps_Left,
          Properties.Settings.Default.Laps_Right,
          Properties.Settings.Default.Laps_Total);
        ready = true;
      }
      else
      {
        ready = myRace.InitFromJson(json);
      }

      Race_SetStatus(ready ? Status.Ready : Status.Online);
    }

    public void Race_Start()
    {
      _ = Mqtt_Send(new Services.Mqtt.Packet("detect/cmd", "start"));
      Race_SetStatus(Status.Running);
    }
    public void Race_Expirey(int span)
    {

    }
    public void Race_Stop()
    {
      _ = Mqtt_Send(new Services.Mqtt.Packet("detect/cmd", "stop"));

      if (Race_status != Status.Running) return;

      Race_SetStatus(Status.Complete);

      string dir = Environment.CurrentDirectory + "\\output";
      Directory.CreateDirectory(dir);
      myRace.ExportToFileAsJson(dir);
      myRace.ExportToFileAsText(dir);
      log.log("Current working dir: " + dir);
    }
    public void Race_Cancel()
    {
      Race_SetStatus(Status.Online);
    }
    public void Race_Detection(string json)
    {
      if (Race_status != Status.Running) return;

      Detection det = new Detection();

      if (det.InitFromJson(json))
      {
        myRace.AddDetection(det);
      }
      else
      {
        det.Colour = "Parsing Error";
        det.Lane = "";
      }

      UpdateDisplay(det);
    }
  }
}
