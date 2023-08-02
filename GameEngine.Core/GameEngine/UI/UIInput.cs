using System;
using GameEngine.Core.GameEngine.InputManagement;
using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Core.GameEngine.UI
{
    public class UIInput : UIElement
    {
        public string InputText;
        internal string Placeholder;

        internal bool IsHover = false;
        internal bool IsPressed = false;
        internal bool IsActive = false;

        private Action<string> OnInputTextChangedCallback;

        private readonly UIElementTheme Theme;

        private KeyboardState KeyboardState;

        public UIInput(
            string inputText,
            string placeholder,
            UIElementTheme theme,
            GraphicsDevice graphics,
            Rectangle container,
            float layerDepth,
            Action<string> onInputTextChangedCallback)
            : base(graphics, container, theme.BackgroundColor, layerDepth)
        {
            Theme = theme;
            InputText = inputText;
            Placeholder = placeholder;
            OnInputTextChangedCallback = onInputTextChangedCallback;
        }

        public override void Draw(SpriteBatch spriteBatch, TextDrawer textDrawer, GameTime gameTime)
        {
            // draw box for outer border
            spriteBatch.Draw(
                Texture,
                new Rectangle(
                    Container.X,
                    Container.Y,
                    Container.Width,
                    Container.Height),
                IsPressed
                    ? Theme.BorderColorPressed
                    : IsHover
                        ? Theme.BorderColorHover
                        : Theme.BorderColor);

            // draw inner box
            spriteBatch.Draw(
                Texture,
                new Rectangle(
                    Container.X + Theme.BorderWidth,
                    Container.Y + Theme.BorderWidth,
                    Container.Width - Theme.BorderWidth * 2,
                    Container.Height - Theme.BorderWidth * 2),
                IsPressed
                    ? Theme.BackgroundColorPressed
                    : IsHover
                        ? Theme.BackgroundColorHover
                        : Theme.BackgroundColor);

            // draw input text of placeholder text
            var text = InputTextFormattting(gameTime, textDrawer);
            textDrawer.Draw(
                text,
                new Vector2(
                    Container.X + Theme.BorderWidth + Theme.InputPadding,
                    Container.Y + Container.Height / 2),
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
                Theme.HAlign != HorizontalAlignment.None ? Theme.HAlign : HorizontalAlignment.Right,
                Theme.VAlign != VerticalAlignment.None ? Theme.VAlign : VerticalAlignment.Middle
                );
        }

        private string InputTextFormattting(GameTime gametime, TextDrawer textDrawer)
        {
            var text = "";
                
            if (IsActive)
            {
                // if active element, blink '|'  at the end of text.
                text += InputText;
                if ((int)gametime.TotalGameTime.TotalSeconds % 2 == 0)
                {
                    text += " ";
                }
                else
                {
                    text += "|";
                }
            } else
            {
                text = InputText.Length > 0 ? InputText : Placeholder;
            }

            // cut overflow text, show last characters added to the end. So cut out the start of the string.
            Vector2 size = textDrawer.TextSize(text);
            while  (size.X > Container.Width - Theme.InputPadding - Theme.BorderWidth)
            {
                text = text.Substring(1);
                size = textDrawer.TextSize(text);
            }

            return text;
        }

        public override void Update(GameTime gameTime)
        {
            var lastKeyboardState = KeyboardState;
            var lastPressState = IsPressed;
            var mState = Mouse.GetState();
            KeyboardState = Keyboard.GetState();
            IsHover = IsColliding(mState);

            if (IsHover && mState.LeftButton == ButtonState.Pressed)
            {
                IsPressed = true;
            }
            else
            {
                IsPressed = false;
            }

            if (lastPressState && !IsPressed && IsHover)
            {
                IsActive = true;
            } else if (lastPressState && !IsPressed && !IsHover)
            {
                IsActive = false; // pressed outside input element
            }

            if (IsActive)
            {
                // check keyboard input
                HandleInput(KeyboardState, lastKeyboardState);
            }
        }

        public  void UpdateInputText(string newText)
        {
            InputText = newText;
        }

        private void HandleInput(KeyboardState keyboardState,  KeyboardState lastKeyboardState)
        {
            var newText = InputText;

            if (TextInputHelper.TryGetPressedKey(keyboardState, lastKeyboardState, out string key))
            {
                // allow only 3 letters to be added.
                if (key == "backspace" && newText.Length > 0)
                {
                    newText = newText.Substring(0, newText.Length - 1);
                }
                else if (key != "backspace")
                {
                    newText += key;
                }

                if (newText != InputText)
                {
                    InvokeOnInputTextChanged(newText);
                }
            }
        }

        private void InvokeOnInputTextChanged(string text)
        {
            OnInputTextChangedCallback?.Invoke(text);
        }

        private bool IsColliding(MouseState mouseState)
        {
            if (Container.Left < mouseState.X && mouseState.X < Container.Right && Container.Top < mouseState.Y)
            {
                return mouseState.Y < Container.Bottom;
            }
            return false;
        }
    }
}

