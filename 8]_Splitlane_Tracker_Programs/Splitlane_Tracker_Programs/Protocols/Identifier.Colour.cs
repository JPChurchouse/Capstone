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

    public struct Colours
    {
        public static List<string> List = new List<string>()
        {
            "0xF00",
            "0xC40",
            "0x880",
            "0x4C0",
            "0x0F0",
            "0x0C4",
            "0x088",
            "0x04C",
            "0x00F",
            "0x40C",
            "0x808",
            "0xC04",
            "0xF00"
        };
    }
}
