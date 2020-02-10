using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AvatarAdventure.TileEngine
{
    public class TileSet
    {
        private readonly Rectangle[] _sourceRectangles;

        public int TilesWide = 8;
        public int TilesHigh = 8;
        public int TileWidth = 64;
        public int TileHeight = 64;

        [ContentSerializerIgnore]
        public Texture2D Texture { get; set; }

        [ContentSerializer]
        public string TextureName { get; set; }

        [ContentSerializerIgnore]
        public Rectangle[] SourceRectangles => (Rectangle[])_sourceRectangles.Clone();

        public TileSet()
        {
            _sourceRectangles = new Rectangle[TilesWide * TilesHigh];
            int tile = 0;
            for (int y = 0; y < TilesHigh; y++)
            for (int x = 0; x < TilesWide; x++)
            {
                _sourceRectangles[tile] = new Rectangle(
                    x * TileWidth,
                    y * TileHeight,
                    TileWidth,
                    TileHeight);
                tile++;
            }
        }
        public TileSet(int tilesWide, int tilesHigh, int tileWidth, int tileHeight)
        {
            TilesWide = tilesWide;
            TilesHigh = tilesHigh;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            _sourceRectangles = new Rectangle[TilesWide * TilesHigh];
            int tile = 0;
            for (int y = 0; y < TilesHigh; y++)
            for (int x = 0; x < TilesWide; x++)
            {
                _sourceRectangles[tile] = new Rectangle(
                    x * TileWidth,
                    y * TileHeight,
                    TileWidth,
                    TileHeight);
                tile++;
            }
        }
    }
}
