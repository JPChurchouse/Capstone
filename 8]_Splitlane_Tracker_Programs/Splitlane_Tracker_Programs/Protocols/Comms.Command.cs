using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XKarts.Comms
{
    public enum Command
    {
        NewRaceInfo,
        StopRace,
        ClearRaceInfo,
        KartDetected
    }
    public enum Request
    {
        RaceStatsAll,
        RaceStatsSingle,
        Display
    }
}
