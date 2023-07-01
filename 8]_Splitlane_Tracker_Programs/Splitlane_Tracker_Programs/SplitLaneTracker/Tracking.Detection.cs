using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SplitlaneTracker.Services.Tracking
{
    public class Detection
    {
        public long Time = 0;
        public string Colour = "null";
        public string Lane = "null";
        public string TimeReadable = "null";

        public Detection() { }

        public void InitFromInfo(long time, string colour, string lane) 
        {
            Time = time;
            Colour = colour;
            Lane = lane;

            ConvertTimeToDate();
        }
        public bool InitFromJson(string json)
        {
            Detection? det;
            try
            {
                det = JsonConvert.DeserializeObject<Detection>(json);
                if (det == null) throw new Exception();
            }
            catch
            {
                return false;
            }

            this.Time = det.Time;
            this.Colour = det.Colour;
            this.Lane = det.Lane;
            this.TimeReadable = det.TimeReadable;

            ConvertTimeToDate();

            return true;
        }
        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        private void ConvertTimeToDate()
        {
            if (Time == 0) return;

            var time = DateTimeOffset.FromUnixTimeMilliseconds(Time);
            TimeReadable = time.ToLocalTime().ToString("HH:mm:ss.fff");
        }
    }
}
