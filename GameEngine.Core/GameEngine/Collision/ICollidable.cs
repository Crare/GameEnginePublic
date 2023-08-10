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
    public interface ICollidable
    {
        BoxCollider Collider { get; set; }

        public void DebugDraw(SpriteBatch spriteBatch, Color color)
        {
            Collider.DebugDraw(spriteBatch, color);
        }
    }
}
