using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Core.GameEngine.UI
{
    public class UIManager
    {
        internal int lastUIElementId = 0;
        internal List<UIElement> uiElements = new();
        // TODO: maybe do UIElementContainer to visualize group of UI-elements.

        internal DebugMouse debugMouse = new();

        public SpriteBatch SpriteBatch;
        public TextDrawer TextDrawer;

        public UIManager(SpriteBatch spriteBatch, TextDrawer textDrawer) {
            SpriteBatch = spriteBatch;
            TextDrawer = textDrawer;
        }

        public void AddUIElement(UIElement element)
        {
            element.Id = lastUIElementId;
            lastUIElementId++;
            uiElements.Add(element);
        }

        public void RemoveUIElement(UIElement element)
        {
            uiElements = uiElements.Where(e => e.Id != element.Id).ToList();
        }

        public virtual void UpdateUIElements(GameTime gameTime)
        {
            uiElements.ForEach(uiElement =>
            {
                uiElement.Update(gameTime);
            });
        }

        public virtual void DrawUIElements()
        {
            uiElements.ForEach(uiElement =>
            {
                uiElement.Draw(SpriteBatch, TextDrawer);
            });
        }

        public virtual void DebugDrawUIElements(Texture2D debugTexture, Color debugColor, Color debugColor2)
        {
            uiElements.ForEach(uiElement =>
            {
                uiElement.DebugDraw(SpriteBatch, TextDrawer, debugTexture, debugColor, debugColor2);
            });

            debugMouse.DebugDraw(SpriteBatch, debugTexture, debugColor2);
        }
    }
}
