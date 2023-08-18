using GameEngine.Core.GameEngine.Utils;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman.GameObjects.tiles
{
    public class TileGate : PacmanTile
    {
        public TileGate(Vector2 position, Sprite sprite)
            : base(Globals.PacmanTiles.GATE, Globals.SPRITE_LAYER_FOREGROUND, position, sprite)
        {
        }

        public override void DebugDraw(SpriteBatch spriteBatch, TextDrawer textDrawer)
        {
            textDrawer.Draw($"G",
                Position,
                HorizontalAlignment.Center,
                VerticalAlignment.Middle);
        }
    }
}
