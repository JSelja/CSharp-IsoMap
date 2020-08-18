using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using IsoMap;
using System;
using SharpDX.DXGI;
using System.Diagnostics;

namespace ExampleGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private int scaleFactor = 4;

        private Tile tile;
        private Texture2D tileTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            MapAsset.LoadMap(@"Content\testmap.json");
            Debug.WriteLine(MapAsset.ActiveMap.Layers[0].Name);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            tile = MapAsset.ActiveMap.GetTile(1, new IsoVector(0, 1));
            tileTexture = Content.Load<Texture2D>(tile.Tileset);
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
            _spriteBatch.Draw(tileTexture, new Rectangle(0, 0, tile.Area.Width * scaleFactor, tile.Area.Height * scaleFactor),
                new Rectangle(tile.Area.X, tile.Area.Y, tile.Area.Width, tile.Area.Height), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
