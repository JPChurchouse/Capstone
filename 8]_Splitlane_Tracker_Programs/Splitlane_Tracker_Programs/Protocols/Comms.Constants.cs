using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Net;

namespace XKarts.Comms
{
    public static class Constants 
    { 
        public static int PortNum = 7070;

        public static IPAddress IpAddress = new IPAddress(new byte[] { 192, 168, 1, 115 });
        //public static IPAddress IpAddress = new IPAddress(new byte[] { 192, 168, 1, 230 });
    }
}
