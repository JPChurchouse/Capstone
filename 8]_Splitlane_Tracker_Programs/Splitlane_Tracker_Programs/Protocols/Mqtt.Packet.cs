using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XKarts.Mqtt
{
    public struct Packet
    {
        public Packet(string tpc, string msg)
        {
            payload = msg;
            topic = tpc;
        }
        public string topic, payload;
    }
}
