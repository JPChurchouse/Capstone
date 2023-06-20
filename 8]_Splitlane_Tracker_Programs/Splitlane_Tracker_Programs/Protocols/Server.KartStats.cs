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
        public KartStats(Kart k)
        {
            ID = k.ID;
            Colour = k.Colour;
        }
        public KartStats(byte id, Colour colour)
        {
            ID = id;
            Colour = colour;
        }

        // Identity
        private byte ID;
        private Colour Colour;

        public byte getID() { return ID; }
        public Colour getColour() { return Colour; }

        // Counting
        private byte NumLeft = 0;
        private byte NumRight = 0;
        private ulong LastDetect = 0;

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
    }
}
