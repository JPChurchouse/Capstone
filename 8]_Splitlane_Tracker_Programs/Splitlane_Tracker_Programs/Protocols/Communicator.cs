using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Http;


namespace XKarts.Comms
{
    public struct Parameters
    {
        public string WhoAmI { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
    }

    public class Communicator
    {
        Communicator(Parameters @params, bool keep_open = false)
        {
            Params = @params;
            WebSocket = keep_open;
        }
        ~Communicator()
        {

        }

        private Parameters Params;
        private bool WebSocket;



    }
}
