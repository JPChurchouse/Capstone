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
        private byte LaneLeft = 0;
        private byte LaneRight = 0;
        private ulong Last = 0;

        public byte addLap(Lane lane)
        {
            Last = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds();

            switch (lane)
            {
                case Lane.Left:
                    return ++LaneLeft;

                case Lane.Right:
                    return ++LaneRight;

                default:
                    Last = 0;
                    throw new ArgumentOutOfRangeException("Invalid Lane");
            }
        }

        public ulong getLastTime()
        {
            return Last;
        }
        public byte getNumLaps(Lane lane = Lane.Total)
        {
            switch (lane)
            {
                case Lane.Total:
                    return (byte)(LaneLeft + LaneRight);

                case Lane.Left:
                    return LaneLeft;

                case Lane.Right:
                    return LaneRight;

                default:
                    throw new ArgumentOutOfRangeException("Invalid Lane");
            }
        }
    }
}
