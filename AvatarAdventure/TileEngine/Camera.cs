﻿using Microsoft.Xna.Framework;

namespace AvatarAdventure.TileEngine
{
    public class Camera
    {

        Vector2 position;
        float speed;


        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float Speed
        {
            get { return speed; }
            set { speed = (float)MathHelper.Clamp(speed, 1f, 16f); }
        }
        public Matrix Transformation
        {
            get { return Matrix.CreateTranslation(new Vector3(-Position, 0f)); }
        }
        public Camera()
        {
            speed = 4f;
        }
        public Camera(Vector2 position)
        {
            speed = 4f;
            Position = position;
        }
        public void LockCamera(TileMap map, Rectangle viewport)
        {
            position.X = MathHelper.Clamp(position.X,
                0,
                map.WidthInPixels - viewport.Width);
            position.Y = MathHelper.Clamp(position.Y,
                0,
                map.HeightInPixels - viewport.Height);
        }

        public void LockToSprite(TileMap map, AnimatedSprite sprite, Rectangle viewport)
        {
            position.X = (sprite.Position.X + sprite.Width / 2)
                         - (viewport.Width / 2);
            position.Y = (sprite.Position.Y + sprite.Height / 2)
                         - (viewport.Height / 2);
            LockCamera(map, viewport);
        }

    }

}
