using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AvatarAdventure.TileEngine
{
    public class TileLayer
    {
        [ContentSerializer(CollectionItemName = "Tiles")]
        private readonly int[] _tiles;

        private Point _cameraPoint;
        private Point _viewPoint;
        private Point _min;
        private Point _max;
        private Rectangle _destination;

        [ContentSerializerIgnore]
        public bool Enabled { get; set; }

        [ContentSerializerIgnore]
        public bool Visible { get; set; }

        [ContentSerializer]
        public int Width { get; private set; }

        [ContentSerializer]
        public int Height { get; private set; }


        private TileLayer()
        {
            Enabled = true;
            Visible = true;
        }

        public TileLayer(int[] tiles, int width, int height) : this()
        {
            this._tiles = (int[])tiles.Clone();
            this.Width = width;
            this.Height = height;
        }

        public TileLayer(int width, int height) : this()
        {
            _tiles = new int[height * width];
            this.Width = width;
            this.Height = height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    _tiles[y * width + x] = 0;
                }
            }
        }

        public TileLayer(int width, int height, int fill) : this()
        {
            _tiles = new int[height * width];
            this.Width = width;
            this.Height = height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    _tiles[y * width + x] = fill;
                }
            }
        }

        public int GetTile(int x, int y)
        {
            if (x < 0 || y < 0)
                return -1;
            if (x >= Width || y >= Height)
                return -1;
            return _tiles[y * Width + x];
        }

        public void SetTile(int x, int y, int tileIndex)
        {
            if (x < 0 || y < 0)
                return;
            if (x >= Width || y >= Height)
                return;
            _tiles[y * Width + x] = tileIndex;
        }

        public void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, TileSet tileSet, Camera camera)
        {
            if (!Visible)
                return;

            _cameraPoint = Engine.VectorToCell(camera.Position);
            _viewPoint = Engine.VectorToCell(
                new Vector2(
                    (camera.Position.X + Engine.ViewportRectangle.Width),
                    (camera.Position.Y + Engine.ViewportRectangle.Height)));

            _min.X = Math.Max(0, _cameraPoint.X - 1);
            _min.Y = Math.Max(0, _cameraPoint.Y - 1);
            _max.X = Math.Min(_viewPoint.X + 1, Width);
            _max.Y = Math.Min(_viewPoint.Y + 1, Height);
            _destination = new Rectangle(0, 0, Engine.TileWidth, Engine.TileHeight);
            int tile;
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                camera.Transformation);

            for (int y = _min.Y; y < _max.Y; y++)
            {
                _destination.Y = y * Engine.TileHeight;
                for (int x = _min.X; x < _max.X; x++)
                {
                    tile = GetTile(x, y);
                    if (tile == -1)
                        continue;
                    _destination.X = x * Engine.TileWidth;
                        spriteBatch.Draw(
                        tileSet.Texture,
                        _destination,
                        tileSet.SourceRectangles[tile],
                        Color.White);
                }
            }
            spriteBatch.End();
        }
    }
}
