using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using IsoMap;

namespace ExampleGame
{
    public static class Assets
    {
        public static Texture2D LoadTexture2D(this ContentManager content, string asset)
        {
            var texture = content.Load<Texture2D>(asset);
            texture.Name = asset;

            return texture;
        }
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Tile[,,] currentMap;
        private Texture2D mapTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Temporary viewport width and height.
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            MapAsset.LoadMap(@"Content\testmap.json");
            MapAsset.ActiveMap.ScaleFactor = 4;
            MapAsset.ActiveMap.CentreMap(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            mapTexture = Assets.LoadTexture2D(Content, "buch-outdoor");

            currentMap = MapAsset.ActiveMap.GetMap();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            for (int l = 0; l < currentMap.GetLength(0); l++)
            {
                for (int y = 0; y < currentMap.GetLength(1); y++)
                {
                    for (int x = 0; x < currentMap.GetLength(2); x++)
                    {
                        Tile drawTile = currentMap[l, y, x];

                        // If the tile's texture name is valid, draw it.
                        if (drawTile.Tileset == mapTexture.Name)
                        {
                            Rectangle drawTilePos = new Rectangle(drawTile.Position.X, drawTile.Position.Y, drawTile.Position.Width, drawTile.Position.Height);
                            Rectangle drawTileArea = new Rectangle(drawTile.Area.X, drawTile.Area.Y, drawTile.Area.Width, drawTile.Area.Height);

                            _spriteBatch.Draw(mapTexture, drawTilePos, drawTileArea, Color.White);

                        }
                    }
                }

            }
            

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
