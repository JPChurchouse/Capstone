using System.Security.Cryptography;
using XKarts;

namespace Server
{
    public partial class Server
    {
        public Server()
        {
            log.log("Server init");
            ListenTCP();
        }

        public void InitRace(string json)
        {
            log.log("Init race");
            XKarts.Creator.Race import_info = new XKarts.Creator.Race(Json: json);

            // Import Karts from creator format to server format
            KartList.Clear();
            foreach (var kart in import_info.KartList)
            {
                KartList.Add(new XKarts.Server.KartStats(kart));
            }

            Laps_Left = import_info.Laps_Left;
            Laps_Right = import_info.Laps_Right;
            Laps_Total = import_info.Laps_Total;

            log.log("Init race complete");
        }

    }
}