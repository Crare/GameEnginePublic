using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman.GameObjects
{
    public class RedGhost : Ghost
    {
        public RedGhost(Vector2 position, Texture2D texture) 
            : base(position, texture, Color.Red)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            // TODO: red ghost logic
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
        }
    }

    public class BlueGhost : Ghost
    {
        public BlueGhost(Vector2 position, Texture2D texture)
            : base(position, texture, Color.Blue)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            // TODO: blue ghost logic
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
        }
    }

    public class OrangeGhost : Ghost
    {
        public OrangeGhost(Vector2 position, Texture2D texture)
            : base(position, texture, Color.Orange)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            // TODO: orange ghost logic
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
        }
    }

    public class PinkGhost : Ghost
    {
        public PinkGhost(Vector2 position, Texture2D texture)
            : base(position, texture, Color.Pink)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            // TODO: pink ghost logic
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
        }
    }

    public class Ghost : Entity
    {
        private SpriteAnimation Animation;
        private Color ColorTint;

        public Ghost(Vector2 position, Texture2D texture, Color colorTint)
            : base(position, new Rectangle((int)position.X, (int)position.Y, 16, 16), (float)Globals.SpriteLayers.MIDDLEGROUND, Globals.GHOST_SPEED, null, (int)Globals.PacmanTags.Ghost)
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
