using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core.SpriteManagement
{
    public class Sprite
    {
        public Texture2D Texture;
        // TODO: add multiple textures to single sprite and add animations etc..
        //public Array<Texture2D> AnimatedTexture;
        //public int frame;

        public Sprite(Texture2D texture)
        {
            Texture = texture;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 entityPosition, float depthLayer, bool horizontalFlipped)
        {
            spriteBatch.Draw(
                Texture,
                entityPosition,
                null, // sourceRectangle
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
