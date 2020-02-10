using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AvatarAdventure.TileEngine
{
    public class Engine
    {
        private static float _scrollSpeed = 500f;
        public static int TileWidth { get; set; } = 32;

        public static int TileHeight { get; set; } = 32;

        public TileMap Map { get; private set; }

        public static Rectangle ViewportRectangle { get; set; }

        public static Camera Camera { get; private set; }

        public Engine(Rectangle viewPort)
        {
            ViewportRectangle = viewPort;
            Camera = new Camera();
            TileWidth = 64;
            TileHeight = 64;
        }

        public Engine(Rectangle viewPort, int tileWidth, int tileHeight) : this(viewPort)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }

        public static Point VectorToCell(Vector2 position)
        {
            return new Point((int)position.X / TileWidth, (int)position.Y / TileHeight);
        }

        public void SetMap(TileMap newMap)
        {
            if (newMap == null)
            {
                throw new ArgumentNullException("newMap");
            }
            Map = newMap;
        }
        public void Update(GameTime gameTime)
        {
            Map.Update(gameTime);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Map.Draw(gameTime, spriteBatch, Camera);
        }
    }
}
