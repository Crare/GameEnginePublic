using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core.SpriteManagement
{
    public class Sprite
    {
        public Texture2D Texture;
        public Rectangle? SourceRectangle = null;

        public Sprite(Texture2D texture)
        {
            Texture = texture;
        }

        public Sprite(Texture2D texture, Rectangle sourceRectangle)
        {
            Texture = texture;
            SourceRectangle = sourceRectangle;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 entityPosition, float depthLayer, bool horizontalFlipped)
        {
            // use whole texture
            spriteBatch.Draw(
                Texture,
                entityPosition,
                SourceRectangle,
                Color.White,
                0f, // rotation
                new Vector2(Texture.Width / 2, Texture.Height / 2), // origin
                Vector2.One, // scale
                horizontalFlipped ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                depthLayer
            );
        }
    }
}
