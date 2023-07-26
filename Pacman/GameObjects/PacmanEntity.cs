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
    public class PacmanEntity : Entity
    {
        SpriteAnimation EatAnimation;
        SpriteAnimation DeathAnimation;
        private int AnimationState = 0; // 0 = eat, 1 = death

        public PacmanEntity(Rectangle boundingBox, Texture2D texture) 
            : base(new Vector2(boundingBox.X, boundingBox.Y), boundingBox, (float)Globals.SpriteLayers.MIDDLEGROUND, Globals.PACMAN_SPEED, null, (int)Globals.PacmanTags.Pacman)
        {
            var pacmanEat = new Rectangle[3];
            pacmanEat[0] = new Rectangle(0, 0, 16, 16);
            pacmanEat[1] = new Rectangle(0, 16, 16, 32);
            pacmanEat[2] = new Rectangle(0, 32, 16, 48);
            EatAnimation = new SpriteAnimation(texture, 0, 15, true, pacmanEat);

            var pacmanDeath = new Rectangle[9];
            pacmanDeath[0] = new Rectangle(0, 0, 0, 0);
            pacmanDeath[1] = new Rectangle(0, 0, 0, 0);
            pacmanDeath[2] = new Rectangle(0, 0, 0, 0);
            pacmanDeath[3] = new Rectangle(0, 0, 0, 0);
            pacmanDeath[4] = new Rectangle(0, 0, 0, 0);
            pacmanDeath[5] = new Rectangle(0, 0, 0, 0);
            pacmanDeath[6] = new Rectangle(0, 0, 0, 0);
            pacmanDeath[7] = new Rectangle(0, 0, 0, 0);
            pacmanDeath[8] = new Rectangle(0, 0, 0, 0);
            DeathAnimation = new SpriteAnimation(texture, 0, 15, false, pacmanDeath);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (AnimationState == 0)
            {
                EatAnimation.Draw(spriteBatch, Position, false, DepthLayer);
            } else
            {
                DeathAnimation.Draw(spriteBatch, Position, false, DepthLayer);
            }
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            if (AnimationState == 0)
            {
                EatAnimation.Update(gameTime);
            }
            else
            {
                var animationEnded = DeathAnimation.Update(gameTime);
            }
        }
    }
}
