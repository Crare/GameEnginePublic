using GameEngine.Core.GameEngine.CameraView;
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
    public class UIInteractableElement : UIElement
    {
        public UIInteractableElement(
            GraphicsDevice graphics, Rectangle container, Color background, float layerDepth) 
            : base(graphics, container, background, layerDepth)
        {
        }

        public bool IsMouseOver(MouseState mouseState, Camera camera)
        {
            var mouseWorldPos = camera.ScreenToUIPosition(mouseState.Position);
            if (Container.Left <= mouseWorldPos.X
                && mouseWorldPos.X <= Container.Right
                && Container.Top <= mouseWorldPos.Y)
            {
                return mouseWorldPos.Y <= Container.Bottom;
            }
            return false;
        }
    }
}
