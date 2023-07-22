using GameEngine.Core.EntityManagement;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static Pong.Globals;

namespace Pong
{
    public class Player : Entity
    {
        public Player(Vector2 position, Sprite sprite, Rectangle boundingBox)
            : base(position, boundingBox, (float)SpriteLayers.PLAYER, PLAYER_BASE_SPEED, sprite)
        {

        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, GraphicsDeviceManager graphics, EntityManager entityManager)
        {
            var velocity = new Vector2(0, 0);

            // get input
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) // TODO: add gamepad support
            {
                velocity.Y = -1;
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                velocity.Y = 1;
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                velocity.X = -1;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                velocity.X = 1;
            }

            if (velocity.X != 0 || velocity.Y != 0)
            {
                velocity.Normalize(); // normalize so diagonals are not faster to move
                Position += velocity * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // keep in window limits
            float x = 0f, y = 0f;
            if (Position.X > graphics.PreferredBackBufferWidth - Sprite.Texture.Width / 2)
            {
                x = graphics.PreferredBackBufferWidth - Sprite.Texture.Width / 2;
            }
            else if (Position.X < Sprite.Texture.Width / 2)
            {
                x = Sprite.Texture.Width / 2;
            }

            if (Position.Y > graphics.PreferredBackBufferHeight - Sprite.Texture.Height / 2)
            {
                y = graphics.PreferredBackBufferHeight - Sprite.Texture.Height / 2;
            }
            else if (Position.Y < Sprite.Texture.Height / 2)
            {
                y = Sprite.Texture.Height / 2;
            }

            if (x != 0f)
            {
                Position = new Vector2(x, Position.Y);
            }
            if (y != 0f)
            {
                Position = new Vector2(Position.X, y);
            }
        }
    }
}
