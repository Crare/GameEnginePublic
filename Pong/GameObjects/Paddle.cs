using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Collision;
using GameEngine.Core.GameEngine.Sprites;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class PlayerPaddle : Paddle
    {
        public PlayerPaddle(Sprite sprite, Vector2 position) 
            : base(sprite, position, Globals.PongTags.leftPaddle)
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
            if (Position.Y > renderTarget2D.Height - Collider.BoundingBox.Height / 2)
            {
                yPos = renderTarget2D.Height - Collider.BoundingBox.Height / 2;
            }
            else if (Position.Y < Collider.BoundingBox.Height / 2)
            {
                yPos = Collider.BoundingBox.Height / 2;
            }
            Position = new Vector2(Position.X, yPos);

            Collider.UpdatePosition(Position);
        }
    }

    public class AIPaddle : Paddle
    {
        public AIPaddle(Sprite sprite, Vector2 position)
            : base(sprite, position, Globals.PongTags.rightPaddle)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            var ball = entityManager.GetEntityByTag<Ball>((int)Globals.PongTags.ball);
            if (ball != null)
            {
                if (ball.Velocity.X > 0) // this is assuming right paddle is AI always. otherwise needs more checks.
                {
                    var newPosY = Position.Y;
                    // coming towards right paddle, start moving.
                    var yVelocity = 0;
                    if (ball.Position.Y > Position.Y + Collider.BoundingBox.Height / 2 + 5)
                    {
                        yVelocity = 1;
                    }
                    if (ball.Position.Y < Position.Y + Collider.BoundingBox.Height / 2 - 5)
                    {
                        yVelocity = -1;
                    }

                    newPosY += yVelocity * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Position = new Vector2(Position.X, newPosY);

                    // keep in window limits
                    float yPos = Position.Y;
                    if (Position.Y > renderTarget2D.Height - Collider.BoundingBox.Height / 2)
                    {
                        yPos = renderTarget2D.Height - Collider.BoundingBox.Height / 2;
                    }
                    else if (Position.Y < Collider.BoundingBox.Height / 2)
                    {
                        yPos = Collider.BoundingBox.Height / 2;
                    }
                    Position = new Vector2(Position.X, yPos);
                    Collider.UpdatePosition(Position);
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
                    Collider.UpdatePosition(Position);
                }
            }
        }
    }

    public class Paddle : Entity, IHasSprite, ICollidable
    {
        internal Vector2 startingPosition;
        public Sprite Sprite { get; set; }
        public bool HorizontalFlipped { get; set; }
        public BoxCollider Collider { get; set; }
        public float DepthLayer { get ; set; }
        public float Speed { get; set; }

        public Paddle(Sprite sprite, Vector2 position, Globals.PongTags tag)
            : base(position, (int)tag)
        {
            startingPosition = position;
            Collider = new BoxCollider(new Rectangle((int)position.X, (int)position.Y, 16, 96));
            Sprite = sprite;
            Speed = Globals.PADDLE_BASE_SPEED;

            PongEventSystem.OnGameOver += OnGameOver;
            DepthLayer = (float)Globals.SpriteLayers.PLAYER;
        }

        private void OnGameOver()
        {
            Position = startingPosition;
            Collider.UpdatePosition(Position);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Draw(
                spriteBatch, 
                new Vector2(Position.X, Position.Y), 
                DepthLayer,
                HorizontalFlipped);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
        }

        public override void DebugDraw(SpriteBatch spriteBatch, Color debugColor)
        {
            Collider.DebugDraw(spriteBatch, debugColor);
        }
    }
}
