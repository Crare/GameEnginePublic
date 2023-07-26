using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Sprites;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameObjects
{
    public class Ghost : Entity
    {
        private SpriteAnimation Animation;
        private Color ColorTint;
        public Ghost(Rectangle boundingBox, Texture2D texture, Color colorTint)
            : base(new Vector2(boundingBox.X, boundingBox.Y), boundingBox, (float)Globals.SpriteLayers.MIDDLEGROUND, Globals.GHOST_SPEED, null, (int)Globals.PacmanTags.Ghost)
        {
            ColorTint = colorTint;
            var anim = new Rectangle[4];
            anim[0] = new Rectangle(0, 0, 16, 16);
            anim[1] = new Rectangle(16, 0, 16, 16);
            anim[2] = new Rectangle(32, 0, 16, 16);
            anim[3] = new Rectangle(48, 0, 16, 16);
            Animation = new SpriteAnimation(texture, 0, 10, true, anim);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Animation.Draw(spriteBatch, Position, HorizontalFlipped, DepthLayer, ColorTint, 0f, Vector2.One);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            Animation.Update(gameTime);
        }
    }
}
