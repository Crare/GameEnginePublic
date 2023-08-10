using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.GameEngine.Sprites
{
    public interface IHasSprite
    {
        Sprite Sprite { get; set; }
        bool HorizontalFlipped { get; set; }
        float DepthLayer { get; set; }

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
