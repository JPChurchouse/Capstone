using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XKarts.Identifier
{
    public class Kart
    {
        public byte ID;
        public Colour Colour;

        /// <summary>
        /// Public struct for creating and sharing Kart informaion.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="colour"></param>
        public Kart(byte id, Colour colour)
        {
            ID = id;
            Colour = colour;
        }
    }
}
