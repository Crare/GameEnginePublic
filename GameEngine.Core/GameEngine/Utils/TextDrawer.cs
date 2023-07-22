using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core.GameEngine.Utils
{
    public enum Alignment
    {
        Left, Right, Center
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

        public void Draw(string text, Vector2 position, Alignment align = Alignment.Right)
        {
            position = Align(text, position, align);
            SpriteBatch.DrawString(Font, text, position, DefaultColor);
        }

        public void Draw(string text, Vector2 position, Color color, Alignment align = Alignment.Right)
        {
            position = Align(text, position, align);
            SpriteBatch.DrawString(Font, text, position, color);
        }

        public void Draw(string text, Vector2 position, Color color, float scale, Alignment align = Alignment.Right)
        {
            position = Align(text, position, align);
            SpriteBatch.DrawString(Font, text, position, color, 0f, Vector2.One, scale, SpriteEffects.None, 0);
        }

        private Vector2 Align(string text, Vector2 position, Alignment align)
        {
            if (align == Alignment.Right)
            {
                return position;
            }
            
            Vector2 size = Font.MeasureString(text);
            if (align == Alignment.Center)
            {
                return new Vector2(position.X - size.X * 0.5f, position.Y);
            }
            else if (align == Alignment.Left)
            {
                return new Vector2(position.X - size.X, position.Y);
            }

            return position;
        }
    }
}
