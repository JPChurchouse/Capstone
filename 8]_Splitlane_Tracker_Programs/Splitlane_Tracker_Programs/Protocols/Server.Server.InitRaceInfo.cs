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
        public static void ImportRace(string json)
        {
            log.log("Init race");
            RaceInfo.Race import_info = new RaceInfo.Race(json: json);

            // Import Karts from creator format to server format
            KartList.Clear();
            foreach (var kart in import_info.KartList)
            {
                KartList.Add(new KartStats(kart));
            }

            ReqLaps_Left = import_info.ReqLaps_Left;
            ReqLaps_Right = import_info.ReqLaps_Right;
            ReqLaps_Total = import_info.ReqLaps_Total;

            log.log("Init race complete");


            log.log($"\nRace info as follows:");
            log.log($"Numer laps L R T: {ReqLaps_Left} {ReqLaps_Right} {ReqLaps_Total}");
            foreach(KartStats kart in KartList)
            {
                log.log($"KartInfo ID: {kart.getID()} Colour: {kart.getColour()}");
            }
            log.log("---\n");

            UpdateSubscribers();
        }

        public static string ExportRace()
        {
            List<RaceInfo.Kart> myList = new List<RaceInfo.Kart>();
            
            foreach (var kart in KartList)
            {
                RaceInfo.Kart myKart = 
                    new RaceInfo.Kart(
                        kart.getID(),
                        kart.getColour(),
                        kart.getNumLaps(Identifier.Lane.Left),
                        kart.getNumLaps(Identifier.Lane.Right)
                    );
                
                myList.Add(myKart);
            }

            RaceInfo.Race myRace = 
                new RaceInfo.Race(
                    myList,
                    ReqLaps_Left,
                    ReqLaps_Right,
                    ReqLaps_Total
                );

            return myRace.GenerateJsonString();
        }
    }
}
