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

    public void Race_New(string json)
    {
      log.log("Initalising new Race");
      log.log(json);

      if (myRace.InitFromJson(json))
      {
        Race_SetStatus(Status.Ready);
      }
      else
      {
        Race_SetStatus(Status.Online);
      }
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
      Race_SetStatus(Status.Online);
    }
    public void Race_Detection(string json)
    {
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
