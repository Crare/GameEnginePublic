using System;
using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;

namespace GameEngine.Core.GameEngine.UI
{
	public class UIElementTheme
	{
        //TODO: do optional sprite variants instead of colors.

        /// <summary>
        /// default color
        /// </summary>
		public Color BackgroundColor;
        /// <summary>
        /// pressed color
        /// </summary>
        public Color BackgroundColorPressed;
        /// <summary>
        /// hover color
        /// </summary>
        public Color BackgroundColorHover;
        /// <summary>
        /// active color, for gamepad control support
        /// </summary>
        public Color BackgroundColorActive;

        //public Color borderColor;
        //public Color borderColor2;

        public Color PlaceholderTextColor;

        public Color TextColor;
        public Color TextColorPressed;
        public Color TextColorHover;
        public Color TextColorActive;

        public float TextSize;
        public float TextSizePressed;
        public float TextSizeHover;
        public float TextSizeActive;

        public HorizontalAlignment HAlign;
        public VerticalAlignment VAlign;

        public int BorderWidth;
        public Color BorderColor;
        public Color BorderColorHover;
        public Color BorderColorPressed;
        public Color BorderColorActive;

        public int InputPadding;

        public UIElementTheme() { }

    }

	public class UITheme
	{
        public UIElementTheme Button;
        public UIElementTheme Title;
        public UIElementTheme Text;
        public UIElementTheme Input;

        public UITheme() { }
    }
}

