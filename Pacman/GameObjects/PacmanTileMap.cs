using GameEngine.Core.GameEngine.FileManagement;
using GameEngine.Core.GameEngine.TileMap;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Pacman.GameObjects
{
    public class PacmanTileMap : TileMap<PacmanTile>
    {
        private Texture2D Texture;
        private string[] Levels;

        public PacmanTileMap() : base(19, 19, Globals.PACMAN_TILESIZE)
        {
        }

        public void Initialize(Texture2D texture, string[] levels)
        {
            Texture = texture;
            Levels = levels;
        }

        public void LoadLevel(int level)
        {
            var tileData = Levels[level]
                .Replace("\r\n", ",")
                .Replace("\r", ",")
                .Replace("\n", ",")
                .Split(",");
            //Debug.WriteLine($"got tiledata with length of {tileData.Length}");
            for(var x = 0; x < Width; x++)
            {
                for(var y = 0; y < Height; y++)
                {
                    var data = tileData[x + Width * y];
                    if (data == "0")
                    {
                        var rect = GetWallTileTexturePosition(x, y);
                        var tile = new TileWall(new Vector2(x,y),
                            new Sprite(Texture, rect));
                        Tiles[x, y] = tile;
                    }
                }
            }
        }

        private Rectangle GetWallTileTexturePosition(int x, int y)
        {
            //var left = x - 1 < 0 ? true;
            //var right = x + 1;
            //var top = y - 1;
            //var bottom = y + 1;
            var tz = Globals.PACMAN_TILESIZE;
            var rect = new Rectangle(16,0, tz, tz);
            return rect;
        }
    }
}
