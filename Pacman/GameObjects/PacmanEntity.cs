using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Audio;
using GameEngine.Core.GameEngine.Pathfinding;
using GameEngine.Core.GameEngine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pacman.Pacman;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pacman.GameObjects
{
    public enum PacmanDirection
    {
        right = 0,
        down = 1,
        left = 2,
        up = 3
    }

    public class PacmanEntity : Entity
    {
        SpriteAnimation EatAnimation;
        SpriteAnimation DeathAnimation;
        private int AnimationState = 0; // 0 = eat, 1 = death
        public PacmanDirection Direction = PacmanDirection.right;
        private float rotation = 0f; // in radians
        private bool stopped = true;
        private PacmanPathfinding Pathfinding;
        private List<PathNode> path;
        private bool gameStarted = false;
        private float invulnerable = 0f;

        public PacmanEntity(Vector2 position, Texture2D texture, PacmanPathfinding pathfinding) 
            : base(position, new Rectangle((int)position.X, (int)position.Y, 10, 10), (float)Globals.SpriteLayers.MIDDLEGROUND, Globals.PACMAN_SPEED, null, (int)Globals.PacmanTags.Pacman)
        {
            Pathfinding = pathfinding;
            var pacmanEat = new Rectangle[3];
            pacmanEat[0] = new Rectangle(0, 0, 16, 16);
            pacmanEat[1] = new Rectangle(16, 0, 16, 16);
            pacmanEat[2] = new Rectangle(32, 0, 16, 16);
            EatAnimation = new SpriteAnimation(texture, 0, 10, true, pacmanEat);

            var pacmanDeath = new Rectangle[9];
            pacmanDeath[0] = new Rectangle(48, 0, 16, 16);
            pacmanDeath[1] = new Rectangle(64, 0, 16, 16);
            pacmanDeath[2] = new Rectangle(80, 0, 16, 16);
            pacmanDeath[3] = new Rectangle(95, 0, 16, 16);
            pacmanDeath[4] = new Rectangle(112, 0, 16, 16);
            pacmanDeath[5] = new Rectangle(128, 0, 16, 16);
            pacmanDeath[6] = new Rectangle(144, 0, 16, 16);
            pacmanDeath[7] = new Rectangle(160, 0, 16, 16);
            pacmanDeath[8] = new Rectangle(176, 0, 16, 16);
            DeathAnimation = new SpriteAnimation(texture, 0, 10, false, pacmanDeath);

            PacmanEventSystem.OnBigDotPicked += OnBigDotPicked;
        }

        private void OnBigDotPicked()
        {
            invulnerable = Globals.VULNERABLE_SECONDS;
            Speed = Globals.PACMAN_SPEED_WHEN_INVULNERABLE;
        }

        public void Restart()
        {
            AnimationState = 0;
            Direction = PacmanDirection.right;
            rotation = 0f;
            stopped = true;
            gameStarted = false;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (AnimationState == 0)
            {
                EatAnimation.Draw(spriteBatch, Position, HorizontalFlipped, DepthLayer, Color.White, rotation, Vector2.One);
            } else
            {
                DeathAnimation.Draw(spriteBatch, Position, HorizontalFlipped, DepthLayer, Color.White, rotation, Vector2.One);
            }
        }

        public override void DebugDraw(SpriteBatch spriteBatch, Texture2D debugTexture, Color debugColor, Color debugColor2)
        {
            if (path != null && path.Any())
            {
                Pathfinding.DrawPath(path);
            }
            base.DebugDraw(spriteBatch, debugTexture, debugColor, debugColor2);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            var lastGameStarted = gameStarted;
            var lastPosition = Position;

            if (AnimationState == 0 && !stopped)
            {
                var looped = EatAnimation.Update(gameTime);
                if (looped)
                {
                    AudioManager.Instance.PlaySound((int)Globals.PacmanSoundEffects.pacmanMove);
                }
            }
            else if(AnimationState == 1)
            {
                var animationEnded = DeathAnimation.Update(gameTime);
                if (animationEnded)
                {
                    PacmanEventSystem.GameOver();
                }
                return; // skip rest of the update code  after death.
            }

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                Direction = PacmanDirection.up;
                stopped = false;
                gameStarted = true;
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                Direction = PacmanDirection.down;
                stopped = false;
                gameStarted = true;
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                Direction = PacmanDirection.left;
                stopped = false;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                Direction = PacmanDirection.right;
                stopped = false;
                gameStarted = true;
            }

            if (!stopped && (path == null || !path.Any()))
            {
                var pos = new Point(
                        (int)Math.Round(Position.X / Globals.PACMAN_TILESIZE),
                        (int)Math.Round(Position.Y / Globals.PACMAN_TILESIZE));
                Point target = new Point(-1, -1);

                if (Direction ==  PacmanDirection.right)
                {
                    target = new Point(pos.X + 1, pos.Y);
                } else if (Direction == PacmanDirection.left)
                {
                    target = new Point(pos.X - 1, pos.Y);
                }
                else if (Direction == PacmanDirection.up)
                {
                    target = new Point(pos.X, pos.Y  - 1);
                }
                else if (Direction == PacmanDirection.down)
                {
                    target = new Point(pos.X, pos.Y + 1);
                }

                if (target.X != -1 && target.Y != -1)
                {
                    path = Pathfinding.GetPath(pos, target);
                    if (path != null && path.Any())
                    {
                        stopped = false;
                    }  else
                    {
                        stopped = true;
                    }
                } else
                {
                    stopped = true;
                }
            }

            if (path != null && path.Any() && !stopped)
            {
                var targetNode = path.FirstOrDefault();
                if (targetNode != null)
                {
                    MoveTowardsPosition(targetNode.Position, gameTime);
                    if (Vector2.Distance(Position, targetNode.Position) < 0.1f)
                    {
                        Position = targetNode.Position;
                        path.RemoveAt(0);
                    }
                    BoundingBox = new Rectangle(
                        (int)Position.X - BoundingBox.Width / 2,
                        (int)Position.Y - BoundingBox.Height / 2,
                        BoundingBox.Width,
                        BoundingBox.Height);
                }
            }

            if (gameStarted && !lastGameStarted)
            {
                Globals.GHOSTS_MOVING = true;
                PacmanEventSystem.GameStarted();
            }

            if (invulnerable >= 0)
            {
                invulnerable -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (invulnerable <= 0)
                {
                    Speed = Globals.PACMAN_SPEED;
                }
            }

            if (entityManager.IsColliding(
                this,
                new int[] {
                    (int)Globals.PacmanTags.BlueGhost,
                    (int)Globals.PacmanTags.OrangeGhost,
                    (int)Globals.PacmanTags.PinkGhost,
                    (int)Globals.PacmanTags.RedGhost
                },
                out var collidedEntity))
            {
                if (invulnerable <=  0)
                {
                    // play death animation, after that we go to GameOver gameState.
                    AnimationState = 1;
                    Globals.GHOSTS_MOVING = false;
                    AudioManager.Instance.PlaySound((int)Globals.PacmanSoundEffects.pacmanDeath);
                } else
                {
                    var ghost = collidedEntity as Ghost;
                    ghost.OnDeath();
                    GameStats.Instance.PlayerScore += Globals.SCORE_ON_GHOST_EATEN;
                    PacmanEventSystem.GhostEaten();
                    AudioManager.Instance.PlaySound((int)Globals.PacmanSoundEffects.ghostDeath);
                }
            }

            UpdateAnimation(lastPosition);
        }

        private void UpdateAnimation(Vector2 lastPosition)
        {
            if (stopped)
            {
                return;
            }

            var dir = new Vector2(Position.X - lastPosition.X, Position.Y - lastPosition.Y);

            // up
            if (dir.Y < 0)
            {
                rotation = MathHelper.ToRadians(270);
                HorizontalFlipped = false;
            }
            // down
            if (dir.Y > 0)
            {
                rotation = MathHelper.ToRadians(90);
                HorizontalFlipped = false;
            }
            // left
            if (dir.X < 0)
            {
                rotation = 0;
                HorizontalFlipped = true;
            }
            // right
            if (dir.X > 0)
            {
                rotation = 0;
                HorizontalFlipped = false;
            }
        }

        private void MoveTowardsPosition(Vector2 pos, GameTime gameTime)
        {
            var velocity = new Vector2(0, 0);
            if (pos.X == Position.X)
            {
                // vertical
                if (Position.Y < pos.Y)
                {
                    velocity.Y += 1;
                }
                else
                {
                    velocity.Y -= 1;
                }
            }
            else
            {
                // horizontal
                if (Position.X < pos.X)
                {
                    velocity.X += 1;
                }
                else
                {
                    velocity.X -= 1;
                }
            }
            Position += velocity * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
