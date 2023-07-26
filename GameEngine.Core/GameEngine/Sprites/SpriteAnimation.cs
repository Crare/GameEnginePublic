using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace GameEngine.Core.GameEngine.Sprites
{
    public class SpriteAnimation
    {
        private Texture2D Texture;
        private int Frame;
        private float Timer;
        private float FrameThreshold;
        private bool IsLooping;
        private Rectangle[] SourceRectangles;

        public SpriteAnimation(Texture2D texture, int startingFrame, float frameRate, bool isLooping, Rectangle[] sourceRectangles)
        {
            Texture = texture;
            Frame = startingFrame;
            FrameThreshold = (1000 / frameRate);
            IsLooping = isLooping;
            SourceRectangles = sourceRectangles;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool flipHorizontally, float depthLayer)
        {
            var rect = SourceRectangles[Frame];
            spriteBatch.Draw(
                    Texture,
                    position,
                    rect, // sourceRectangle
                    Color.White,
                    0f, // rotation
                    new Vector2(rect.Width / 2, rect.Height / 2), // origin
                    Vector2.One, // scale
                    flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    depthLayer
                );
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool flipHorizontally, float depthLayer, Color colorTint, float rotation)
        {
            var rect = SourceRectangles[Frame];
            spriteBatch.Draw(
                    Texture,
                    position,
                    rect, // sourceRectangle
                    colorTint,
                    rotation,
                    new Vector2(rect.Width / 2, rect.Height / 2), // origin
                    Vector2.One, // scale
                    flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    depthLayer
                );
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool flipHorizontally, float depthLayer, Color colorTint, float rotation, Vector2 scale)
        {
            var rect = SourceRectangles[Frame];
            spriteBatch.Draw(
                    Texture,
                    position,
                    rect, // sourceRectangle
                    colorTint,
                    rotation,
                    new Vector2(rect.Width / 2, rect.Height / 2), // origin
                    scale,
                    flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                    depthLayer
                );
        }

        /// <summary>
        /// Updates the Sprite to next one based on framerate.
        /// </summary>
        /// <returns>If non looping animation has ended.</returns>
        public bool Update(GameTime gameTime)
        {
            if (!IsLooping && Frame == SourceRectangles.Length - 1)
            {
                return true; // indicates animation has ended
            }

            if (Timer > FrameThreshold)
            {
                Frame++;
                if (Frame == SourceRectangles.Length)
                {
                    Frame = 0;
                }
                Timer = 0;
            } else
            {
                Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            return false;
        }
    }
}
