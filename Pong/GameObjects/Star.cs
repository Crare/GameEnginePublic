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

        public override void Update(GameTime gameTime, KeyboardState keyboardState, GraphicsDeviceManager graphics, EntityManager entityManager)
        {
            Animation.Update(gameTime);

            if (entityManager.IsColliding(this, (int)PongTags.ball))
            {
                PongEventSystem.StarPicked();
                entityManager.RemoveEntity(Id);
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            Animation.Render(spriteBatch, Position, HorizontalFlipped, DepthLayer);
        }
    }
}
