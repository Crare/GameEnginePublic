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
        public PlayerPaddle(Sprite sprite, Vector2 position) 
            : base(sprite, position, PongTags.leftPaddle)
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
            if (Position.Y > renderTarget2D.Height - BoundingBox.Height / 2)
            {
                yPos = renderTarget2D.Height - BoundingBox.Height / 2;
            }
            else if (Position.Y < BoundingBox.Height / 2)
            {
                yPos = BoundingBox.Height / 2;
            }
            Position = new Vector2(Position.X, yPos);

            BoundingBox = new Rectangle((int)Position.X - BoundingBox.Width/2, (int)Position.Y- BoundingBox.Height/2, BoundingBox.Width, BoundingBox.Height);
        }
    }

    public class AIPaddle : Paddle
    {
        public AIPaddle(Sprite sprite, Vector2 position)
            : base(sprite, position, PongTags.rightPaddle)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            var ball = entityManager.GetEntityByTag<Ball>((int)PongTags.ball);
            if (ball != null)
            {
                if (ball.Velocity.X > 0) // this is assuming right paddle is AI always. otherwise needs more checks.
                {
                    var newPosY = Position.Y;
                    // coming towards right paddle, start moving.
                    var yVelocity = 0;
                    if (ball.Position.Y > Position.Y + BoundingBox.Height / 2 + 5)
                    {
                        yVelocity = 1;
                    }
                    if (ball.Position.Y < Position.Y + BoundingBox.Height / 2 - 5)
                    {
                        yVelocity = -1;
                    }

                    newPosY += yVelocity * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Position = new Vector2(Position.X, newPosY);

                    // keep in window limits
                    float yPos = Position.Y;
                    if (Position.Y > renderTarget2D.Height - BoundingBox.Height / 2)
                    {
                        yPos = renderTarget2D.Height - BoundingBox.Height / 2;
                    }
                    else if (Position.Y < BoundingBox.Height / 2)
                    {
                        yPos = BoundingBox.Height / 2;
                    }
                    Position = new Vector2(Position.X, yPos);
                    UpdateBoundingBoxPosition();
                }
                else
                {
                    var newPosY = Position.Y;
                    var yVelocity = 0;
                    // go towards the center
                    if (Position.Y < renderTarget2D.Height / 2 - 5)
                    {
                        yVelocity = 1;
                    }
                    else if (Position.Y > renderTarget2D.Height / 2 + 5)
                    {
                        yVelocity = -1;
                    }

                    newPosY = newPosY + yVelocity * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Position = new Vector2(Position.X, newPosY);
                    UpdateBoundingBoxPosition();
                }
            }
        }
    }

    public class Paddle : Entity
    {
        internal Vector2 startingPosition;

        public Paddle(Sprite sprite, Vector2 position, PongTags tag)
            : base(position, new Rectangle((int)position.X - 8, (int)position.Y - 48, 16, 96), (float)SpriteLayers.PLAYER, PADDLE_BASE_SPEED, sprite, (int)tag)
        {
            startingPosition = position;

            PongEventSystem.OnGameOver += OnGameOver;
        }

        private void OnGameOver()
        {
            Position = startingPosition;
            BoundingBox =new Rectangle((int)Position.X - 8, (int)Position.Y - 48, 16, 96);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Draw(
                spriteBatch, 
                new Vector2(Position.X, Position.Y), 
                DepthLayer,
                HorizontalFlipped);
        }

        internal void UpdateBoundingBoxPosition()
        {
            BoundingBox = new Rectangle((int)Position.X - BoundingBox.Width / 2, (int)Position.Y - BoundingBox.Height / 2, BoundingBox.Width, BoundingBox.Height);
        }
    }
}
