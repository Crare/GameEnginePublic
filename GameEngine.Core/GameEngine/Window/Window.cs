using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core.GameEngine.Window
{
    public class Window
    {
        public RenderTarget2D RenderTarget;
        public Point GameResolution;
        public Rectangle RenderTargetDestination;
        public Color LetterBoxingColor = Color.Black;
        public GraphicsDeviceManager Graphics;

        public Window( 
            Point gameResolution,
            RenderTarget2D renderTarget2D,
            GraphicsDeviceManager graphics
            ) {
            RenderTarget = renderTarget2D;
            GameResolution = gameResolution;
            Graphics = graphics;
            Graphics.PreferredBackBufferWidth = GameResolution.X;
            Graphics.PreferredBackBufferHeight = GameResolution.Y;
            Graphics.ApplyChanges();
            RenderTargetDestination = GetRenderTargetDestination(GameResolution, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
        }

        public void StartDrawToRenderTarget(SpriteBatch spriteBatch)
        {
            Graphics.GraphicsDevice.SetRenderTarget(RenderTarget);
            Graphics.GraphicsDevice.Clear(LetterBoxingColor);
            spriteBatch.Begin();
        }

        public void EndDrawToRenderTarget(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            Graphics.GraphicsDevice.SetRenderTarget(null);
            Graphics.GraphicsDevice.Clear(LetterBoxingColor);
        }

        public void DrawToDestination(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp); // renders clear pixels, no antialiasing
            //spriteBatch.Begin();
            spriteBatch.Draw(RenderTarget, RenderTargetDestination, Color.White);
            spriteBatch.End();
        }

        public void ToggleFullScreen()
        {
            if (!Graphics.IsFullScreen)
            {
                Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                Graphics.PreferredBackBufferWidth = GameResolution.X;
                Graphics.PreferredBackBufferHeight = GameResolution.Y;
            }
            Graphics.IsFullScreen = !Graphics.IsFullScreen;
            Graphics.ApplyChanges();

            RenderTargetDestination = GetRenderTargetDestination(GameResolution, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
        }

        private Rectangle GetRenderTargetDestination(Point resolution, int preferredBackBufferWidth, int preferredBackBufferHeight)
        {
            float resolutionRatio = (float)resolution.X / resolution.Y;
            float screenRatio;
            Point bounds = new Point(preferredBackBufferWidth, preferredBackBufferHeight);
            screenRatio = (float)bounds.X / bounds.Y;
            float scale;
            Rectangle rectangle = new Rectangle();

            if (resolutionRatio < screenRatio)
                scale = (float)bounds.Y / resolution.Y;
            else if (resolutionRatio > screenRatio)
                scale = (float)bounds.X / resolution.X;
            else
            {
                // Resolution and window/screen share aspect ratio
                rectangle.Size = bounds;
                return rectangle;
            }
            rectangle.Width = (int)(resolution.X * scale);
            rectangle.Height = (int)(resolution.Y * scale);
            return CenterRectangle(new Rectangle(Point.Zero, bounds), rectangle);
        }

        static Rectangle CenterRectangle(Rectangle outerRectangle, Rectangle innerRectangle)
        {
            Point delta = outerRectangle.Center - innerRectangle.Center;
            innerRectangle.Offset(delta);
            return innerRectangle;
        }
    }
}
