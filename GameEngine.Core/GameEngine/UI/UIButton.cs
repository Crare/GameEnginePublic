using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.GameEngine.UI
{
    public class UIButton : UIElement
    {
        internal string Text;
        internal Color TextColor;
        internal Color TextPressedColor;
        internal float TextScale;
        internal event EventHandler OnPressedDown;
        internal event EventHandler OnPressRelease;
        internal bool IsPressed = false;
        internal Color PressedBackgroundColor;

        public UIButton(
            GraphicsDevice graphics, 
            string text, 
            Rectangle container, 
            Color backgroundColor, 
            Color pressedBackgroundColor,
            Color textColor,
            Color textPressedColor,
            float textScale = 1f,
            float layerDepth = 1f
            ) 
            : base(graphics, container, backgroundColor, layerDepth)
        {
            Text = text;
            PressedBackgroundColor = pressedBackgroundColor;
            TextColor = textColor;
            TextPressedColor = textPressedColor;
            TextScale = textScale;
        }

        public override void Draw(SpriteBatch spriteBatch, TextDrawer textDrawer)
        {
            // draw box
            spriteBatch.Draw(Texture, 
                Container,
                IsPressed ? PressedBackgroundColor : BackgroundColor
                );
            //spriteBatch.Draw(
            //        Texture,
            //        new Vector2(Container.X, Container.Y),
            //        null,
            //        IsPressed ? PressedBackgroundColor : BackgroundColor,
            //        0f, // rotation
            //        new Vector2(Container.Width / 2, Container.Height / 2), // origin
            //        Vector2.One, // scale
            //        SpriteEffects.None,
            //        LayerDepth
            //    );

            // draw title over box
            textDrawer.Draw(
                Text,
                new Vector2(Container.X + Container.Width / 2, Container.Y + Container.Height / 2),
                IsPressed ? TextPressedColor : TextColor,
                TextScale,
                HorizontalAlignment.Center,
                VerticalAlignment.Middle
                );
        }

        override public void Update(GameTime gameTime)
        {
            var lastPressState = IsPressed;

            // TODO: checking if button is being pressed.
            // get mouse position
            // check if it is over button
            // check if mouse button is pressed
            //IsPressed = 
            var mState = Mouse.GetState();
            if (IsColliding(mState) && mState.LeftButton == ButtonState.Pressed)
            {
                IsPressed = true;
            } else
            {
                IsPressed = false;
            }

            if (!lastPressState && IsPressed) {
                InvokeOnPressedDown();
            }
            if (lastPressState && !IsPressed)
            {
                InvokeOnPressRelease();
            }
        }

        private bool IsColliding(MouseState mouseState) {
            if (Container.Left < mouseState.X && mouseState.X < Container.Right && Container.Top < mouseState.Y)
            {
                return mouseState.Y < Container.Bottom;
            }
            return false;
        }

        private void InvokeOnPressedDown()
        {
            var handler = OnPressedDown;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void InvokeOnPressRelease()
        {
            var handler = OnPressRelease;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
