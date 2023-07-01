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
        public long Time;
        public string Colour;
        public string Lane;
        public string TimeReadable = "null";

        public Detection() 
        {
            Time = 0;
            Colour = "null";
            Lane = "null";
            TimeReadable = "null";
        }
        public Detection(long time, string colour, string lane) 
        {
            Time = time;
            Colour = colour;
            Lane = lane;

            ConvertTimeToDate();
        }
        public Detection(string json) 
        {
            Detection det = JsonConvert.DeserializeObject<Detection>(json)??
                new Detection(0,"err","err");

            this.Time = det.Time;
            this.Colour = det.Colour;
            this.Lane = det.Lane;
            this.TimeReadable = det.TimeReadable;

            ConvertTimeToDate();
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
