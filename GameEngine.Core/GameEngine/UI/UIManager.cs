using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.GameEngine.UI
{
    public class UIManager
    {
        private int lastUIElementId = 0;
        private List<UIElement> uiElements = new();
        // TODO: maybe do UIElementContainer to visualize group of UI-elements.

        private DebugMouse debugMouse = new();

        SpriteBatch SpriteBatch;
        TextDrawer TextDrawer;

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

        public void UpdateUIElements(GameTime gameTime)
        {
            uiElements.ForEach(uiElement =>
            {
                uiElement.Update(gameTime);
            });
        }

        public void DrawUIElements()
        {
            uiElements.ForEach(uiElement =>
            {
                uiElement.Draw(SpriteBatch, TextDrawer);
            });
        }

        public void DebugDrawUIElements(Texture2D debugTexture, Color debugColor, Color debugColor2)
        {
            uiElements.ForEach(uiElement =>
            {
                uiElement.DebugDraw(SpriteBatch, TextDrawer, debugTexture, debugColor, debugColor2);
            });

            debugMouse.DebugDraw(SpriteBatch, debugTexture, debugColor2);
        }
    }
}
