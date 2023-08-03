using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace GameEngine.Core.GameEngine.Sprites
{
    public class SpriteAnimation
    {
        public Texture2D Texture;
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

        /// <summary>
        /// this works if we know all the tiles from spritesheet are used for this animation.
        /// </summary>
        public SpriteAnimation(Texture2D texture, int startingFrame, float frameRate, bool isLooping, int spriteSheetColumns, int spriteSheetRows)
        {
            Texture = texture;
            Frame = startingFrame;
            FrameThreshold = (1000 / frameRate);
            IsLooping = isLooping;
            SetupSourceRectangles(spriteSheetColumns, spriteSheetRows);
        }

        private void SetupSourceRectangles(int columns, int rows)
        {
            var spriteWidth = Texture.Width / columns;
            var spriteHeight = Texture.Height / rows;
            SourceRectangles = new Rectangle[columns * rows];
            var i = 0;
            for (var row = 0; row < rows; row++)
            {
                for(var col = 0; col < columns; col++)
                {
                    SourceRectangles[i] = new Rectangle(columns * spriteWidth, rows * spriteHeight, spriteWidth, spriteHeight);
                }
            }
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
        /// <returns>If non looping animation has ended. Or animation has looped back to start.</returns>
        public bool Update(GameTime gameTime)
        {
            var looped = false;
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
                    looped = true;
                }
                Timer = 0;
            } else
            {
                Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            return looped;
        }
    }
}
