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

        public ScrollingBackground(GraphicsDeviceManager graphics)
        {
            Texture = new Texture2D(graphics.GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            Rnd = new Random();
            //the array holds the color for each pixel in the texture
            data = new Color[graphics.PreferredBackBufferWidth * graphics.PreferredBackBufferHeight];
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

        internal void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            if (!((int)gameTime.ElapsedGameTime.TotalSeconds % 10 == 0))
            {
                return; // update only every n frames.
            }
            
            //the array holds the color for new row of pixels
            Color[] newRow = new Color[graphics.PreferredBackBufferWidth];

            for (int pixel = 0; pixel < newRow.Count(); pixel++)
            {
                //the function applies the color according to the specified pixel
                newRow[pixel] = Rnd.Next(0, 1000) == 0 ? Color.White : Color.Black;
            }
            
            for (int pixel = graphics.PreferredBackBufferWidth * graphics.PreferredBackBufferHeight-1; pixel > 0; pixel--)
            {
                if (pixel >= graphics.PreferredBackBufferWidth)
                {
                    // move old rows of pixels down on array
                    data[pixel] = data[pixel - graphics.PreferredBackBufferWidth];
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
