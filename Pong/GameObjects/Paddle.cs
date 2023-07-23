using GameEngine.Core.EntityManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static Pong.Globals;

namespace Pong
{
    public class Paddle : Entity
    {
        private Vector2 startingPosition;
        private readonly Texture2D Texture;
        private readonly int Width, Height;
        private readonly bool IsPlayerControlled;

        public Paddle(Texture2D texture, int width, int height, Vector2 position, string tag, Rectangle boundingBox, bool isPlayerControlled) 
            : base(position, boundingBox, (float)SpriteLayers.PLAYER, PADDLE_BASE_SPEED)
        {
            startingPosition = position;
            Texture = texture;
            Width = width;
            Height = height;
            Tag = tag;
            IsPlayerControlled = isPlayerControlled;

            PongEventSystem.OnGameOver += OnGameOver;
        }
        private void OnGameOver()
        {
            Position = startingPosition;
            BoundingBox = new Rectangle((int)Position.X - BoundingBox.Width / 2, (int)Position.Y - BoundingBox.Height / 2, BoundingBox.Width, BoundingBox.Height);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            // draws rectangle
            spriteBatch.Draw(
                    Texture,
                    Position,
                    new Rectangle(0, 0, Width, Height),
                    Color.Blue,
                    0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    DepthLayer
                );
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, GraphicsDeviceManager graphics, EntityManager entityManager)
        {
            if (IsPlayerControlled)
            {
                PlayerInput(gameTime, keyboardState, graphics);
            } else
            {
                AIControls(gameTime, entityManager);
            }
            
        }

        private void AIControls(GameTime gameTime, EntityManager entityManager)
        {
            var ball = (Ball)entityManager.GetEntityByTag("ball");
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

        private void PlayerInput(GameTime gameTime, KeyboardState keyboardState, GraphicsDeviceManager graphics)
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
            if (Position.Y > graphics.PreferredBackBufferHeight - Height)
            {
                yPos = graphics.PreferredBackBufferHeight - Height;
            }
            else if (Position.Y < 0)
            {
                yPos = 0;
            }
            Position = new Vector2(Position.X, yPos);
            
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

        }

    }
}
