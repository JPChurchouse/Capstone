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
            NumLeft = k.Laps_Left;
            NumRight = k.Laps_Right;
            LastDetect = 0;
        }
        public KartStats(byte id, Colour colour)
        {
            ID = id;
            Colour = colour;
            NumLeft = 0;
            NumRight = 0;
            LastDetect = 0;
        }

        // IDENTIFICATION
        private byte ID;
        private Colour Colour;

        public byte getID() { return ID; }
        public Colour getColour() { return Colour; }

        // LAP COUNTING
        private byte NumLeft, NumRight;
        private ulong LastDetect;

        public byte addLap(Lane lane)
        {
            LastDetect = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds();

            switch (lane)
            {
                case Lane.Left:
                    return ++NumLeft;

                case Lane.Right:
                    return ++NumRight;

                default:
                    LastDetect = 0;
                    throw new ArgumentOutOfRangeException("Invalid Lane");
            }
        }

        public ulong getLastDetect()
        {
            return LastDetect;
        }
        public byte getNumLaps(Lane lane = Lane.Total)
        {
            switch (lane)
            {
                case Lane.Total:
                    return (byte)(NumLeft + NumRight);

                case Lane.Left:
                    return NumLeft;

                case Lane.Right:
                    return NumRight;

                default:
                    throw new ArgumentOutOfRangeException("Invalid Lane");
            }
        }

        public string getStats()
        {
            // ID,Colour,Left,Right,Time
            return $"{ID},{Colour},{NumLeft},{NumRight},{LastDetect}";
        }

        public RaceInfo.Kart getRaceInfoKart()
        {
            RaceInfo.Kart kart = 
                new RaceInfo.Kart(
                    ID, 
                    Colour,
                    NumLeft, 
                    NumRight);

            return kart;
        }
    }
}
