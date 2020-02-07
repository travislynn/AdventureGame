﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AvatarAdventure.TileEngine
{
    public class Camera
    {
        #region Field Region

        Vector2 position;
        float speed;

        #endregion
        #region Property Region

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
        #endregion
        #region Constructor Region
        public Camera()
        {
            speed = 4f;
        }
        public Camera(Vector2 position)
        {
            speed = 4f;
            Position = position;
        }
        #endregion
        public void LockCamera(TileMap map, Rectangle viewport)
        {
            position.X = MathHelper.Clamp(position.X,
                0,
                map.WidthInPixels - viewport.Width);
            position.Y = MathHelper.Clamp(position.Y,
                0,
                map.HeightInPixels - viewport.Height);
        }
    }

}
