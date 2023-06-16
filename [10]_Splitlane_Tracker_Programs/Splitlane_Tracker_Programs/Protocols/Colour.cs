using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XKarts.Identifier
{
    public enum Colour : ulong
    {
        None    = 0x000000,
        Red     = 0xFF0000,
        Orange  = 0xFF8000,
        Yellow  = 0xFFFF00,
        Lime    = 0x80FF00,
        Green   = 0x00FF00,
        Spring  = 0x00FF80,
        Cyan    = 0x00FFFF,
        Sky     = 0x0080FF,
        Blue    = 0x0000FF,
        Purple  = 0x8000FF,
        Magenta = 0xFF00FF,
        Pink    = 0xFF0080,
        White   = 0xFFFFFF
    }
}
