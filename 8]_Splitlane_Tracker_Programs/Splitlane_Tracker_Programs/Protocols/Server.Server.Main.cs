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
        private List<KartStats> KartList = new List<KartStats>();

        // Number of required laps
        private byte Laps_Left, Laps_Right, Laps_Total;

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
