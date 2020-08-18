using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IsoMap
{
    public class Layer
    {
        public Layer()
        {
            Data = new int[] { 0 };
            Name = "Empty Layer";
        }

        public Layer(int[] data, string name)
        {
            Data = data;
            Name = name;
        }

        public int[] Data { get; set; }
        public string Name { get; set; }

    }

    public class Tileset
    {
        public Tileset()
        {
            Columns = 1;
            FirstGID = 0;
            Image = "";
            ImageHeight = 1;
            ImageWidth = 1;
            Margin = 0;
            Name = "Empty Tileset";
            Spacing = 0;
            TileCount = 1;
            TileHeight = 1;
            TileWidth = 1;
        }

        public Tileset(int columns, int firstGID, string image, Rectangle imageSize, int margin, string name, int spacing, int tileCount, Rectangle tileSize)
        {
            Columns = columns;
            FirstGID = firstGID;
            Image = image;
            ImageHeight = imageSize.Height;
            ImageWidth = imageSize.Width;
            Margin = margin;
            Name = name;
            Spacing = spacing;
            TileCount = tileCount;
            TileHeight = tileSize.Height;
            TileWidth = tileSize.Width;
        }

        public int Columns { get; set; }
        public int FirstGID { get; set; }
        public string Image { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public int Margin { get; set; }
        public string Name { get; set; }
        public int Spacing { get; set; }
        public int TileCount { get; set; }
        public int TileHeight { get; set; }
        public int TileWidth { get; set; }
    }

    public class Map
    {
        public Map()
        {
            Height = 1;
            Layers = new Layer[] { new Layer() };
            Orientation = "isometric";
            RenderOrder = "right-down";
            TileHeight = 1;
            Tilesets = new Tileset[] { new Tileset() };
            TileWidth = 1;
            Width = 1;
        }

        public Map(Rectangle mapSize, Layer[] layers, string orientation, string renderOrder, Rectangle tileSize, Tileset[] tilesets)
        {
            Height = mapSize.Height;
            Layers = layers;
            Orientation = orientation;
            RenderOrder = renderOrder;
            TileHeight = tileSize.Height;
            Tilesets = tilesets;
            TileWidth = tileSize.Width;
            Width = mapSize.Width;
        }

        public int Height { get; set; }
        public Layer[] Layers { get; set; }
        public string Orientation { get; set; }
        public string RenderOrder { get; set; }
        public int TileHeight { get; set; }
        public Tileset[] Tilesets { get; set; }
        public int TileWidth { get; set; }
        public int Width { get; set; }
    }
}
