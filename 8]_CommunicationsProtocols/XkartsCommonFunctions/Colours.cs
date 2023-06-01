using Newtonsoft.Json;

namespace XkartsCommonFunctions
{
    public static class Colours
    {
        public static void testfunc()
        {
            Console.WriteLine("TESTING");
        }
        private static string file_colours = "ColoursList.json";
        public struct Colour
        {
            public UInt16 Red { get; set; }
            public UInt16 Green { get; set; }
            public UInt16 Blue { get; set; }
        }

        public static List<Colour> GetColoursList()
        {
            Console.WriteLine($"File: {file_colours}");
            string json = File.ReadAllText(file_colours);
            Console.WriteLine($"Colours: {json}");
            List<Colour> list = JsonConvert.DeserializeObject<List<Colour>>(json);
            if (list == null) throw new FileNotFoundException("No list exists");
            return list;
        }
    }
}
