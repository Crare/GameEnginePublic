using GameEngine.Core.GameEngine.CameraView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameEngine.Core.GameEngine.Window
{
    public class GameWindow
    {
        public RenderTarget2D RenderTarget;
        public Point GameResolution;
        public Rectangle RenderTargetDestination;
        public Color LetterBoxingColor = Color.Black;
        public GraphicsDeviceManager Graphics;
        public float Scaling;

        public GameWindow(
            Point gameResolution,
            RenderTarget2D renderTarget2D,
            GraphicsDeviceManager graphics
            ) {
            RenderTarget = renderTarget2D;
            GameResolution = gameResolution;
            Graphics = graphics;
            Graphics.PreferredBackBufferWidth = Graphics.GraphicsDevice.DisplayMode.Width;
            Graphics.PreferredBackBufferHeight = Graphics.GraphicsDevice.DisplayMode.Height;
            Graphics.ApplyChanges();
            RenderTargetDestination = GetRenderTargetDestination(GameResolution, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
            Scaling = (float)GameResolution.Y / (float)Graphics.PreferredBackBufferHeight;
        }

        public float GetHorizontalCenter()
        {
            return RenderTarget.Width / 2;
        }

        public float GetVerticalCenter()
        {
            return RenderTarget.Height / 2;
        }

        /// <summary>
        /// Start draw to render target with camera translation matrix.
        /// </summary>
        public void StartDrawToRenderTarget(SpriteBatch spriteBatch, Camera camera = null)
        {
            Graphics.GraphicsDevice.SetRenderTarget(RenderTarget);
            Graphics.GraphicsDevice.Clear(LetterBoxingColor);
            if (camera != null)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        camera.get_transformation(RenderTarget.Width, RenderTarget.Height));
                return;
            } 
            spriteBatch.Begin();
        }

        public void EndDrawToRenderTarget(SpriteBatch spriteBatch, bool drawUIAfter = false)
        {
            spriteBatch.End();
            if (!drawUIAfter)
            {
                Graphics.GraphicsDevice.SetRenderTarget(null);
                Graphics.GraphicsDevice.Clear(LetterBoxingColor);
            }
        }

        /// <summary>
        /// Start draw UI over screen. doesn't use camera translation matrix.
        /// </summary>
        public void StartDrawUIToRenderTarget(SpriteBatch spriteBatch)
        {
            Graphics.GraphicsDevice.SetRenderTarget(RenderTarget);
            //Graphics.GraphicsDevice.Clear(LetterBoxingColor);
            spriteBatch.Begin();
        }

        public void EndDrawUIToRenderTarget(SpriteBatch spriteBatch)
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
            Scaling = GameResolution.Y / Graphics.PreferredBackBufferHeight;
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

        public void OnResize(object sender, EventArgs e)
        {
            if ((Graphics.PreferredBackBufferWidth != Graphics.GraphicsDevice.Viewport.Width) ||
                (Graphics.PreferredBackBufferHeight != Graphics.GraphicsDevice.Viewport.Height))
            {
                Graphics.PreferredBackBufferWidth = Graphics.GraphicsDevice.Viewport.Width;
                Graphics.PreferredBackBufferHeight = Graphics.GraphicsDevice.Viewport.Height;
                Graphics.ApplyChanges();

                //States[_currentState].Rearrange();
                RenderTargetDestination = GetRenderTargetDestination(GameResolution, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
                Scaling = GameResolution.Y / Graphics.PreferredBackBufferHeight;
            }
        }

        public void OnMaximise(object sender, EventArgs e) //, Window window)
        {
            //window.Position = new Point(0, 0);
            Graphics.PreferredBackBufferWidth = Graphics.GraphicsDevice.DisplayMode.Width;
            Graphics.PreferredBackBufferHeight = Graphics.GraphicsDevice.DisplayMode.Height;
            Graphics.ApplyChanges();
        }
    }
}
