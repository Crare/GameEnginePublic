using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Core.GameEngine.Utils
{
    public class DebugMouse
    {
        public void DebugDraw(SpriteBatch spriteBatch, Texture2D debugTexture, Color debugColor2)
        {
            var mouseState = Mouse.GetState();
            spriteBatch.Draw(debugTexture, new Rectangle(mouseState.X - 1, mouseState.Y - 1, 2, 2), debugColor2);
        }
    }
}
