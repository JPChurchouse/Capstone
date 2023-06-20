using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace XKarts.Server
{
    public partial class Server
    {
        // Logger
        private static Logging.Logger log = new Logging.Logger();

        // List of active Karts and their info
        private static List<KartStats> KartList = new List<KartStats>();

        // Number of required laps
        private static byte ReqLaps_Left, ReqLaps_Right, ReqLaps_Total;

        public Server()
        {
            try
            {
                log.log("Server Running");
                log.open();
                RunTcpServer();
            }
            catch
            {
                throw;
            }
        }
    }
}
