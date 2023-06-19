using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XKarts.Server
{
    public partial class Server
    {
        public static void InitRace(string json)
        {
            log.log("Init race");
            Creator.Race import_info = new Creator.Race(Json: json);

            // Import Karts from creator format to server format
            KartList.Clear();
            foreach (var kart in import_info.KartList)
            {
                KartList.Add(new KartStats(kart));
            }

            Laps_Left = import_info.Laps_Left;
            Laps_Right = import_info.Laps_Right;
            Laps_Total = import_info.Laps_Total;

            log.log("Init race complete");


            log.log($"\nRace info as follows:\n" +
                $"Numer laps L R T: {Laps_Left} {Laps_Right} {Laps_Total}\n");
            foreach(KartStats kart in KartList)
            {
                log.log($"KartInfo ID: {kart.getID()} Colour: {kart.getColour()}\n");
            }
            log.log("-Ends\n");

        }
    }
}
