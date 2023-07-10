using Newtonsoft.Json;
using SplitlaneTracker.Services.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitlaneTracker.Services.Tracking.Race
{
  public class Race
  {
    public List<Kart> KartList = new List<Kart>();
    public int[] RequiredLaps = new int[3] {0,0,0};

    public Race() { }

    public bool InitFromJson(string json)
    {
      Race? rac;

      try
      {
        rac = JsonConvert.DeserializeObject<Race>(json);
        if (rac == null) throw new Exception();
      }
      catch
      {
        return false;
      }
      
      KartList = rac.KartList;
      RequiredLaps = rac.RequiredLaps;

      return true;
    }

    public string GetJson()
    {
      return JsonConvert.SerializeObject(this);
    }
    public void ExportToFileAsJson(string filePath)
    {
      string json = JsonConvert.SerializeObject(this);
      string name = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");

      File.WriteAllText($"{filePath}\\{name}.json", json);
    }
    public void ExportToFileAsText(string filePath)
    {
      try
      {
        string name = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");

        // Title
        string data = "XKarts Palmerston North - Splitlane Tracker end-of-race printout\n\n";

        // Table header
        data += "\t\tKart Number\n";
        data += "Lap";
        foreach (Kart kart in KartList)
        {
          data += $"\t\t{kart.Number}";
        }
        data += "\n";

        // Table info
        int laps = 1;// (RequiredLaps[2] != 0) ? RequiredLaps[2] : RequiredLaps[1] + RequiredLaps[2];

        for (int i = 0; i < laps; i++)
        {
          data += $"{i+1}\t\t";

          foreach (Kart kart in KartList)
          {
            // On the first cycle
            if (i == 0)
            {
              // If the current list's count > current max laps, update
              if (kart.DetectionList.Count > laps)
              {
                laps = kart.DetectionList.Count;
              }
            }

            try
            {
              Detection det = kart.DetectionList.ElementAt(i);
              data += det.Lane;
            }
            catch { }
            data += "\t";
          }
          data += "\n";
        }

        // Footer
        data += "\n";
        data += "Thanks for visiting XKarts!";

        // Write info
        File.WriteAllText($"{filePath}\\{name}.txt", data);
      }
      catch (Exception ex)
      {

      }
    }

    public bool AddDetection(Detection detection)
    {
      try
      {
        foreach(Kart kart in KartList)
        {
          if (kart.Colour == detection.Colour)
          {
            kart.Detect(detection);
            return true;
          }
        }

        // Not found - create new kart
        KartList.Add(new Kart(detection.Colour,"unknown"));
        KartList[KartList.Count - 1].Detect(detection);
      }
      catch
      {
        return false;
      }
      return true;
    }

    private long TimeNow()
    {
      return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    public Kart GetNextExpectedKart()
    {
      Kart kart = new Kart();
      foreach (Kart k in KartList)
      {
        long tim = k.NextExpectedDetection;
        if (tim > 0 && tim > kart.NextExpectedDetection)
        {
          kart = k;
        }
      }
      return kart;
    }

    public string GetDisplayInfoAsJson()
    {
      // Create new list from the karts list and sort it
      List<Kart> input = new List<Kart>(KartList);
      input.OrderBy(x => x.NextExpectedDetection);

      // Create a new output list
      List<KartDisplay> output = new List<KartDisplay>();

      // Iterate over each kart in the sorted list
      foreach (Kart kart in input)
      {
        KartDisplay info = new KartDisplay();

        info.Number  = kart.Number;

        // Calculate how many laps are requred in each lane (and total)
        int[] laps_comp = kart.GetLapCount();
        int[] laps_togo = new int[laps_comp.Length];

        for (int i = 0; i < laps_comp.Length; i++)
        {
          laps_togo[i] = this.RequiredLaps[i] - laps_comp[i];
          if (laps_togo[i] <= 0) laps_togo[i] = 0;
        }

        info.Left  = laps_togo[0].ToString();
        info.Right = laps_togo[1].ToString();
        info.Total = laps_togo[2].ToString();

        output.Add(info);
      }

      return JsonConvert.SerializeObject(output);
    }
  }
  public struct KartDisplay
  {
    public KartDisplay() { }
    public KartDisplay(string num, string l, string r, string t) 
    {
      Number = num;
      Left = l;
      Right = r;
      Total = t;
    }
    public string Number  = "null";
    public string Left  = "00";
    public string Right = "00";
    public string Total = "00";
  }
}
