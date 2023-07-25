using GameEngine.Core.EntityManagement;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Pong.Globals;

namespace Pong
{
    public class PlayerPaddle : Paddle
    {
        public PlayerPaddle(Sprite sprite, Vector2 position, Rectangle boundingBox) 
            : base(sprite, position, PongTags.leftPaddle, boundingBox)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
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

            if (velocity.X != 0 || velocity.Y != 0)
            {
                velocity.Normalize(); // normalize so diagonals are not faster to move
                Position += velocity * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // keep in window limits
            float yPos = Position.Y;
            if (Position.Y > renderTarget2D.Height - Height)
            {
                yPos = renderTarget2D.Height - Height;
            }
            else if (Position.Y < 0)
            {
                yPos = 0;
            }
            Position = new Vector2(Position.X, yPos);

            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }
    }

    public class AIPaddle : Paddle
    {
        public AIPaddle(Sprite sprite, Vector2 position, Rectangle boundingBox)
            : base(sprite, position, PongTags.rightPaddle, boundingBox)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            var ball = entityManager.GetEntityByTag<Ball>((int)PongTags.ball);
            if (ball != null)
            {
                if (ball.Velocity.X > 0) // this is assuming right paddle is AI always. otherwise needs more checks.
                {
                    // coming towards right paddle, start moving.
                    var yVelocity = 0;
                    if (ball.Position.Y > Position.Y + Height / 2)
                    {
                        yVelocity = 1;
                    }
                    if (ball.Position.Y < Position.Y + Height / 2)
                    {
                        yVelocity = -1;
                    }
                    Position = new Vector2(Position.X, Position.Y + (yVelocity * (Speed / 2) * (float)gameTime.ElapsedGameTime.TotalSeconds));
                    BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, BoundingBox.Width, BoundingBox.Height);
                }
            }
        }
    }

    public class Paddle : Entity
    {
        internal Vector2 startingPosition;
        internal readonly int Width, Height;

        public Paddle(Sprite sprite, Vector2 position, PongTags tag, Rectangle boundingBox)
            : base(position, boundingBox, (float)SpriteLayers.PLAYER, PADDLE_BASE_SPEED, sprite)
        {
            startingPosition = position;
            Width = boundingBox.Width;
            Height = boundingBox.Height;
            Tag = (int)tag;

            PongEventSystem.OnGameOver += OnGameOver;
        }

        private void OnGameOver()
        {
            Position = startingPosition;
            BoundingBox = new Rectangle((int)Position.X - BoundingBox.Width / 2, (int)Position.Y - BoundingBox.Height / 2, BoundingBox.Width, BoundingBox.Height);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            Sprite.Render(
                spriteBatch, 
                new Vector2(Position.X + Width / 2, Position.Y + Height / 2), 
                DepthLayer, false);
        }
    }
}
