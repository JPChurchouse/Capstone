using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitLaneTracker.Services.Tracking
{
    public class Kart
    {
        public string Colour;
        public ushort Number;
        public List<Detection> DetectionList = new List<Detection>();
        public Kart(string colour, ushort number) 
        { 
            Colour = colour;
            Number = number;
        }

        public void Detect(Detection detection)
        {
            DetectionList.Add(detection);
        }
    }
}
