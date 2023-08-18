using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman.GameObjects.tiles
{
    public class TileFloor : PacmanTile
    {
        public TileFloor(Vector2 position, Sprite sprite)
            : base(Globals.PacmanTiles.FLOOR, Globals.SPRITE_LAYER_BACKGROUND, position, sprite)
        {

        }
    }
}
