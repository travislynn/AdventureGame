using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace AvatarAdventure.TileEngine
{
    public class PortalLayer
    {
        [ContentSerializer]
        public Dictionary<Rectangle, Portal> Portals { get; private set; }

        public PortalLayer()
        {
            Portals = new Dictionary<Rectangle, Portal>();
        }
    }
}