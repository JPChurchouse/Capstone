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
  public class bob { }
  #endregion
  public partial class GUI : Form
  {
    System.Timers.Timer Race_Timer = new System.Timers.Timer(60000);
    private void Race_Timer_Start()
    {
      log.log("RaceInfo timer starting");
      Race_Timer_Stop();
      Race_Timer.Interval = 30000;
      Race_Timer.Start();
      Race_Timer.Elapsed += Race_Timer_Elapsed;
    }
    private void Race_Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      log.log("RaceInfo timeout");
      Race_Timer_Stop();
      Race_SetStatus(Status.Online);
    }

    private void Race_Timer_Stop()
    {
      Race_Timer.Stop();
    }


    private Status Race_status = Status.Initalising;
    private void Race_SetStatus(Status stat)
    {
      Race_Timer_Stop();

      if (stat == Race_status) return; // Already set to this status

      Race_status = stat;
      log.log($"RaceStatus status = {Race_status}");

      switch (Race_status)
      {
        case Status.Initalising:
          break;

        case Status.Online:
          break;

        case Status.Ready:
          Race_Timer_Start();
          break;

        case Status.Running:
          break;

        case Status.Complete:
          Race_Timer_Start();
          break;

        case Status.Offline:
        case Status.Error:
        default:
          break;
      }

      _ = Mqtt_Send(new Services.Mqtt.Packet("race/status", $"{Race_status}"));
      
      UpdateDisplay();
    }
  }
}
