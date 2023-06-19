using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XKarts.Creator
{
    public class Race
    {
        // Public list to hold the info on all karts in the race
        public List<Identifier.Kart> KartList = new List<Identifier.Kart>();

        // Bytes to hold the required number of laps
        public byte Laps_Left, Laps_Right, Laps_Total;

        /// <summary>
        /// Create a race with the required info
        /// </summary>
        /// <param name="List"> List of Karts in the race [optional] </param>
        /// <param name="Left"> Required uses of Left lane [optional] </param>
        /// <param name="Right"> Required uses of Right lane [optional] </param>
        /// <param name="Total"> Required Total laps [optional] </param>
        /// <param name="Json"> Populate from JSON [optional - overrides other params] </param>
        public Race(
            List<Identifier.Kart>? List = null,
            byte Left = 0,
            byte Right = 0,
            byte Total = 0,
            string? Json = null)
        {
            // First check if Json is available
            if (Json != null)
            {
                PopulateFromJson(Json);
                return;
            }

            // Check if list is avail and apply
            if (List != null) KartList = List;

            Laps_Left = Left;
            Laps_Right = Right;
            Laps_Total = Total;
        }


        /// <summary>
        /// Add a Kart to the Race List
        /// </summary>
        /// <param name="kart"> 
        /// Kart object to add [can also be list]
        /// </param>
        /// <param name="clear"> 
        /// Clear the list first? [false] 
        /// </param>
        public void AddKart(Identifier.Kart kart, bool clear = false)
        {
            if (clear) KartList.Clear();

            KartList.Add(kart);
        }
        public void AddKart(List<Identifier.Kart> kartList, bool clear = false)
        {
            if (clear) KartList.Clear();

            KartList.AddRange(kartList);
        }

        /// <summary>
        /// Clear the Race List
        /// </summary>
        public void Reset()
        {
            KartList.Clear();
            Laps_Left = 0;
            Laps_Right = 0;
            Laps_Total = 0;
        }

        /// <summary>
        /// Convert the current Race Creator object to a JSON string for transmission
        /// </summary>
        /// <returns>
        /// Serialised JSON string
        /// </returns>
        public string GenerateJsonString()
        {
            if (!KartList.Any())
            {
                throw new ArgumentNullException("No Karts assigned to race");
            }

            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Convert the received JSON string into the Race Creator object
        /// </summary>
        /// <param name="data">
        /// JSON string for Deserialisation
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Conversion invalid
        /// </exception>
        public void PopulateFromJson(string data)
        {
            // Ensure the list is empty
            KartList.Clear();

            // Attempt to Deserialise the provided data and put it in the list
            var temp = 
                JsonConvert.DeserializeObject<Race>(data) ??
                throw new ArgumentNullException("Unable to Deserialise");

            // Validate List info
            var list = temp.KartList;
            if (list == null || !list.Any())
            {
                throw new ArgumentNullException("No Karts in race");
            }
            KartList = list;
                
            // Assign other race info
            Laps_Left = temp.Laps_Left;
            Laps_Right = temp.Laps_Right;
            Laps_Total = temp.Laps_Total;
        }
    }
}
