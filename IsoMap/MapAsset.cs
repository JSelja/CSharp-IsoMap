using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IsoMap
{
    public static class MapAsset
    {
        public static Map ActiveMap { get; set; }


        public static void LoadMap(string path)
        {
            string mapString;

            if (File.Exists(path))
            {
                mapString = File.ReadAllText(path);
                ActiveMap = JsonSerializer.Deserialize<Map>(mapString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                Console.Error.WriteLine("Map not found at " + path);
            }
        }
    }
}
