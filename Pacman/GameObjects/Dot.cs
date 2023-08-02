using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Pacman.Globals;

namespace Pacman.GameObjects
{
    public class SmallDot : Dot
    {
        public SmallDot(Vector2 position, Texture2D texture)
            : base(position, new Rectangle((int)position.X, (int)position.Y, 4, 4), Globals.PacmanTags.DotSmall)
        {
            var anim = new Rectangle[4];
            anim[0] = new Rectangle(0,0,16,16);
            anim[1] = new Rectangle(16, 0, 16, 16);
            anim[2] = new Rectangle(32, 0, 16, 16);
            anim[3] = new Rectangle(48, 0, 16, 16);
            SpriteAnimation = new SpriteAnimation(texture, 0, 10, true, anim);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
            if (entityManager.IsColliding(this, (int)PacmanTags.Pacman, out var collidedEntity))
            {
                PacmanEventSystem.SmallDotPicked();
                entityManager.RemoveEntity(Id);
            }
        }

        public SmallDot Copy()
        {
            return new SmallDot(Position, SpriteAnimation.Texture);
        }
    }

    public class BigDot : Dot
    {
        public BigDot(Vector2 position, Texture2D texture)
            : base(position, new Rectangle((int)position.X, (int)position.Y, 8, 8), Globals.PacmanTags.DotBig)
        {
            var anim = new Rectangle[3];
            anim[0] = new Rectangle(64, 0, 16, 16);
            anim[1] = new Rectangle(80, 0, 16, 16);
            anim[2] = new Rectangle(96, 0, 16, 16);
            SpriteAnimation = new SpriteAnimation(texture, 0, 5, true, anim);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
            if (entityManager.IsColliding(this, (int)PacmanTags.Pacman, out var collidedEntity))
            {
                PacmanEventSystem.BigDotPicked();
                entityManager.RemoveEntity(Id);
            }
        }

        public BigDot Copy()
        {
            return new BigDot(Position, SpriteAnimation.Texture);
        }
    }

    public class Dot : Entity
    {
        internal SpriteAnimation SpriteAnimation;

        public Dot(Vector2 position, Rectangle boundingBox, Globals.PacmanTags tag)
            : base(position, boundingBox, (float)Globals.SpriteLayers.BACKGROUND, 0, null, (int)tag)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            SpriteAnimation.Draw(spriteBatch, Position, false, DepthLayer);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            SpriteAnimation.Update(gameTime);
        }
    }
}
