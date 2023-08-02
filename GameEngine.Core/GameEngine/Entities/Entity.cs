using GameEngine.Core.GameEngine.Utils;
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

        public Entity(Vector2 position, Rectangle boundingBox, float layer, float speed, Sprite sprite, int tag)
        {
            Position = position;
            BoundingBox = boundingBox;
            DepthLayer = layer;
            Speed = speed;
            Sprite = sprite;
            Tag = tag;
        }

        public virtual void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Draw(spriteBatch, Position, DepthLayer, HorizontalFlipped);
        }

        public virtual void DebugDraw(SpriteBatch spriteBatch, Texture2D debugTexture, Color debugColor, Color debugColor2)
        {
            GameDebug.DrawRectangle(BoundingBox, debugColor, false);
            //graphics.GraphicsDevice.DrawIndexedPrimitives
            //spriteBatch.Draw(debugTexture, BoundingBox, debugColor);
            spriteBatch.Draw(debugTexture, new Rectangle((int)Position.X-1, (int)Position.Y-1, 2, 2), debugColor2);
        }

    }
}
