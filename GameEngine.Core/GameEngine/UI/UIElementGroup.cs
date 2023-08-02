using System;
using System.Collections.Generic;
using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Core.GameEngine.UI
{
	public class UIElementGroup
	{
		public List<UIElement> UIElements;

		public UIElementGroup(List<UIElement> uIElements)
		{
			UIElements = uIElements;
		}

		public void Draw(SpriteBatch spriteBatch, TextDrawer textDrawer, GameTime gameTime)
		{
			UIElements.ForEach(e => e.Draw(spriteBatch, textDrawer,  gameTime));
		}

		public void Update(GameTime gameTime)
		{
			UIElements.ForEach(e => e.Update(gameTime));
		}

		public void DebugDraw(SpriteBatch spriteBatch, TextDrawer textDrawer, Texture2D debugTexture, Color debugColor, Color debugColor2)
        {
            UIElements.ForEach(e => e.DebugDraw(spriteBatch, textDrawer, debugTexture, debugColor, debugColor2));
        }
	}
}

