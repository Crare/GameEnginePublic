using System;
using System.Collections.Generic;
using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core.GameEngine.UI
{
	public class UIElementGroup
	{
		public List<UIElement> UIElements;

		public UIElementGroup(List<UIElement> uIElements)
		{
			UIElements = uIElements;
		}

		public void Draw(SpriteBatch spriteBatch, TextDrawer textDrawer)
		{
			UIElements.ForEach(e => e.Draw(spriteBatch, textDrawer));
		}

		public void Update(GameTime gameTime)
		{
			UIElements.ForEach(e => e.Update(gameTime));
		}
	}
}

