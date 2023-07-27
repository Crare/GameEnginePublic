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
        private Rectangle[] textureSources;

        public PacmanTileMap() : base(19, 21, Globals.PACMAN_TILESIZE)
        {
        }

        public void Initialize(Texture2D texture, string[] levels)
        {
            Texture = texture;
            Levels = levels;

            var tz = Globals.PACMAN_TILESIZE;
            textureSources = new Rectangle[30];
            var i = 0;
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    textureSources[i] = new Rectangle(x * tz, y * tz, tz, tz);
                    i++;
                }
            }
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
                        var textureIndex = GetWallTileTextureIndex(x, y);
                        var wallTile = (TileWall)Tiles[x, y];
                        var rect = textureSources[textureIndex];
                        wallTile.SetTextureSourceRect(rect);
                        wallTile.WallNeighbourIndex = textureIndex;
                        Tiles[x, y] = wallTile;
                    }
                }
            }
        }

        private int GetWallTileTextureIndex(int x, int y)
        {
            var l = x - 1 < 0       ? false : Tiles[x - 1, y    ]?.TileType == (int)Globals.PacmanTiles.WALL;
            var r = x + 1 >= Width  ? false : Tiles[x + 1, y    ]?.TileType == (int)Globals.PacmanTiles.WALL;
            var t = y - 1 < 0       ? false : Tiles[x    , y - 1]?.TileType == (int)Globals.PacmanTiles.WALL;
            var b = y + 1 >= Height ? false : Tiles[x    , y + 1]?.TileType == (int)Globals.PacmanTiles.WALL;

            var tl = x - 1 < 0 || y - 1 < 0 ? false : Tiles[x - 1, y - 1]?.TileType == (int)Globals.PacmanTiles.WALL;
            var tr = x + 1 >= Width || y - 1 < 0 ? false : Tiles[x + 1, y - 1]?.TileType == (int)Globals.PacmanTiles.WALL;
            var bl = x - 1 < 0 || y + 1 >= Height ? false : Tiles[x - 1, y + 1]?.TileType == (int)Globals.PacmanTiles.WALL;
            var br = x + 1 >= Width || y + 1 >= Height ? false : Tiles[x + 1, y + 1]?.TileType == (int)Globals.PacmanTiles.WALL;

            var tz = Globals.PACMAN_TILESIZE;

            // no neighbours = single dot
            if (!t && !r && !b && !l)
            {
                return 1;
            }

            // all 8 sides.
            if (tl && t && tr && r && br && b && bl)
            {
                return 0;
            }

            // two sides and their adjacent tile are walls
            if (tl && t && !r && !b && l)
            {
                return 25;
            }
            if (t && tr && r && !b && !l)
            {
                return 24;
            }
            if (!t && r && br && b && !l)
            {
                return 18;
            }
            if (!t && !r && b && bl && l)
            {
                return 19;
            }

            // all sides = cross
            if (t && r && b && l)
            {
                return 7;
            }

            // three sides
            if (!t && r && b && l)
            {
                return 4;
            }
            if (t && !r && b && l)
            {
                return 10;
            }
            if (t && r && !b && l)
            {
                return 9;
            }
            if (t && r && b && !l)
            {
                return 3;
            }

            // two adjacent sides
            if (!t && !r && b && l)
            {
                return 16;
            }
            if (t && !r && !b && l)
            {
                return 22;
            }
            if (t && r && !b && !l)
            {
                return 21;
            }
            if (!t && r && b && !l)
            {
                return 15;
            }

            // two opposite sides
            if (!t && r && !b && l)
            {
                return 5;
            }
            if (t && !r && b && !l)
            {
                return 11;
            }

            // one side is wall
            if (!t && !r && b && !l)
            {
                return 1;
            }
            if (!t && !r && !b && l)
            {
                return 8;
            }
            if (t && !r && !b && !l)
            {
                return 13;
            }
            if (!t && r && !b && !l)
            {
                return 6;
            }

            // default full texture
            return 0;
        }
    }
}
