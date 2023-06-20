using System.Net.Sockets;
using System.Net;
using XKarts;
using System.Text;
using Serilog;

namespace XKarts.Server
{
    public partial class Server
    {
        #region GET
        static string HandleGet(string page, string info)
        {
            page = page.Replace("/", "");
            log.log($"HandleGet: Page-{page} Info-{info}");
            Comms.Request command = (Comms.Request) Enum.Parse(typeof(Comms.Request), page);

            string ret = "Not implemented";

            switch (command)
            {
                case Comms.Request.RaceStatsAll:
                    log.log("Case: RaceStatsAll");
                    ret = GetRaceStats();
                    break;

                case Comms.Request.RaceStatsSingle:
                    log.log("Case: RaceStatsSingle");
                    ret = GetRaceStats(info);
                    break;

                case Comms.Request.Display:
                    log.log("Case: Display");
                    break;

                default:
                    log.log("Default");
                    break;
            }

            return ret;
        }

        private static string GetRaceStats(string? info = null)
        {
            if (info != null)
            {
                Identifier.Colour colour;
                UInt16 ID;

                // Try to get Colour
                if (Enum.TryParse(info, out colour))
                {
                    foreach (KartStats kart in KartList)
                    {
                        if (kart.getColour() == colour)
                        {
                            return kart.getStats();
                        }
                    }
                }

                // Try to get ID
                else if (UInt16.TryParse(info, out ID))
                {
                    foreach (KartStats kart in KartList)
                    {
                        if (kart.getID() == ID)
                        {
                            return kart.getStats();
                        }
                    }
                }

                // Not found
                return "Unable to find information";
            }

            string ret = $"{Laps_Left},{Laps_Right},{Laps_Total}";
            foreach (KartStats kart in KartList)
            {
                ret += $"\r\n{kart.getStats()}";
            }

            return ret;
        }

        #endregion

        #region POST
        static void HandlePost(string page, string info)
        {
            page = page.Replace("/", "");
            log.log($"HandlePut: Page-{page} Info-{info}");
            Comms.Command command = (Comms.Command) Enum.Parse(typeof(Comms.Command), page);

            switch (command)
            {
                case Comms.Command.NewRaceInfo:
                    log.log("Case: NewRaceInfo");
                    InitRace(info);
                    break;

                case Comms.Command.StopRace:
                    log.log("Case: StopRace");
                    break;

                case Comms.Command.ClearRaceInfo:
                    log.log("Case: ClearRaceInfo");
                    break;

                case Comms.Command.KartDetected:
                    log.log("Case: KartDetected");
                    KartDetected(info);
                    break;

                default:
                    break;
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
        #endregion
    }
}