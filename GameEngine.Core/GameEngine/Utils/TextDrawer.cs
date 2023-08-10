using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core.GameEngine.Utils
{
    public enum HorizontalAlignment
    {
        None, Left, Right, Center,
    }

    public enum VerticalAlignment
    {
        None, Top, Bottom, Middle,
    }

    public class TextDrawer
    {
        private SpriteFont Font { get; set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public Color DefaultColor { get; set; }

        public TextDrawer(SpriteBatch spriteBatch, SpriteFont font, Color defaultColor)
        {
            SpriteBatch = spriteBatch;
            Font = font;
            DefaultColor = defaultColor;
        }

        /// <summary>
        /// Draws without trying to align text
        /// </summary>
        public void Draw2(string text, Vector2 position, Color color, float scale = 1.0f)
        {
            SpriteBatch.DrawString(Font, text, position, color, 0f, Vector2.One, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// draws with alignment
        /// </summary>
        public void Draw(string text, Vector2 position, HorizontalAlignment hAlign = HorizontalAlignment.Right, VerticalAlignment vAlign = VerticalAlignment.Bottom)
        {
            position = Align(text, position, hAlign, vAlign);
            SpriteBatch.DrawString(Font, text, position, DefaultColor);
        }

        /// <summary>
        /// draws with alignment
        /// </summary>
        public void Draw(string text, Vector2 position, Color color, HorizontalAlignment hAlign = HorizontalAlignment.Right, VerticalAlignment vAlign = VerticalAlignment.Bottom)
        {
            position = Align(text, position, hAlign, vAlign);
            SpriteBatch.DrawString(Font, text, position, color);
        }

        /// <summary>
        /// draws with alignment
        /// </summary>
        public void Draw(string text, Vector2 position, Color color, float scale, HorizontalAlignment hAlign = HorizontalAlignment.Right, VerticalAlignment vAlign = VerticalAlignment.Bottom)
        {
            position = Align(text, position, hAlign, vAlign);
            SpriteBatch.DrawString(Font, text, position, color, 0f, Vector2.One, scale, SpriteEffects.None, 0);
        }

        public Vector2 TextSize(string text)
        {
            return Font.MeasureString(text);
        }

        private Vector2 Align(string text, Vector2 position, HorizontalAlignment hAlign = HorizontalAlignment.Right, VerticalAlignment vAlign = VerticalAlignment.Bottom)
        {
            var pos = position;
            //if (hAlign == HorizontalAlignment.Right) // default
            //if (vAlign == HorizontalAlignment.Bottom) // default

            Vector2 size = TextSize(text);

            if (hAlign == HorizontalAlignment.Center)
            {
                pos = new Vector2(position.X - size.X * 0.5f, position.Y);
            }
            else if (hAlign == HorizontalAlignment.Left)
            {
                pos = new Vector2(position.X - size.X, position.Y);
            }

            if (vAlign == VerticalAlignment.Middle)
            {
                pos = new Vector2(pos.X, position.Y - size.Y * 0.5f);
            }
            else if (vAlign == VerticalAlignment.Top)
            {
                pos = new Vector2(pos.X, position.Y - size.Y);
            }

            return pos;
        }
    }
}
