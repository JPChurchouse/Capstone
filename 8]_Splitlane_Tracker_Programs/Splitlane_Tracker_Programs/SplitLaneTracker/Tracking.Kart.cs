using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitlaneTracker.Services.Tracking
{
    public class Kart
    {
        public string Colour;
        public string Number;
        public string Name;
        public List<Detection> DetectionList = new List<Detection>();
        public long NextExpectedDetection = 0;

        public Kart()
        {
            Colour = "null";
            Number = "null";
            Name = "null";
        }
        public Kart(string colour, string number, string name = "null")
        { 
            Colour = colour;
            Number = number;
            Name = name;
        }

        public void Detect(Detection detection)
        {
            DetectionList.Add(detection);
            CalculateNextDetection(detection.Time);
        }

        public int[] GetLapCount()
        {
            int[] ints = new int[3] { 0, 0, 0 };
            foreach (Detection det in DetectionList)
            {
                if (det.Lane == "left") ints[0]++;
                else if (det.Lane == "right") ints[1]++;
                else ints[2]++;
            }
            ints[2] += ints[0] + ints[1];
            return ints;
        }


        // Maths
        private long[] LastLapTimes = new long[3];
        private void CalculateNextDetection(long stamp)
        {
            int count = DetectionList.Count;
            int index = count % 3;

            LastLapTimes[index] = stamp;

            // Fewer than 3 laps of data
            if (count < 3 ) 
            { 
                NextExpectedDetection = 0;
                return;
            }

            // Oldest value will be the next on to be overwritten == (index + 1) % 3
            long oldest = LastLapTimes[(index + 1) % 3];
            // Next deteciton will be now plus half the time to do two laps
            NextExpectedDetection = (stamp - oldest) / 2 + stamp;
        }
    }
}
