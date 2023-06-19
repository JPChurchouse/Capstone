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
            
            page = page.Replace("/", "");

            log.log($"HandlePut: Page-{page} Info-{info}");

            Comms.Command command = (Comms.Command) Enum.Parse(typeof(Comms.Command), page);

            switch (command)
            {
                case Comms.Command.NewRaceInfo:
                    log.log("NewRaceInfo");
                    InitRace(info);
                    break;
                case Comms.Command.StopRace:
                    log.log("StopRace");
                    break;
                case Comms.Command.KartDetected:
                    log.log("KartDetected");
                    KartDetected(info);
                    break;

                default: break;
            }

            
        }

        private static void KartDetected(string info)
        {
            string[] info_split = info.Split(' ');
            string _colour = info_split[0];
            string _lane = info_split[1];

            Identifier.Colour colour = (Identifier.Colour) Enum.Parse(typeof(Identifier.Colour), _colour);
            Identifier.Lane lane = (Identifier.Lane) Enum.Parse(typeof(Identifier.Lane), _lane);

            foreach (KartStats kart in KartList)
            {
                if (kart.getColour() == colour)
                {
                    kart.addLap(lane);
                    break;
                }
            }
        }
    }
}