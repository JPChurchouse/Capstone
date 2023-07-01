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
