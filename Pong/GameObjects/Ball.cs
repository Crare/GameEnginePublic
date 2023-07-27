﻿using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Audio;
using GameEngine.Core.GameEngine.Particles;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Pong.Globals;

namespace Pong
{
    public class Ball : Entity
    {
        private Vector2 startingPosition;
        public Vector2 Velocity;
        private Particle particle;

        public Ball(Vector2 position, Rectangle boundingBox, Sprite sprite) 
            : base(position, boundingBox, (float)SpriteLayers.PLAYER, BALL_BASE_SPEED, sprite, (int)PongTags.ball)
        {
            startingPosition = position;
            Velocity = new Vector2(-Speed, Speed);
            particle = new Particle(1, Sprite.Texture, new Vector2(4, 4), Position, Color.White, DepthLayer, sprite.SourceRectangle.Value);

            PongEventSystem.OnGameOver += OnGameOver;
        }

        private void OnGameOver()
        {
            Position = startingPosition;
            BoundingBox = new Rectangle((int)Position.X - BoundingBox.Width / 2, (int)Position.Y - BoundingBox.Height / 2, BoundingBox.Width, BoundingBox.Height);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            if (Position.Y - BoundingBox.Height/2 < 0)
            {
                // ball hit top wall
                Velocity.Y = -Velocity.Y;
                var hitPosition = new Vector2(Position.X + BoundingBox.Width / 2, Position.Y);
                SpawnParticlesAtPosition(hitPosition, 5);
                AudioManager.Instance.PlaySound((int)PongSoundEffects.BallHit);
            }

            if (Position.Y + BoundingBox.Height / 2 > renderTarget2D.Height)
            {
                // ball hit bottom wall
                Velocity.Y = -Velocity.Y;
                var hitPosition = new Vector2(Position.X + BoundingBox.Width / 2, renderTarget2D.Height);
                SpawnParticlesAtPosition(hitPosition, 5);
                AudioManager.Instance.PlaySound((int)PongSoundEffects.BallHit);
            }

            if (Position.X - BoundingBox.Width / 2 < 0)
            {
                // ball hit left wall
                if (Velocity.X < 0)
                {
                    Velocity.X = -Velocity.X;
                    GameStats.Instance.PlayerLives -= 1;
                    var hitPosition = new Vector2(0, Position.Y + BoundingBox.Height / 2);
                    SpawnParticlesAtPosition(hitPosition, 5);
                    AudioManager.Instance.PlaySound((int)PongSoundEffects.LiveLost);
                    AudioManager.Instance.PlaySound((int)PongSoundEffects.BallHit);
                }
            }

            if (Position.X + BoundingBox.Width /2 > renderTarget2D.Width)
            {
                // ball hit right wall
                if (Velocity.X > 0)
                {
                    Velocity.X = -Velocity.X;
                    GameStats.Instance.PlayerScore += SCORE_ON_WALL_HIT;
                    var hitPosition = new Vector2(renderTarget2D.Width, Position.Y + BoundingBox.Height / 2);
                    SpawnParticlesAtPosition(hitPosition, 5);
                    AudioManager.Instance.PlaySound((int)PongSoundEffects.Scored);
                    AudioManager.Instance.PlaySound((int)PongSoundEffects.BallHit);
                }
            }

            if (IsColliding(PongTags.leftPaddle, entityManager, out Entity collidedEntity1))
            {
                if (Velocity.X < 0)
                {
                    if (collidedEntity1.Position.Y > Position.Y)
                    {
                        Velocity.Y -= 1;
                    } else {
                        Velocity.Y  += 1;
                    }
                    Velocity.X = -Velocity.X * 1.1f;
                    Velocity.Y *= 1.1f;
                    GameStats.Instance.PlayerScore += SCORE_ON_PADDLE_HIT;
                    var hitPosition = new Vector2(Position.X, Position.Y + BoundingBox.Height / 2);
                    SpawnParticlesAtPosition(hitPosition, 15);
                    AudioManager.Instance.PlaySound((int)PongSoundEffects.PaddleHit);
                }
            }

            if (IsColliding(PongTags.rightPaddle, entityManager, out Entity collidedEntity2))
            {
                if (Velocity.X > 0)
                {
                    Velocity.X = -Velocity.X * 1.1f;
                    Velocity.Y *= 1.1f;
                    var hitPosition = new Vector2(Position.X + BoundingBox.Width, Position.Y + BoundingBox.Height / 2);
                    SpawnParticlesAtPosition(hitPosition, 15);
                    AudioManager.Instance.PlaySound((int)PongSoundEffects.PaddleHit);

                    if (collidedEntity2.Position.Y > Position.Y)
                    {
                        if (Velocity.Y > 0)
                        {

                        }
                        Velocity.Y -= 2;
                    }
                    else
                    {
                        Velocity.Y += 2;
                    }
                }
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

        private void SpawnParticlesAtPosition(Vector2 position, int amount)
        {
            var newParticle = particle.Copy();
            newParticle.Position = position;
            ParticleSystem.Instance.Spawn(newParticle, amount, 16, 20);
        }

        private bool IsColliding(PongTags tag, EntityManager entityManager, out Entity collidedEntity)
        {
            return entityManager.IsColliding(this, (int)tag, out collidedEntity);
        }
    }
}
