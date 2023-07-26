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
        private int Direction = 0; // 0 = right, 1 = down, 2 = left, 3 = up

        public PacmanEntity(Rectangle boundingBox, Texture2D texture) 
            : base(new Vector2(boundingBox.X, boundingBox.Y), boundingBox, (float)Globals.SpriteLayers.MIDDLEGROUND, Globals.PACMAN_SPEED, null, (int)Globals.PacmanTags.Pacman)
        {
            var pacmanEat = new Rectangle[3];
            pacmanEat[0] = new Rectangle(0, 0, 16, 16);
            pacmanEat[1] = new Rectangle(16, 0, 16, 16);
            pacmanEat[2] = new Rectangle(32, 0, 16, 16);
            EatAnimation = new SpriteAnimation(texture, 0, 10, true, pacmanEat);

            var pacmanDeath = new Rectangle[9];
            pacmanDeath[0] = new Rectangle(48, 0, 16, 16);
            pacmanDeath[1] = new Rectangle(64, 0, 16, 16);
            pacmanDeath[2] = new Rectangle(80, 0, 16, 16);
            pacmanDeath[3] = new Rectangle(95, 0, 16, 16);
            pacmanDeath[4] = new Rectangle(112, 0, 16, 16);
            pacmanDeath[5] = new Rectangle(128, 0, 16, 16);
            pacmanDeath[6] = new Rectangle(144, 0, 16, 16);
            pacmanDeath[7] = new Rectangle(160, 0, 16, 16);
            pacmanDeath[8] = new Rectangle(176, 0, 16, 16);
            DeathAnimation = new SpriteAnimation(texture, 0, 10, false, pacmanDeath);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (AnimationState == 0)
            {
                EatAnimation.Draw(spriteBatch, Position, false, DepthLayer, Color.White, 0f, Vector2.One);
            } else
            {
                DeathAnimation.Draw(spriteBatch, Position, false, DepthLayer, Color.White, 0f, Vector2.One);
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

            // check keyboardState to change rotation and start moving in that direction if there is no wall.

        }
    }
}
