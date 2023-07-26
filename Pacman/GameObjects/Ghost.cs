using GameEngine.Core.EntityManagement;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameObjects
{
    public class Ghost : Entity
    {
        public Ghost(Vector2 position, Rectangle boundingBox, float layer, float speed = 0, Sprite sprite = null, int tag = 0) : base(position, boundingBox, layer, speed, sprite, tag)
        {
        }
    }
}
