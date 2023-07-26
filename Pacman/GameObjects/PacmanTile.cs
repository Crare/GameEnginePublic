using GameEngine.Core.GameEngine.TileMap;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Pacman.Globals;

namespace Pacman.GameObjects
{
    public class PacmanTile : Tile
    {
        private Sprite Sprite;

        public PacmanTile(PacmanTiles tileType, SpriteLayers layer, Vector2 position, Sprite sprite)
            : base((int)tileType, position, (float)layer, PACMAN_TILESIZE)
        {
            Sprite = sprite;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, Position, DepthLayer);
        }
    }
}
