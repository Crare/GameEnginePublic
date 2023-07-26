using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman.GameObjects
{
    public class TileWall : PacmanTile
    {
        public TileWall(Vector2 position, Sprite sprite)
            : base(Globals.PacmanTiles.WALL, Globals.SpriteLayers.BACKGROUND, position, sprite)
        {
            
        }
    }
}
