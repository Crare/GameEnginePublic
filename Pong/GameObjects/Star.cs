using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Pong.Globals;

namespace Pong.GameObjects
{
    public class Star : Entity
    {
        private SpriteAnimation Animation;

        public Star(Vector2 position, Rectangle boundingBox, SpriteAnimation animation) 
            : base(position, boundingBox, (float)SpriteLayers.BACKGROUND, 0, null, (int)PongTags.star)
        {
            Animation = animation;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            Animation.Update(gameTime);

            if (entityManager.IsColliding(this, (int)PongTags.ball))
            {
                PongEventSystem.StarPicked();
                entityManager.RemoveEntity(Id);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Animation.Draw(spriteBatch, Position, HorizontalFlipped, DepthLayer);
        }
    }
}
