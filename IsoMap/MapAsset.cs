using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IsoMap
{
    public static class MapAsset
    {
        // The current map loaded in. Contains an instance of the Map class.
        public static Map ActiveMap { get; set; }

        // Reads all information from a JSON file, and stores it as the active map.
        public static void LoadMap(string path)
        {
            string mapString;

            // If the file is at the specified path,
            if (File.Exists(path))
            {
                // Read it, and deserialize it to a new instance of the Map class.
                mapString = File.ReadAllText(path);
                ActiveMap = JsonSerializer.Deserialize<Map>(mapString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            // Sends a message if the map wasn't found, and loads a placeholder map to prevent runtime errors.
            else
            {
                Console.Error.WriteLine("Map not found at " + path);
                ActiveMap = new Map();
            }
        }
    }
}
