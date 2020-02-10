using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace AvatarAdventure.TileEngine
{
    public class Portal
    {
        [ContentSerializer]
        public Point SourceTile { get; private set; }

        [ContentSerializer]
        public Point DestinationTile { get; private set; }

        [ContentSerializer]
        public string DestinationLevel { get; private set; }

        private Portal()
        {
        }
        public Portal(Point sourceTile, Point destinationTile, string destinationLevel)
        {
            SourceTile = sourceTile;
            DestinationTile = destinationTile;
            DestinationLevel = destinationLevel;
        }
    }
}
