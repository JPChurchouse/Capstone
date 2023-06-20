using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XKarts.Identifier;

namespace XKarts.Server
{
    // Kart info for server
    public class KartStats
    {
        public KartStats(RaceInfo.Kart k)
        {
            ID = k.ID;
            Colour = k.Colour;
            CountLap_Left = k.Laps_Left;
            CountLap_Right = k.Laps_Right;
            LastDetect = 0;
        }
        public KartStats(byte id, Colour colour)
        {
            ID = id;
            Colour = colour;
            CountLap_Left = 0;
            CountLap_Right = 0;
            LastDetect = 0;
        }

        // IDENTIFICATION
        private byte ID;
        private Colour Colour;

        public byte getID() { return ID; }
        public Colour getColour() { return Colour; }

        // LAP COUNTING
        private byte CountLap_Left, CountLap_Right;
        private ulong LastDetect;

        public byte addLap(Lane lane)
        {
            LastDetect = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds();

            switch (lane)
            {
                case Lane.Left:
                    return ++CountLap_Left;

                case Lane.Right:
                    return ++CountLap_Right;

                default:
                    LastDetect = 0;
                    throw new ArgumentOutOfRangeException("Invalid Lane");
            }
        }

        public ulong getLastDetect()
        {
            return LastDetect;
        }

        public byte getLapCount(Lane lane = Lane.Total)
        {
            switch (lane)
            {
                case Lane.Total:
                    return (byte)(CountLap_Left + CountLap_Right);

                case Lane.Left:
                    return CountLap_Left;

                case Lane.Right:
                    return CountLap_Right;

                default:
                    throw new ArgumentOutOfRangeException("Invalid Lane");
            }
        }

        public string getStats()
        {
            // ID,Colour,Left,Right,Time
            return 
                $"{ID}," +
                $"{Colour}," +
                $"{CountLap_Left}," +
                $"{CountLap_Right}," +
                $"{LastDetect}";
        }

        public RaceInfo.Kart getRaceInfoKart()
        {
            RaceInfo.Kart kart = 
                new RaceInfo.Kart(
                    ID, 
                    Colour,
                    CountLap_Left, 
                    CountLap_Right);

            return kart;
        }
    }
}
