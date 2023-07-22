using GameEngine.Core.EntityManagement;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Pong.Globals;

namespace Pong
{
    public class Ball : Entity
    {
        public Vector2 Velocity;

        public Ball(Vector2 position, Rectangle boundingBox, Sprite sprite) : base(position, boundingBox, (float)SpriteLayers.PLAYER, BALL_BASE_SPEED, sprite)
        {
            Velocity = new Vector2(-Speed, Speed);
            Tag = "ball";
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, GraphicsDeviceManager graphics, EntityManager entityManager)
        {
            if (Position.Y - Sprite.Texture.Height/2 < 0)
            {
                Velocity.Y = -Velocity.Y;
            }

            if (Position.Y + Sprite.Texture.Height / 2 > graphics.PreferredBackBufferHeight)
            {
                Velocity.Y = -Velocity.Y;
            }

            if (Position.X - Sprite.Texture.Width / 2 < 0)
            {
                Velocity.X = -Velocity.X;
                GameStats.Instance.PlayerLives -= 1;
            }

            if (Position.X +  Sprite.Texture.Width /2 > graphics.PreferredBackBufferWidth)
            {
                Velocity.X = -Velocity.X;
                GameStats.Instance.PlayerScore += SCORE_ON_WALL_HIT;
            }

            if (IsColliding("leftPaddle", entityManager))
            {
                Velocity.X = -Velocity.X * 1.1f;
                Velocity.Y *= 1.1f;
                GameStats.Instance.PlayerScore += SCORE_ON_PADDLE_HIT;
            }

            if (IsColliding("rightPaddle", entityManager))
            {
                Velocity.X = -Velocity.X * 1.1f;
                Velocity.Y *= 1.1f;
            }

            // limit velocity
            if (Velocity.X < -BALL_MAX_SPEED)
            {
                Velocity.X = -BALL_MAX_SPEED;
            }
            if (Velocity.X > BALL_MAX_SPEED)
            {
                Velocity.X = BALL_MAX_SPEED;
            }
            if (Velocity.Y < -BALL_MAX_SPEED)
            {
                Velocity.Y = -BALL_MAX_SPEED;
            }
            if (Velocity.Y > BALL_MAX_SPEED)
            {
                Velocity.Y = BALL_MAX_SPEED;
            }

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            BoundingBox = new Rectangle((int)Position.X - BoundingBox.Width/2, (int)Position.Y - BoundingBox.Height/2, BoundingBox.Width, BoundingBox.Height);
        }

        private bool IsColliding(string tag, EntityManager entityManager)
        {
            return entityManager.IsColliding(this, tag);
        }
    }
}
