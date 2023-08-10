using GameEngine.Core.GameEngine.CameraView;
using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Core.GameEngine.UI
{
    public class UIElement
    {
        public int Id;

        internal Rectangle Container;
        internal Color BackgroundColor;
        internal Texture2D Texture;
        internal float LayerDepth;

        public UIElement(GraphicsDevice graphics, Rectangle container, Color background, float layerDepth) {
            Container = container;
            BackgroundColor = background;
            Texture = new Texture2D(graphics, 1, 1);
            Texture.SetData(new Color[] { Color.White });
            LayerDepth = layerDepth;
        }

        //public virtual void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(Texture, Container, BackgroundColor);
        //    spriteBatch.Draw(
        //            Texture,
        //            new Vector2(Container.X, Container.Y),
        //            null,
        //            BackgroundColor,
        //            0f, // rotation
        //            new Vector2(Container.X, Container.Y), // origin
        //            Vector2.One, // scale
        //            SpriteEffects.None,
        //            LayerDepth
        //        );
        //}

        public virtual void Draw(SpriteBatch spriteBatch, TextDrawer textDrawer, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Container, BackgroundColor);
        }

        public virtual void DebugDraw(SpriteBatch spriteBatch, TextDrawer textDrawer, Texture2D debugTexture, Color debugColor, Color debugColor2)
        {
            spriteBatch.Draw(debugTexture, Container, debugColor);
            spriteBatch.Draw(debugTexture, new Rectangle(Container.X - 1, Container.Y - 1, 2, 2), debugColor2);

            textDrawer.Draw(
                $"x={Container.X}, y={Container.Y}",
                new Vector2(Container.X, Container.Y),
                HorizontalAlignment.Right
            );
        }

        public virtual void Update(GameTime gameTime, Camera camera) { 
            
        }
    }
}
