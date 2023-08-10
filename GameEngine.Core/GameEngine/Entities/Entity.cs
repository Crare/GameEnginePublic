using GameEngine.Core.GameEngine.Utils;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Core.EntityManagement
{
    public abstract class Entity
    {
        public long Id { get; internal set; }
        public int Tag { get; set; }
        public Vector2 Position { get; set; }

        public Entity(Vector2 position, int tag)
        {
            Position = position;
            Tag = tag;
        }

        public abstract void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void DebugDraw(SpriteBatch spriteBatch, Color debugColor);

    }
}
