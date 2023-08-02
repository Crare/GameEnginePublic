using System;
using GameEngine.Core.GameEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core.GameEngine.TileMap
{
    public class TileMap<TTile> where TTile : Tile
    {
        public TTile[,] Tiles;
        public int Width, Height;
        public int TileSize;

        public TileMap(int width, int height, int tileSize) { 
            Tiles = new TTile[width, height];
            Width = width;
            Height = height;
            TileSize = tileSize;
        }

        public void DrawTiles(SpriteBatch spriteBatch)
        {
            for(var x = 0; x < Width; x++)
            {
                for(var y = 0; y < Height; y++)
                {
                    if (Tiles[x, y] != null)
                    {
                        Tiles[x, y].Draw(spriteBatch);
                    }
                }
            }
        }

        public void DebugDrawTiles(SpriteBatch spriteBatch, TextDrawer textDrawer)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Tiles[x, y] != null)
                    {
                        Tiles[x, y].DebugDraw(spriteBatch, textDrawer);
                    }
                }
            }
        }

        public void UpdateTiles(GameTime gameTime)
        {
            //for (var x = 0; x < Width; x++)
            //{
            //    for (var y = 0; y < Height; y++)
            //    {
            //        Tiles[x, y]?.Update(gameTime);
            //    }
            //}
        }

        public Point WorldPositionToTilePosition(Vector2 worldPosition)
        {
            return new Point((int)Math.Round(worldPosition.X / TileSize), (int)Math.Round(worldPosition.Y / TileSize));
        }

        public Vector2 TilePositionToWorldPosition(Point tilePosition)
        {
            return new Vector2(tilePosition.X * TileSize, tilePosition.Y * TileSize);
        }
    }
}
