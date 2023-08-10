using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.GameEngine.Collision
{
    public class BoxCollider
    {
        public Rectangle BoundingBox { get; private set; }

        public BoxCollider(Rectangle boundingBox)
        {
            BoundingBox = boundingBox;
        }

        public bool IsColliding(BoxCollider other)
        {
            return Intersects(BoundingBox, other.BoundingBox);
        }

        public bool IsColliding(Rectangle other)
        {
            return Intersects(BoundingBox, other);
        }

        private bool Intersects(Rectangle a, Rectangle b)
        {
            if (b.Left <= a.Right && a.Left <= b.Right && b.Top <= a.Bottom)
            {
                return a.Top <= b.Bottom;
            }

            return false;
        }

        public void UpdatePosition(Vector2 position)
        {
            BoundingBox = new Rectangle(
                (int)position.X - BoundingBox.Width / 2,
                (int)position.Y - BoundingBox.Height / 2, 
                BoundingBox.Width, 
                BoundingBox.Height);
        }

        public void DebugDraw(SpriteBatch spriteBatch, Color debugColor)
        {
            GameDebug.DrawRectangle(
                BoundingBox,
                debugColor,
                false
                );
        }
    }
}
