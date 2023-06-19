using System.Net.Sockets;
using System.Net;
using XKarts;
using System.Text;
using Serilog;

namespace XKarts.Server
{
    public partial class Server
    {
        static string HandleGet(string page, string info)
        {
            log.log($"HandleGet: {info}");
            return "No information to show";
        }

        static void HandlePut(string page, string info)
        {
            log.log($"HandlePut: {info}");


            Comms.Command command = (Comms.Command) Enum.Parse(typeof(Comms.Command), info);

            switch (command)
            {
                case Comms.Command.NewRaceInfo:
                    Handle_NewRaceInfo(info);
                    break;

                default: break;
            }

            
        }

        private static void Handle_NewRaceInfo(string info)
        {


        }
    }
}