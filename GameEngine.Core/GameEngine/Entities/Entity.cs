using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Core.EntityManagement
{
    public class Entity
    {
        public long Id { get; internal set; }

        public Rectangle BoundingBox { get; set; }
        public Vector2 Position { get; set; }
        public float DepthLayer { get; set; }
        public float Speed { get; set; }
        public bool HorizontalFlipped { get; set; }
        public Sprite Sprite { get; set; }
        public int Tag { get; set; }

        public Entity(Vector2 position, Rectangle boundingBox, float layer, float speed = 0, Sprite sprite = null, int tag = 0)
        {
            Position = position;
            BoundingBox = boundingBox;
            DepthLayer = layer;
            Speed = speed;
            Sprite = sprite;
            Tag = tag;
        }

        public virtual void Update(GameTime gameTime, KeyboardState keyboardState, GraphicsDeviceManager graphics, EntityManager entityManager)
        {

        }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            Sprite.Render(spriteBatch, Position, DepthLayer, HorizontalFlipped);
        }

        public virtual void DebugRender(SpriteBatch spriteBatch, Texture2D debugTexture, Color debugColor)
        {
            //graphics.GraphicsDevice.DrawIndexedPrimitives
            spriteBatch.Draw(debugTexture, BoundingBox, debugColor);
        }

    }
}
