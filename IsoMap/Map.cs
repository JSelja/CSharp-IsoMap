using System;
using System.Drawing;

namespace IsoMap
{
    // Specifies the tileset, the location and size within the tileset, and the tile's position in the map.
    // Used as an object passed from tile drawing methods.
    public class Tile
    {
        // Default constructor, with default values.
        public Tile()
        {
            Tileset = "";
        }

        // Primary constructor, inserts tileset name, location and size.
        public Tile(string tileset, Rectangle area, Rectangle position)
        {
            Tileset = tileset;
            Area = area;
            Position = position;
        }

        public string Tileset { get; set; }
        public Rectangle Area { get; set; }
        public Rectangle Position { get; set; }
    }

    // 2D plane of tiles.
    public class Layer
    {
        // Default constructor. Creates an empty layer.
        public Layer()
        {
            Data = new int[] { 0 };
            Name = "layer";
        }

        // Primary constructor, which inserts tile data and a name from the JSON file.
        public Layer(int[] data, string name)
        {
            Data = data;
            Name = name;
        }

        public int[] Data { get; set; }
        public string Name { get; set; }

    }

    // Set of tile sprites.
    public class Tileset
    {
        // Default constructor, with a default image.
        public Tileset()
        {
            Columns = 1;
            FirstGID = 1;
            Image = "";
            ImageHeight = 1;
            ImageWidth = 1;
            Margin = 0;
            Name = "default";
            Spacing = 0;
            TileCount = 1;
            TileHeight = 1;
            TileWidth = 1;
        }

        // Main constructor, inserts all necessary data from the JSON file.
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

    // A full map, made up of layers of tiles.
    public class Map
    {
        // Default constructor, which calls the Layer and Tileset default constructors.
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

            CentreOffset = new IsoVector(0, 0);
            ScaleFactor = 1;
        }

        // Primary constructor, inserts all necessary data from the JSON file.
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

            CentreOffset = new IsoVector(0, 0);
            ScaleFactor = 1;
        }

        public int Height { get; set; }
        public Layer[] Layers { get; set; }
        public string Orientation { get; set; }
        public string RenderOrder { get; set; }
        public int TileHeight { get; set; }
        public Tileset[] Tilesets { get; set; }
        public int TileWidth { get; set; }
        public int Width { get; set; }

        public IsoVector CentreOffset { get; }
        public int ScaleFactor { get; set; }

        /// <summary>
        /// Centre align the map within the viewport.
        /// Ensure the scale factor is set before this method is run.
        /// </summary>
        /// <param name="viewportWidth">The width of the active game window.</param>
        /// <param name="viewportHeight">The height of the active game window.</param>
        public void CentreMap(int viewportWidth, int viewportHeight)
        {
            int mapPixelWidth = Width * TileWidth * ScaleFactor;
            int mapPixelHeight = Height * TileHeight * ScaleFactor;

            CentreOffset.X = (viewportWidth - mapPixelWidth) / 2;
            CentreOffset.Y = (viewportHeight - mapPixelHeight) / 2;

            // If the grid is isometric, centre align the map by half the viewport width.
            if (Orientation == "isometric")
                CentreOffset.X = (viewportWidth - TileWidth) / 2;
        }

        /// <summary>
        /// Get a particular tile's sprite and position from map data.
        /// </summary>
        /// <param name="layer">The map layer to lookup.</param>
        /// <param name="position">The x and y coordinates to lookup.</param>
        /// <returns>A tile object including the name of the tileset, a rectangle of the sprite within the tileset, and a rectangle of the tile's draw location on screen.</returns>
        public Tile GetTile(int layer, IsoVector position)
        {
            // Create local position vector, ensuring it is in cartesian coordinates.
            IsoVector pos = Coordinates.ToCartesian(position);

            // If the position vector is out of bounds, raise an error and output default value.
            if (pos.X < 0 || pos.X >= Width || pos.Y < 0 || pos.Y >= Height)
            {
                Console.Error.WriteLine("Map.GetTile() - Position vector out of bounds.");
                return new Tile();
            }

            // Get the GID at the specified layer and location.
            int gid = Layers[layer].Data[pos.X + pos.Y * Width];

            // If the GID is 0, the tile should be left empty. Return an empty tile.
            if (gid == 0)
                return new Tile();

            // Find the tileset the GID corresponds to.
            int targetTileset = -1;
            for (int i = 0; i < Tilesets.Length; i++)
                // If the GID is within the bounds of the tileset, it is part of the tileset.
                if (gid <= Tilesets[i].FirstGID + Tilesets[i].TileCount - 1)
                    targetTileset = i;

            // If the GID wasn't in any tileset, there is a problem with the layer data. Raise an error and output default value.
            if (targetTileset == -1)
            {
                Console.Error.WriteLine("Map.GetTile() - Layer " + Layers[layer].Name + " data has invalid GID at " + pos.X + ":" + pos.Y + ", " + gid);
                return new Tile();
            }

            Tileset ts = Tilesets[targetTileset];
            // Find the local tile id, by subtracting the first global id. For the first tileset, this is 1, while also aligns the local id back to 0!
            int localID = gid - ts.FirstGID;
            int areaX = localID % ts.Columns;
            int areaY = (localID - areaX) / ts.Columns;

            // Create a rectangle with the location and size of the tile within the tileset.
            Rectangle areaRect = new Rectangle(
                areaX * ts.TileWidth + areaX * ts.Spacing + ts.Margin,
                areaY * ts.TileHeight + areaY * ts.Spacing + ts.Margin,
                ts.TileWidth, ts.TileHeight);

            // Create a rectangle with the position of the tile for rendering on-screen.
            Rectangle posRect;
            // For orthogonal maps,
            if (Orientation == "orthogonal")
            {
                posRect = new Rectangle(
                    position.X * TileWidth * ScaleFactor + CentreOffset.X,
                    position.Y * TileHeight * ScaleFactor + CentreOffset.Y,
                    areaRect.Width * ScaleFactor, areaRect.Height * ScaleFactor);
            }
            // If the orientation is isometric, convert the position vector.
            else if (Orientation == "isometric")
            {
                posRect = new Rectangle(
                    (position.X - position.Y) * TileWidth / 2 * ScaleFactor + CentreOffset.X,
                    (position.X + position.Y) * TileHeight / 2 * ScaleFactor + CentreOffset.Y,
                    areaRect.Width * ScaleFactor, areaRect.Height * ScaleFactor);
            }
            // Otherwise, the orientation is not supported. Raise an error and output default value.
            else
            {
                Console.Error.WriteLine("Map.GetTile() - Map has unsupported orientation " + Orientation);
                return new Tile();
            }



            // Return the tileset, location and area as a tile.
            return new Tile(ts.Name, areaRect, posRect);
        }

        /// <summary>
        /// Get all tiles from the current map.
        /// </summary>
        /// <returns>A 3-dimensional array of tile objects.</returns>
        public Tile[,,] GetMap()
        {
            // Create a 3-dimensional array of tiles. Outer dimension specifies layers, then rows and columns.
            Tile[,,] mapTiles = new Tile[Layers.Length, Height, Width];

            for (int l = 0; l < mapTiles.GetLength(0); l++)
                for (int y = 0; y < mapTiles.GetLength(1); y++)
                    for (int x = 0; x < mapTiles.GetLength(2); x++)
                        mapTiles[l, y, x] = GetTile(l, new IsoVector(x, y));

            return mapTiles;
        }
    }
}
