using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitlaneTracker.Services.Mqtt
{
    public struct Packet
    {
        public string topic, payload;
        public Packet(string tpc, string msg)
        {
            payload = msg;
            topic = tpc;
        }
    }
}
