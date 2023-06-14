using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XKarts
{
    namespace Server
    {
        public class Race
        {
            // Private list to hold the info on all karts in the race
            private List<Identifier.Kart> RaceList = new List<KIdentifierart.Kart>();

            /// <summary>
            /// Create new Race object
            /// </summary>
            /// <param name="raceList">
            /// Initalise a list of Karts [optional]
            /// </param>
            Race(List<Identifier.Kart> raceList)
            {
                RaceList = raceList;
            }
            Race() { }


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
                if (clear)
                {
                    RaceList.Clear();
                }

                RaceList.Add(kart);
            }

            public void AddKart(List<Identifier.Kart> kartList, bool clear = false)
            {
                if (clear)
                {
                    RaceList.Clear();
                }

                RaceList.AddRange(kartList);
            }

            /// <summary>
            /// Clear the Race List
            /// </summary>
            public void Clear()
            {
                RaceList.Clear();
            }

            /// <summary>
            /// Convert the current Race List to a JSON string for transmission
            /// </summary>
            /// <returns>
            /// Serialised JSON string
            /// </returns>
            public string SerialiseToString()
            {
                return JsonConvert.SerializeObject(RaceList);
            }

            /// <summary>
            /// Convert the received JSON string into the Race List
            /// </summary>
            /// <param name="data">
            /// JSON string for Deserialisation
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// Conversion invalid
            /// </exception>
            public void DeserialiseToList(string data)
            {
                // Ensure the list is empty
                RaceList.Clear();

                // Attempt to Deserialise the provided data and put it in the list
                RaceList = JsonConvert.DeserializeObject<List<Identifier.Kart>>(data) ?? 
                    throw new ArgumentNullException("Unable to Deserialise");
            }
        }
    }
}
