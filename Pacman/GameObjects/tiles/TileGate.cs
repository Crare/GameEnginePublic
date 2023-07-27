using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;

namespace Pacman.GameObjects.tiles
{
    public class TileGate : PacmanTile
    {
        public TileGate(Vector2 position, Sprite sprite)
            : base(Globals.PacmanTiles.GATE, Globals.SpriteLayers.FOREGROUND, position, sprite)
        {
        }
    }
}
