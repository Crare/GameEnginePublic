using GameEngine.Core.GameEngine.TileMap;
using GameEngine.Core.GameEngine.Utils;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Pacman.Globals;

namespace Pacman.GameObjects.tiles
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

        public override void DebugDraw(SpriteBatch spriteBatch, TextDrawer textDrawer)
        {
            //textDrawer.Draw($"{Position.X}, {Position.Y}",
            //    Position,
            //    Alignment.Center);
        }

        public void SetTextureSourceRect(Rectangle sourceRectangle)
        {
            Sprite.SourceRectangle = sourceRectangle;
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
