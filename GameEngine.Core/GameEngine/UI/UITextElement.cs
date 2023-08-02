using System;
using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core.GameEngine.UI
{
    public class UITextElement : UIElement
    {
        public UIElementTheme Theme;
        private string Text;
        public HorizontalAlignment HAlign =  HorizontalAlignment.None;
        public VerticalAlignment VAlign  = VerticalAlignment.None;

        // TODO: add listener to trigger changing text.
        public UITextElement(
            string text,
            UIElementTheme theme,
            GraphicsDevice graphics,
            Rectangle container,
            float layerDepth,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment)
            : base(graphics, container, default, layerDepth)
        {
            Text = text;
            Theme = theme;
            HAlign = horizontalAlignment;
            VAlign = verticalAlignment;
        }

        public UITextElement(
            string text,
            UIElementTheme theme,
            GraphicsDevice graphics,
            Rectangle container,
            float layerDepth)
            : base(graphics, container, default, layerDepth)
        {
            Text = text;
            Theme = theme;
        }

        public void SetText(string newText)
        {
            Text = newText;
        }

        public override void Draw(SpriteBatch spriteBatch, TextDrawer textDrawer, GameTime gameTime)
        {
            textDrawer.Draw(
                Text,
                new Vector2(Container.X, Container.Y),
                Theme.TextColor,
                Theme.TextSize,
                HAlign != HorizontalAlignment.None ? HAlign : Theme.HAlign,
                VAlign != VerticalAlignment.None ? VAlign : Theme.VAlign
                );
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}

