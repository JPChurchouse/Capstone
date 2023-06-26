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
        public Kart(string colour, string number, string name) 
        { 
            Colour = colour;
            Number = number;
            Name = name;
        }

        public void Detect(Detection detection)
        {
            DetectionList.Add(detection);
        }
    }
}
