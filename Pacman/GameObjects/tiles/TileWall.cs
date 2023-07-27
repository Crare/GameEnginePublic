using GameEngine.Core.GameEngine.Utils;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman.GameObjects.tiles
{
    public class TileWall : PacmanTile
    {
        public int WallNeighbourIndex = -1;

        public TileWall(Vector2 position, Sprite sprite)
            : base(Globals.PacmanTiles.WALL, Globals.SpriteLayers.BACKGROUND, position, sprite)
        {

        }


        public override void DebugDraw(SpriteBatch spriteBatch, TextDrawer textDrawer)
        {
            textDrawer.Draw($"{WallNeighbourIndex}",
                Position,
                Alignment.Center);
        }
    }
}
