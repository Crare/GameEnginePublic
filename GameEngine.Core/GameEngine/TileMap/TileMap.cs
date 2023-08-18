using System;
using System.Collections.Generic;
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
        protected readonly RenderTarget2D _target;

        public TileMap(int width, int height, int tileSize) {
            Tiles = new TTile[width, height];
            Width = width;
            Height = height;
            TileSize = tileSize;

            _target = new(CoreGlobals.GraphicsDevice, Width * TileSize, Height * TileSize);
        }

        /// <summary>
        /// Renders tilemap to RenderTarget2D for later use.
        /// </summary>
        public virtual void PreRenderTileMap()
        {
            CoreGlobals.GraphicsDevice.SetRenderTarget(_target);
            CoreGlobals.GraphicsDevice.Clear(Color.Transparent);
            CoreGlobals.SpriteBatch.Begin();

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Tiles[x, y] != null)
                    {
                        Tiles[x, y].Draw(CoreGlobals.SpriteBatch);
                    }
                }
            }
            CoreGlobals.SpriteBatch.End();
            CoreGlobals.GraphicsDevice.SetRenderTarget(null);
        }

        public virtual void DrawTiles()
        {
            // TODO: maybe needs camera offset?
            CoreGlobals.SpriteBatch.Draw(_target, Vector2.Zero, Color.White);
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
                        Tiles[x, y].DebugDraw(CoreGlobals.SpriteBatch, textDrawer);
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
            //PreRenderTileMap();
        }

        public Point WorldPositionToTilePosition(Vector2 worldPosition)
        {
            //return new Point((int)Math.Round(worldPosition.X / TileSize), (int)Math.Round(worldPosition.Y / TileSize));
            return new Point((int)Math.Floor(worldPosition.X / TileSize), (int)Math.Floor(worldPosition.Y / TileSize));
        }

        public Point WorldPositionToTilePositionWithCeiling(Vector2 worldPosition)
        {
            return new Point((int)Math.Ceiling(worldPosition.X / TileSize), (int)Math.Round(worldPosition.Y / TileSize));
            //return new Point((int)Math.Floor(worldPosition.X / TileSize), (int)Math.Floor(worldPosition.Y / TileSize));
        }

        public Vector2 TilePositionToWorldPosition(Point tilePosition)
        {
            return new Vector2(tilePosition.X * TileSize, tilePosition.Y * TileSize);
        }

        public List<TTile> GetTilesInsideWorldPositionRectangle(Rectangle rect)
        {
            var tilePos = WorldPositionToTilePosition(new Vector2(rect.X - TileSize, rect.Y - TileSize));
            var tilePos2 = WorldPositionToTilePosition(new Vector2(rect.X + rect.Width + TileSize, rect.Y + rect.Height + TileSize));
            int minY = Math.Min(tilePos.Y, tilePos2.Y);
            int maxY = Math.Max(tilePos.Y, tilePos2.Y);
            int minX = Math.Min(tilePos.X, tilePos2.X);
            int maxX = Math.Max(tilePos.X, tilePos2.X);
            var tiles = new List<TTile>();
            for (int y = minY; y < maxY; y++)
            {
                for (int x = minX; x < maxX; x++)
                {
                    if (x < 0 || x >= Width || y < 0 || y >= Height)
                    {
                        continue;
                    }
                    if (Tiles[x, y] != null)
                    {
                        var tile = Tiles[x, y];
                        tiles.Add(tile);
                    }
                }
            }
            return tiles;
        }

        public Rectangle TileWorldPositionBounds(TTile tile)
        {
            return new Rectangle(
                (int)tile.Position.X,
                (int)tile.Position.Y,
                TileSize,
                TileSize
                );
        }

        public Rectangle TileWorldPositionBounds(Point tilePosition)
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
