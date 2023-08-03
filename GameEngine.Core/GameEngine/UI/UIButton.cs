using GameEngine.Core.GameEngine.Audio;
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

        //public event EventHandler OnPressedDown;
        //public event EventHandler OnPressRelease;
        internal bool IsPressed = false;
        internal bool IsHover = false;
        internal bool IsDisabled = false;

        private Action OnPressCallback;
        private Action OnPressReleasedCallback;

        private int SoundEffectIndex;

        public UIButton(
            GraphicsDevice graphics,
            string text,
            Rectangle container,
            UITheme theme,
            float layerDepth = 1f,
            Action onPressCallback = null,
            Action onPressReleasedCallback = null,
            int soundEffectIndex = -1,
            bool isDisabled = false
            )
            : base(graphics, container, theme.Button.BackgroundColor, layerDepth)
        {
            Text = text;
            Theme = theme.Button;

            OnPressCallback = onPressCallback;
            OnPressReleasedCallback = onPressReleasedCallback;
            SoundEffectIndex = soundEffectIndex;
            IsDisabled = isDisabled;
        }

        public override void Draw(SpriteBatch spriteBatch, TextDrawer textDrawer, GameTime gameTime)
        {
            // draw box
            spriteBatch.Draw(Texture, 
                Container,
                IsDisabled
                    ? Theme.BackgroundColorDisabled
                    : IsPressed
                        ? Theme.BackgroundColorPressed
                        : IsHover
                            ? Theme.BackgroundColorHover
                            : Theme.BackgroundColor
                );

            // draw title over box
            textDrawer.Draw(
                Text,
                new Vector2(Container.X + Container.Width / 2, Container.Y + Container.Height / 2),
                IsDisabled
                    ? Theme.TextColorDisabled
                    : IsPressed
                        ? Theme.TextColorPressed
                        : IsHover
                            ? Theme.TextColorHover
                            : Theme.TextColor,
                IsDisabled
                    ? Theme.TextSizeDisabled
                    : IsPressed
                        ? Theme.TextSizePressed
                        : IsHover
                            ? Theme.TextSizeHover
                            : Theme.TextSize,
                HorizontalAlignment.Center,
                VerticalAlignment.Middle
            );
        }

        public void SetDisabled(bool disabled)
        {
            IsDisabled = disabled;
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
            if (SoundEffectIndex != -1)
            {
                AudioManager.Instance.PlaySound(SoundEffectIndex);
            }
            if (IsDisabled)
            {
                return;
            }
            OnPressCallback?.Invoke();
        }

        private void InvokeOnPressRelease()
        {
            if (IsDisabled)
            {
                return;
            }
            OnPressReleasedCallback?.Invoke();
        }
    }
}
