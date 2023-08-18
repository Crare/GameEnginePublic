using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core.GameEngine.TileMap
{
    public abstract class Tile
    {
        public int TileType;
        public int X, Y;
        /// <summary>
        /// Tile's world position
        /// </summary>
        public Vector2 Position;
        public float DepthLayer;

        public Tile(int tileType, Vector2 position, float depthLayer, int tileSize)
        {
            TileType = tileType;
            Position = new Vector2 (position.X * tileSize, position.Y * tileSize);
            X = (int)position.X;
            Y = (int)position.Y;
            DepthLayer = depthLayer;
        }

        public Tile(int tileType, int x, int y, float depthLayer, int tileSize)
        {
            TileType = tileType;
            Position = new Vector2(x * tileSize, y * tileSize);
            X = x;
            Y = y;
            DepthLayer = depthLayer;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void DebugDraw(SpriteBatch spriteBatch, TextDrawer textDrawer);
        public abstract void Update(GameTime gameTime);
    }
}
