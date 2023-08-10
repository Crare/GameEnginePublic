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
        protected SpriteBatch SpriteBatch;

        public TileMap(int width, int height, int tileSize, SpriteBatch spriteBatch) { 
            Tiles = new TTile[width, height];
            Width = width;
            Height = height;
            TileSize = tileSize;
            SpriteBatch = spriteBatch;
        }

        public virtual void DrawTiles()
        {
            for(var x = 0; x < Width; x++)
            {
                for(var y = 0; y < Height; y++)
                {
                    if (Tiles[x, y] != null)
                    {
                        Tiles[x, y].Draw(SpriteBatch);
                    }
                }
            }
        }

        public virtual void UpdateTiles(GameTime gameTime)
        {
            //for (var x = 0; x < Width; x++)
            //{
            //    for (var y = 0; y < Height; y++)
            //    {
            //        Tiles[x, y]?.Update(gameTime);
            //    }
            //}
        }

        public virtual void DebugDrawTiles(TextDrawer textDrawer)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Tiles[x, y] != null)
                    {
                        Tiles[x, y].DebugDraw(SpriteBatch, textDrawer);
                    }
                }
            }
        }

        public TTile GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x > Tiles.GetUpperBound(0) || y > Tiles.GetUpperBound(1))
            {
                return null;
            }
            return Tiles[x, y];
        }

        public void SetTile(int x, int y, TTile tile)
        {
            if (x < 0 || y < 0 || x > Tiles.GetUpperBound(0) - 1 || y > Tiles.GetUpperBound(1) - 1)
            {
                throw new Exception("outside tilemap area");
            }
            Tiles[x, y] = tile;
        }

        public Point WorldPositionToTilePosition(Vector2 worldPosition)
        {
            //return new Point((int)Math.Round(worldPosition.X / TileSize), (int)Math.Round(worldPosition.Y / TileSize));
            return new Point((int)Math.Floor(worldPosition.X / TileSize), (int)Math.Floor(worldPosition.Y / TileSize));
        }

        public Vector2 TilePositionToWorldPosition(Point tilePosition)
        {
            return new Vector2(tilePosition.X * TileSize, tilePosition.Y * TileSize);
        }

        public Rectangle TilePositionBounds(TTile tile)
        {
            return new Rectangle(
                (int)tile.Position.X,
                (int)tile.Position.Y,
                TileSize,
                TileSize
                );
        }

        public Rectangle TilePositionBounds(Point tilePosition)
        {
            return new Rectangle(
                tilePosition.X * TileSize,
                tilePosition.Y * TileSize,
                TileSize,
                TileSize
                );
        }
    }
}
