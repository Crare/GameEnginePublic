using System;
using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core.GameEngine.UI
{
    public class UITextElement : UIElement
    {
        public UIElementTheme Theme;
        public string Text;

        // TODO: add listener to trigger changing text.

        public UITextElement(string text, UIElementTheme theme, GraphicsDevice graphics, Rectangle container, float layerDepth)
            : base(graphics, container, default, layerDepth)
        {
            Text = text;
            Theme = theme;
        }

        public void SetText(string newText)
        {
            Text = newText;
        }

        public override void Draw(SpriteBatch spriteBatch, TextDrawer textDrawer)
        {
            textDrawer.Draw(
                Text,
                new Vector2(Container.X, Container.Y),
                Theme.TextColor,
                Theme.TextSize,
                Theme.HAlign,
                Theme.VAlign
                );
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}

