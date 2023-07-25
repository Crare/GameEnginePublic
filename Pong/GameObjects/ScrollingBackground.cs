using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.GameObjects
{
    public class ScrollingBackground
    {
        private Texture2D Texture;
        private Random Rnd;
        private Color[] data;

        public ScrollingBackground(RenderTarget2D renderTarget2D)
        {
            Texture = new Texture2D(renderTarget2D.GraphicsDevice, renderTarget2D.Width, renderTarget2D.Height);

            Rnd = new Random();
            //the array holds the color for each pixel in the texture
            data = new Color[renderTarget2D.Width * renderTarget2D.Height];
            for (int pixel = 0; pixel < data.Count(); pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = Rnd.Next(0,1000) == 0 ? Color.White : Color.Black;
            }
            Texture.SetData(data);
        }

        internal void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Vector2.Zero, Color.White);
        }

        internal void Update(GameTime gameTime, RenderTarget2D renderTarget2D)
        {
            if (!((int)gameTime.ElapsedGameTime.TotalSeconds % 10 == 0))
            {
                return; // update only every n frames.
            }
            
            //the array holds the color for new row of pixels
            Color[] newRow = new Color[renderTarget2D.Width];

            for (int pixel = 0; pixel < newRow.Count(); pixel++)
            {
                //the function applies the color according to the specified pixel
                newRow[pixel] = Rnd.Next(0, 1000) == 0 ? Color.White : Color.Black;
            }
            
            for (int pixel = renderTarget2D.Width * renderTarget2D.Height-1; pixel > 0; pixel--)
            {
                if (pixel >= renderTarget2D.Width)
                {
                    // move old rows of pixels down on array
                    data[pixel] = data[pixel - renderTarget2D.Width];
                } else
                { 
                    // add newRow to the start of the array
                    data[pixel] = newRow[pixel];
                }
            }

            //set the color
            Texture.SetData(data);
        }
    }
}
