using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XKarts.RaceInfo
{
    public class Race
    {
        // Public list to hold the info on all karts in the race
        public List<Kart> KartList = new List<Kart>();

        // Bytes to hold the required number of laps
        public byte ReqLaps_Left, ReqLaps_Right, ReqLaps_Total;

        /// <summary>
        /// Create a race with the required info
        /// </summary>
        /// <param name="list"> List of Karts in the race [optional] </param>
        /// <param name="left"> Required uses of Left lane [optional] </param>
        /// <param name="right"> Required uses of Right lane [optional] </param>
        /// <param name="total"> Required Total laps [optional] </param>
        public Race(
            List<Kart> list,
            byte left = 0,
            byte right = 0,
            byte total = 0)
        {
            KartList = list;
            ReqLaps_Left = left;
            ReqLaps_Right = right;
            ReqLaps_Total = total;
        }
        public Race(string json)
        {
            PopulateFromJson(json);
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
        public void AddKart(Kart kart, bool clear = false)
        {
            if (clear) KartList.Clear();

            KartList.Add(kart);
        }
        public void AddKart(List<Kart> kartList, bool clear = false)
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
            ReqLaps_Left = 0;
            ReqLaps_Right = 0;
            ReqLaps_Total = 0;
        }

        /// <summary>
        /// Convert the current Race Creator object to a JSON string for transmission
        /// </summary>
        /// <returns>
        /// Serialised JSON string
        /// </returns>
        public string GenerateJsonString()
        {
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
            Reset();

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
            ReqLaps_Left = temp.ReqLaps_Left;
            ReqLaps_Right = temp.ReqLaps_Right;
            ReqLaps_Total = temp.ReqLaps_Total;
        }
    }
}
