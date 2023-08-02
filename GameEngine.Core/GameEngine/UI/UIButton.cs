using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameEngine.Core.GameEngine.UI
{
    public class UIButton : UIElement
    {
        internal string Text;

        private UIElementTheme Theme;

        public event EventHandler OnPressedDown;
        public event EventHandler OnPressRelease;
        internal bool IsPressed = false;
        internal bool IsHover = false;

        private Action OnPressCallback;
        private Action OnPressReleasedCallback;

        public UIButton(
            GraphicsDevice graphics,
            string text,
            Rectangle container,
            UITheme theme,
            float layerDepth = 1f,
            Action onPressCallback = null,
            Action onPressReleasedCallback = null
            )
            : base(graphics, container, theme.Button.BackgroundColor, layerDepth)
        {
            Text = text;
            Theme = theme.Button;

            OnPressCallback = onPressCallback;
            OnPressReleasedCallback = onPressReleasedCallback;
        }

        public override void Draw(SpriteBatch spriteBatch, TextDrawer textDrawer)
        {
            // draw box
            spriteBatch.Draw(Texture, 
                Container,
                IsPressed
                    ? Theme.BackgroundColorPressed
                    : IsHover
                        ? Theme.BackgroundColorHover
                        : Theme.BackgroundColor
                );

            // draw title over box
            textDrawer.Draw(
                Text,
                new Vector2(Container.X + Container.Width / 2, Container.Y + Container.Height / 2),
                IsPressed
                    ? Theme.TextColorPressed
                    : IsHover
                        ? Theme.TextColorHover
                        : Theme.TextColor,
                IsPressed
                    ? Theme.TextSizePressed
                    : IsHover
                        ? Theme.TextSizeHover
                        : Theme.TextSize,
                HorizontalAlignment.Center,
                VerticalAlignment.Middle
                );
        }

        override public void Update(GameTime gameTime)
        {
            var lastPressState = IsPressed;
            var mState = Mouse.GetState();
            IsHover = IsColliding(mState);

            if (IsHover && mState.LeftButton == ButtonState.Pressed)
            {
                IsPressed = true;
            } else
            {
                IsPressed = false;
            }

            if (!lastPressState && IsPressed && IsHover) {
                InvokeOnPressedDown();
            }
            if (lastPressState && !IsPressed && IsHover)
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
            OnPressCallback?.Invoke();
        }

        private void InvokeOnPressRelease()
        {
            var handler = OnPressRelease;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
            OnPressReleasedCallback?.Invoke();
        }
    }
}
