using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XKarts
{
    namespace Identifier
    {
        /// <summary>
        /// Public struct for creating and sharing Kart informaion.
        /// Holds: ID number, Beacon Colour.
        /// </summary>
        public struct Kart
        {
            public byte ID;
            public Colour Colour;

            public Kart(byte id, Colour colour)
            {
                ID = id;
                Colour = colour;
            }
        }
    }
}
