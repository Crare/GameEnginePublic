using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.FileManagement;
using GameEngine.Core.GameEngine.TileMap;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pacman.GameObjects.tiles
{
    public class PacmanTileMap : TileMap<PacmanTile>
    {
        private Texture2D Texture;
        private string[] Levels;
        private Rectangle[] textureSources;
        private EntityManager EntityManager;
        private List<Entity> tileMapEntities = new();

        public PacmanTileMap(EntityManager entityManager) 
            : base(19, 21, Globals.PACMAN_TILESIZE)
        {
            EntityManager = entityManager;
            PacmanEventSystem.OnGameOver += OnGameOver;
        }

        private void OnGameOver()
        {
            ClearLevel();
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

        public void ClearLevel()
        {
            if (Tiles != null)
            {
                tileMapEntities.ForEach(entity =>
                    EntityManager.RemoveEntity(entity.Id)
                );
                tileMapEntities = new();
                Tiles = new PacmanTile[Width, Height];
            }
        }

        public void LoadLevel(int level)
        {
            ClearLevel();

            var defaultWallTileTexture = new Rectangle(16, 16, Globals.PACMAN_TILESIZE, Globals.PACMAN_TILESIZE);
            var defaultFloorTileTexture = new Rectangle(48, 64, Globals.PACMAN_TILESIZE, Globals.PACMAN_TILESIZE);
            var defaultGateTileTexture = new Rectangle(64, 64, Globals.PACMAN_TILESIZE, Globals.PACMAN_TILESIZE);
            var tileData = Levels[level]
                .Replace("\r\n", ",")
                .Replace("\r", ",")
                .Replace("\n", ",")
                .Split(",");
            //Debug.WriteLine($"got tiledata with length of {tileData.Length}");

            var dotSmall = EntityManager.GetEntityByTag<SmallDot>((int)Globals.PacmanTags.DotSmall);
            var dotBig = EntityManager.GetEntityByTag<BigDot>((int)Globals.PacmanTags.DotBig);

            // sets all tiles first
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var data = tileData[x + Width * y];
                    if (data == "1"
                        || data == "N"
                        || data == "C"
                        || data == "R"
                        || data == "O"
                        || data == "B"
                        || data == "P"
                        || data == "X")
                    {
                        // floor tile
                        var tile = new TileFloor(
                            new Vector2(x, y),
                            new Sprite(Texture, defaultFloorTileTexture)
                            );
                        Tiles[x, y] = tile;
                    }
                    else if (data == "0")
                    {
                        var tile = new TileWall(
                            new Vector2(x, y),
                            new Sprite(Texture, defaultWallTileTexture));
                        Tiles[x, y] = tile;
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

                    if (data == "1")
                    {
                        var dotSmall2 = dotSmall.Copy();
                        dotSmall2.Position = new Vector2(x * TileSize, y * TileSize);
                        EntityManager.AddEntity(dotSmall2);
                        tileMapEntities.Add(dotSmall2);
                        dotSmall2.Collider.UpdatePosition(dotSmall2.Position);
                    }
                    else if (data == "C")
                    {
                        var dotBig2 = dotBig.Copy();
                        dotBig2.Position = new Vector2(x * TileSize, y * TileSize);
                        EntityManager.AddEntity(dotBig2);
                        tileMapEntities.Add(dotBig2);
                        dotBig2.Collider.UpdatePosition(dotBig2.Position);
                    }
                    else if (data == "R")
                    {
                        var ghost = EntityManager.GetEntityByTag<RedGhost>((int)Globals.PacmanTags.RedGhost);
                        ghost.Position = new Vector2(x * TileSize, y * TileSize);
                        ghost.Collider.UpdatePosition(ghost.Position);
                        ghost.Restart();
                    }
                    else if (data == "B")
                    {
                        var ghost = EntityManager.GetEntityByTag<BlueGhost>((int)Globals.PacmanTags.BlueGhost);
                        ghost.Position = new Vector2(x * TileSize, y * TileSize);
                        ghost.Collider.UpdatePosition(ghost.Position); 
                        ghost.Restart();
                    }
                    else if (data == "P")
                    {
                        var ghost = EntityManager.GetEntityByTag<PinkGhost>((int)Globals.PacmanTags.PinkGhost);
                        ghost.Position = new Vector2(x * TileSize, y * TileSize);
                        ghost.Collider.UpdatePosition(ghost.Position);
                        ghost.Restart();
                    }
                    else if (data == "O")
                    {
                        var ghost = EntityManager.GetEntityByTag<OrangeGhost>((int)Globals.PacmanTags.OrangeGhost);
                        ghost.Position = new Vector2(x * TileSize, y * TileSize);
                        ghost.Collider.UpdatePosition(ghost.Position);
                        ghost.Restart();
                    }
                    else if (data == "X")
                    {
                        var pacman = EntityManager.GetEntityByTag<PacmanEntity>((int)Globals.PacmanTags.Pacman);
                        pacman.Position = new Vector2(x * TileSize, y * TileSize);
                        pacman.Collider.UpdatePosition(pacman.Position);
                        pacman.Restart();
                    }
                }
            }

            //EntityManager.RemoveEntity(dotSmall.Id);
            //EntityManager.RemoveEntity(dotBig.Id);

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

            PreRenderTileMap();
            PacmanEventSystem.LevelLoaded(level);
        }

        private bool IsWallOrGateTile(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return false;
            }
            return Tiles[x, y]?.TileType == (int)Globals.PacmanTiles.WALL || Tiles[x, y]?.TileType == (int)Globals.PacmanTiles.GATE;
        }

        private int GetWallTileTextureIndex(int x, int y)
        {
            var l = IsWallOrGateTile(x - 1, y    );
            var r = IsWallOrGateTile(x + 1, y    );
            var t = IsWallOrGateTile(x    , y - 1);
            var b = IsWallOrGateTile(x    , y + 1);

            var tl = IsWallOrGateTile(x - 1, y - 1);
            var tr = IsWallOrGateTile(x + 1, y - 1);
            var bl = IsWallOrGateTile(x - 1, y + 1);
            var br = IsWallOrGateTile(x + 1, y + 1);

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
