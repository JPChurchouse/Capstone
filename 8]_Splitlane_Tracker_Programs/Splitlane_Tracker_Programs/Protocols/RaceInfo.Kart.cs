using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XKarts.RaceInfo
{
    public class Kart
    {
        public byte ID, Laps_Left, Laps_Right;
        public Identifier.Colour Colour;

        /// <summary>
        /// Public struct for creating and sharing Kart informaion.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="colour"></param>
        public Kart(byte id, Identifier.Colour colour, byte left = 0, byte right = 0)
        {
            ID = id;
            Colour = colour;
            Laps_Left = left;
            Laps_Right = right;
        }
    }
}
