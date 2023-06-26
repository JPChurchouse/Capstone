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
        public Race(string json)
        {
            Race det = JsonConvert.DeserializeObject<Race>(json) ?? new Race();
            KartList = det.KartList;
            RequiredLaps = det.RequiredLaps;
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public void ExportToFileAsJson(string filePath)
        {
            string json = JsonConvert.SerializeObject(this);
            string name = DateTime.Now.ToString("yyyy/MM/dd_HH:mm");

            File.WriteAllText($"{filePath}\\{name}.json", json);
        }
        public void ExportToFileAsText(string filePath)
        {
            string name = DateTime.Now.ToString("yyyy/MM/dd_HH:mm");

            // Title
            string data = "XKarts Palmerston North - Splitlane Tracker end-of-race printout\n";
            
            // Table header
            data += "Lap";
            foreach (Kart kart in KartList)
            {
                data += $"\t{kart.Number}";
            }
            data += "\n";

            // Table info
            int laps = (RequiredLaps[2] != 0) ? RequiredLaps[2] : RequiredLaps[1] + RequiredLaps[2];

            for (int i = 0; i < laps; i++)
            {
                foreach (Kart kart in KartList)
                {
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
            File.WriteAllText($"{filePath}\\{name}.json", data);
        }

        public bool AddDetection(Detection detection)
        {
            try
            {
                foreach(Kart kart in KartList)
                {
                    if (kart.Colour == detection.Colour)
                    {
                        kart.DetectionList.Add(detection);
                        break;
                    }
                }
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
    }
}
