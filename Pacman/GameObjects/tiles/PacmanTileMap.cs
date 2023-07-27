using GameEngine.Core.GameEngine.FileManagement;
using GameEngine.Core.GameEngine.TileMap;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Pacman.GameObjects.tiles
{
    public class PacmanTileMap : TileMap<PacmanTile>
    {
        private Texture2D Texture;
        private string[] Levels;

        public PacmanTileMap() : base(19, 21, Globals.PACMAN_TILESIZE)
        {
        }

        public void Initialize(Texture2D texture, string[] levels)
        {
            Texture = texture;
            Levels = levels;
        }

        public void LoadLevel(int level)
        {
            var defaultWallTileTexture = new Rectangle(16, 16, Globals.PACMAN_TILESIZE, Globals.PACMAN_TILESIZE);
            var defaultFloorTileTexture = new Rectangle(48, 64, Globals.PACMAN_TILESIZE, Globals.PACMAN_TILESIZE);
            var defaultGateTileTexture = new Rectangle(64, 64, Globals.PACMAN_TILESIZE, Globals.PACMAN_TILESIZE);
            var tileData = Levels[level]
                .Replace("\r\n", ",")
                .Replace("\r", ",")
                .Replace("\n", ",")
                .Split(",");
            //Debug.WriteLine($"got tiledata with length of {tileData.Length}");

            // sets all tiles first
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var data = tileData[x + Width * y];
                    if (data == "0")
                    {
                        var tile = new TileWall(
                            new Vector2(x, y),
                            new Sprite(Texture, defaultWallTileTexture));
                        Tiles[x, y] = tile;
                    }
                    else if (data == "N")
                    {
                        var tile = new TileFloor(
                            new Vector2(x, y),
                            new Sprite(Texture, defaultFloorTileTexture)
                            );
                        Tiles[x, y] = tile;
                    }
                    else if (data == "1")
                    {
                        // dot
                    }
                    else if (data == "C")
                    {
                        // cherry  = big dot
                    }
                    else if (data == "R")
                    {
                        // red ghost
                    }
                    else if (data == "B")
                    {
                        // blue ghost
                    }
                    else if (data == "P")
                    {
                        // pink ghost
                    }
                    else if (data == "O")
                    {
                        // orange ghost
                    }
                    else if (data == "G")
                    {
                        // gate
                        var tile = new TileGate(
                            new Vector2(x, y),
                            new Sprite(Texture, defaultGateTileTexture)
                            );
                        Tiles[x, y] = tile;
                    }
                    else if (data == "X")
                    {
                        // pacman
                    }
                }
            }

            // then check wall tile textures.
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var data = tileData[x + Width * y];
                    if (data == "0")
                    {
                        var rect = GetWallTileTexturePosition(x, y);
                        Tiles[x, y].SetTextureSourceRect(rect);
                    }
                }
            }
        }

        private Rectangle GetWallTileTexturePosition(int x, int y)
        {
            var l = x - 1 < 0       ? false : Tiles[x - 1, y    ]?.TileType == (int)Globals.PacmanTiles.WALL;
            var r = x + 1 >= Width  ? false : Tiles[x + 1, y    ]?.TileType == (int)Globals.PacmanTiles.WALL;
            var t = y - 1 < 0       ? false : Tiles[x    , y - 1]?.TileType == (int)Globals.PacmanTiles.WALL;
            var b = y + 1 >= Height ? false : Tiles[x    , y + 1]?.TileType == (int)Globals.PacmanTiles.WALL;

            //var tl = x - 1 < 0 && y - 1 < 0 ? true : Tiles[x - 1, y - 1]?.TileType == (int)Globals.PacmanTiles.WALL;
            //var tr = x + 1 >= Width && y - 1 < 0 ? true : Tiles[x + 1, y - 1]?.TileType == (int)Globals.PacmanTiles.WALL;
            //var bl = x - 1 < 0 && y + 1 >= Height ? true : Tiles[x + 1, y + 1]?.TileType == (int)Globals.PacmanTiles.WALL;
            //var br = x + 1 >= Width && y + 1 >= Height ? true : Tiles[x + 1, y + 1]?.TileType == (int)Globals.PacmanTiles.WALL;

            var tz = Globals.PACMAN_TILESIZE;

            // check neighbours
            //if (tl && t && tr && r && )
            //{

            //}

            // no neighbours = single dot
            if (!t && !r && !b && !l)
            {
                return new Rectangle(32, 0, tz, tz);
            }

            // all sides = cross
            if (t && r && b && l)
            {
                return new Rectangle(16, 16, tz, tz);
            }

            // three sides
            if (!t && r && b && l)
            {
                return new Rectangle(64, 0, tz, tz);
            }
            if (t && !r && b && l)
            {
                return new Rectangle(64, 16, tz, tz);
            }
            if (t && r && !b && l)
            {
                return new Rectangle(48, 16, tz, tz);
            }
            if (t && r && b && !l)
            {
                return new Rectangle(48, 0, tz, tz);
            }

            // two adjacent sides
            if (!t && !r && b && l)
            {
                return new Rectangle(64, 32, tz, tz);
            }
            if (t && !r && !b && l)
            {
                return new Rectangle(64, 48, tz, tz);
            }
            if (t && r && !b && !l)
            {
                return new Rectangle(48, 48, tz, tz);
            }
            if (!t && r && b && !l)
            {
                return new Rectangle(48, 32, tz, tz);
            }

            // two opposite sides
            if (!t && r && !b && l)
            {
                return new Rectangle(80, 0, tz, tz);
            }
            if (t && !r && b && !l)
            {
                return new Rectangle(80, 16, tz, tz);
            }

            // one side is wall
            if (!t && !r && b && !l)
            {
                return new Rectangle(16, 0, tz, tz);
            }
            if (!t && !r && !b && l)
            {
                return new Rectangle(32, 16, tz, tz);
            }
            if (t && !r && !b && !l)
            {
                return new Rectangle(16, 32, tz, tz);
            }
            if (!t && r && !b && !l)
            {
                return new Rectangle(0, 16, tz, tz);
            }

            // default full texture
            return new Rectangle(0, 0, tz, tz);
        }
    }
}
