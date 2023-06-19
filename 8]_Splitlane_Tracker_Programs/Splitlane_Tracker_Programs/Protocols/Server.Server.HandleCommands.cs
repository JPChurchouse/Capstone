﻿using System.Net.Sockets;
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
                    Handle_NewRaceInfo(info);
                    break;

                default: break;
            }

            
        }

        private static void Handle_NewRaceInfo(string info)
        {
            InitRace(info);
        }
    }
}