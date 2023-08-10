using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Collision;
using GameEngine.Core.GameEngine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using static Pong.Globals;

namespace Pong.GameObjects
{
    public class Star : Entity, ICollidable, IHasSpriteAnimation
    {
        public SpriteAnimation Animation { get; set; }
        public bool HorizontalFlipped { get; set; }
        public float DepthLayer { get; set; }
        public BoxCollider Collider { get; set; }

        public Star(Vector2 position, Rectangle boundingBox, SpriteAnimation animation) 
            : base(position, (int)PongTags.star)
        {
            Collider = new BoxCollider(boundingBox);
            DepthLayer = (float)SpriteLayers.BACKGROUND;
            Animation = animation;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            Animation.Update(gameTime);

            if (entityManager.IsColliding(this, (int)PongTags.ball, out var collidedEntity))
            {
                PongEventSystem.StarPicked();
                entityManager.RemoveEntity(Id);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Animation.Draw(spriteBatch, Position, HorizontalFlipped, DepthLayer);
        }

        public override void DebugDraw(SpriteBatch spriteBatch, Color debugColor)
        {
            Collider.DebugDraw(spriteBatch, debugColor);
        }
    }
}
