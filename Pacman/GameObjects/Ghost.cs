using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Collision;
using GameEngine.Core.GameEngine.Particles;
using GameEngine.Core.GameEngine.Pathfinding;
using GameEngine.Core.GameEngine.Sprites;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman.GameObjects
{
    // wikipedia:
    // Blinky(red) gives direct chase to Pac-Man;
    // Pinky(pink) and Inky(blue) try to position themselves in front of Pac-Man, usually by cornering him;
    // and Clyde(orange) will switch between chasing Pac-Man and fleeing from him.[9]

    /// <summary>
    /// Blinky(red) gives direct chase to Pac-Man;
    /// </summary>
    public class RedGhost : Ghost
    {
        public RedGhost(Vector2 position, Texture2D texture, PacmanPathfinding pathfinding, EntityManager entityManager) 
            : base(position, texture, Color.Red, pathfinding, entityManager, Globals.PacmanTags.RedGhost)
        {
            timeout = 1;
            DebugPathOffset = 4;
        }

        public override void Restart()
        {
            timeout = 1;
            base.Restart();
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            if (!Globals.GHOSTS_MOVING)
            {
                return;
            }
            if (timeout > 0)
            {
                base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
                return;
            }

            UpdatePath(entityManager);
            MoveUsingPath(gameTime);

            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
        }

        internal override void UpdatePath(EntityManager entityManager)
        {
            if (path == null || !path.Any() || CanUpdatePath)
            {
                if (vulnerable > 0)
                {
                    // go back to gated area.
                    path = Pathfinding.GetPath(Position, StartingPoint);
                }
                else
                {
                    // wikipedia: Blinky(red) gives direct chase to Pac-Man
                    var pacman = entityManager.GetEntityByTag<PacmanEntity>((int)Globals.PacmanTags.Pacman);
                    path = Pathfinding.GetPath(Position, pacman.Position);
                }
                base.UpdatePath(entityManager);
            }
        }
    }

    /// <summary>
    /// Inky(blue) try to position behind Pac-Man, usually by cornering him;
    /// </summary>
    public class BlueGhost : Ghost
    {
        public BlueGhost(Vector2 position, Texture2D texture, PacmanPathfinding pathfinding, EntityManager entityManager)
            : base(position, texture, Color.Blue, pathfinding, entityManager, Globals.PacmanTags.BlueGhost)
        {
            timeout = 2;
            DebugPathOffset = 3;
        }

        public override void Restart()
        {
            timeout = 2;
            base.Restart();
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            if (!Globals.GHOSTS_MOVING)
            {
                return;
            }
            if (timeout > 0)
            {
                base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
                return;
            }

            UpdatePath(entityManager);
            MoveUsingPath(gameTime);

            // so one of them probably targets the tile pacman was before, and  other  one targets  the tile pacman is moving to(prediction/assumption).
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
        }


        internal override void UpdatePath(EntityManager entityManager)
        {
            if (path == null || !path.Any() || CanUpdatePath)
            {
                if (vulnerable > 0)
                {
                    // go back to gated area.
                    path = Pathfinding.GetPath(Position, StartingPoint);
                }
                else
                {
                    var a = GetCurrentPositionInTileMap();
                    // wikipedia: Pinky(pink) and Inky(blue) try to position themselves in front of Pac-Man, usually by cornering him;
                    var pacman = entityManager.GetEntityByTag<PacmanEntity>((int)Globals.PacmanTags.Pacman);
                    var b = new Point((int)Math.Round(pacman.Position.X / Globals.PACMAN_TILESIZE), (int)Math.Round(pacman.Position.Y / Globals.PACMAN_TILESIZE));

                    // try to get path to the position behind of pacman
                    if (pacman.Direction == PacmanDirection.right)
                    {
                        b.X = b.X - 1 >= 0 && Pathfinding.PathNodes[b.X - 1, b.Y] != null ? b.X - 1 : b.X;
                    }
                    else if (pacman.Direction == PacmanDirection.left)
                    {
                        b.X = b.X + 1 <= Pathfinding.PathNodes.GetUpperBound(0) && Pathfinding.PathNodes[b.X + 1, b.Y] != null ? b.X + 1 : b.X;
                    }
                    if (pacman.Direction == PacmanDirection.up)
                    {
                        b.Y = b.Y + 1 <= Pathfinding.PathNodes.GetUpperBound(1) && Pathfinding.PathNodes[b.X, b.Y + 1] != null ? b.Y + 1 : b.Y;
                    }
                    else if (pacman.Direction == PacmanDirection.down)
                    {
                        b.Y = b.Y - 1 >= 0 && Pathfinding.PathNodes[b.X, b.Y - 1] != null ? b.Y - 1 : b.Y;
                    }
                    path = Pathfinding.GetPath(a, b);
                }
                base.UpdatePath(entityManager);
            } 
        }
    }

    /// <summary>
    /// Pinky(pink) try to position in front of Pac-Man, usually by cornering him;
    /// </summary>
    public class PinkGhost : Ghost
    {
        public PinkGhost(Vector2 position, Texture2D texture, PacmanPathfinding pathfinding, EntityManager entityManager)
            : base(position, texture, Color.Pink, pathfinding, entityManager, Globals.PacmanTags.PinkGhost)
        {
            timeout = 3;
            DebugPathOffset = 2;
        }

        public override void Restart()
        {
            timeout = 3;
            base.Restart();
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            if (!Globals.GHOSTS_MOVING)
            {
                return;
            }
            if (timeout > 0)
            {
                base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
                return;
            }

            UpdatePath(entityManager);
            MoveUsingPath(gameTime);

            // so one of them probably targets the tile pacman was before, and  other  one targets  the tile pacman is moving to(prediction/assumption).
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
        }


        internal override void UpdatePath(EntityManager entityManager)
        {
            if (path == null || !path.Any() || CanUpdatePath)
            {
                if (vulnerable > 0)
                {
                    // go back to gated area.
                    path = Pathfinding.GetPath(Position, StartingPoint);
                }
                else
                {
                    var a = GetCurrentPositionInTileMap();
                    // wikipedia: Pinky(pink) and Inky(blue) try to position themselves in front of Pac-Man, usually by cornering him;
                    var pacman = entityManager.GetEntityByTag<PacmanEntity>((int)Globals.PacmanTags.Pacman);
                    var b = new Point((int)Math.Round(pacman.Position.X / Globals.PACMAN_TILESIZE), (int)Math.Round(pacman.Position.Y / Globals.PACMAN_TILESIZE));

                    // try to get path to the position in front of pacman
                    if (pacman.Direction == PacmanDirection.right)
                    {
                        if (b.X + 1 <= Pathfinding.PathNodes.GetUpperBound(0)) {
                            b.X = Pathfinding.PathNodes[b.X + 1, b.Y] != null ? b.X + 1 : b.X;
                        }
                    }
                    else if (pacman.Direction == PacmanDirection.left)
                    {
                        if (b.X - 1 >= 0)
                        {
                            b.X = Pathfinding.PathNodes[b.X - 1, b.Y] != null ? b.X - 1 : b.X;
                        }
                    }
                    if (pacman.Direction == PacmanDirection.up)
                    {
                        b.Y = Pathfinding.PathNodes[b.X, b.Y - 1] != null ? b.Y - 1 : b.Y;
                    }
                    else if (pacman.Direction == PacmanDirection.down)
                    {
                        b.Y = Pathfinding.PathNodes[b.X, b.Y + 1] != null ? b.Y + 1 : b.Y;
                    }
                    path = Pathfinding.GetPath(a, b);
                }
                base.UpdatePath(entityManager);
            }
        }
    }

    /// <summary>
    /// Clyde(orange) will switch between chasing Pac-Man and fleeing from him.
    /// </summary>
    public class OrangeGhost : Ghost
    {
        private bool ChasePacman = false;

        public OrangeGhost(Vector2 position, Texture2D texture, PacmanPathfinding pathfinding, EntityManager entityManager)
            : base(position, texture, Color.Orange, pathfinding, entityManager, Globals.PacmanTags.OrangeGhost)
        {
            timeout = 4;
            DebugPathOffset = 1;
        }

        public override void Restart()
        {
            timeout = 4;
            base.Restart();
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            if (!Globals.GHOSTS_MOVING)
            {
                return;
            }
            if (timeout > 0)
            {
                base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
                return;
            }

            UpdatePath(entityManager);
            MoveUsingPath(gameTime);

            if (path == null || !path.Any())
            {
                // reached  end of path
                ChasePacman = !ChasePacman;
                CanUpdatePath = true;
            }

            //  so probably: targets furthest away tile from pacman and when  reaches it,
            //  targets next  the tile the pacman is  on  and  moves there
            //  and then switches back tto furthests tile.  
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
        }


        internal override void UpdatePath(EntityManager entityManager)
        {
            if (path == null || !path.Any() || CanUpdatePath)
            {
                if (vulnerable > 0)
                {
                    // go back to gated area.
                    path = Pathfinding.GetPath(Position, StartingPoint);
                }
                else
                {
                    var pacman = entityManager.GetEntityByTag<PacmanEntity>((int)Globals.PacmanTags.Pacman);
                    // wikipedia: Clyde(orange) will switch between chasing Pac-Man and fleeing from him.
                    if (ChasePacman)
                    {
                        path = Pathfinding.GetPath(Position, pacman.Position);
                    }
                    else if (!ChasePacman)
                    {
                        var furthestPoint = Pathfinding.GetFurthestNodePositionFromPosition(pacman.Position);
                        path = Pathfinding.GetPath(Position, furthestPoint);
                    }
                }
                base.UpdatePath(entityManager);
            }
        }
    }

    public class Ghost : Entity, IHasSpriteAnimation, IHasSprite, ICollidable
    {
        public BoxCollider Collider { get; set; }
        public SpriteAnimation Animation { get; set; }
        public bool HorizontalFlipped { get; set; }
        public float DepthLayer { get; set; }
        public float Speed { get; set; }
        public Sprite Sprite { get; set; }

        private Color ColorTint;
        private Color ColorVulnerable;
        internal PacmanPathfinding Pathfinding;
        internal List<PathNode> path;
        internal EntityManager EntityManager;
        internal float timeout = 8;
        internal float vulnerable = 0;
        internal Point StartingPoint;
        internal bool CanUpdatePath = true;
        internal int DebugPathOffset = 0;
        internal Particle particle;

        public Ghost(Vector2 position, Texture2D texture, Color colorTint, PacmanPathfinding pathfinding, EntityManager entityManager, Globals.PacmanTags tag)
            : base(position, (int)tag)
        {
            Collider = new BoxCollider(new Rectangle((int)position.X, (int)position.Y, 10, 10));
            DepthLayer = (float)Globals.SPRITE_LAYER_MIDDLEGROUND;
            Speed = Globals.GHOST_SPEED;

            ColorTint = colorTint;
            ColorVulnerable = new Color(0.5f, 0, 0.9f);

            var anim = new Rectangle[4];
            anim[0] = new Rectangle(0, 0, 16, 16);
            anim[1] = new Rectangle(16, 0, 16, 16);
            anim[2] = new Rectangle(32, 0, 16, 16);
            anim[3] = new Rectangle(48, 0, 16, 16);
            Animation = new SpriteAnimation(texture, 0, 10, true, anim);

            Sprite = new Sprite(texture, new Rectangle(64, 0, 16, 16));

            Pathfinding = pathfinding;
            EntityManager = entityManager;
            particle = new Particle(1, texture, new Vector2(4, 4), Position, ColorTint, DepthLayer, anim[0]);

            PacmanEventSystem.OnBigDotPicked += OnBigDotPicked;
            PacmanEventSystem.OnNewGame += OnNewGame;
            PacmanEventSystem.OnGameOver += OnGameOver;
        }

        private void OnGameOver()
        {
            path = null;
        }

        private void OnNewGame()
        {
            Position = new Vector2(StartingPoint.X * Globals.PACMAN_TILESIZE, StartingPoint.Y * Globals.PACMAN_TILESIZE);
            Collider.UpdatePosition(Position);
            Restart();
            path = null;
        }

        private void OnBigDotPicked()
        {
            vulnerable = Globals.VULNERABLE_SECONDS;
        }

        public virtual void OnDeath()
        {
            SpawnParticlesAtPosition(Position, 10);
            Position = new Vector2(StartingPoint.X * Globals.PACMAN_TILESIZE, StartingPoint.Y * Globals.PACMAN_TILESIZE);
            timeout = 10;
            path = null;
            Restart();
        }

        public virtual void Restart()
        {
            StartingPoint = new Point((int)Math.Round(Position.X / Globals.PACMAN_TILESIZE), (int)Math.Round(Position.Y / Globals.PACMAN_TILESIZE));
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (vulnerable <= 0)
            {
                Animation.Draw(spriteBatch, Position, HorizontalFlipped, DepthLayer, ColorTint, 0f, Vector2.One);
            } else
            {
                Sprite.Draw(spriteBatch, Position, ColorVulnerable, DepthLayer, HorizontalFlipped);
            }
        }

        internal virtual void UpdatePath(EntityManager entityManager)
        {
            CanUpdatePath = false;
        }

        internal virtual void MoveUsingPath(GameTime gameTime)
        {
            if (path != null && path.Any())
            {
                var targetNode = path.FirstOrDefault();
                if (targetNode != null)
                {
                    MoveTowardsPosition(targetNode.Position, gameTime);
                    if (Vector2.Distance(Position, targetNode.Position) < 0.1f)
                    {
                        Position = targetNode.Position;
                        path.RemoveAt(0);
                        CanUpdatePath = true; // path position reached. allow updating path.
                    } else
                    {
                        CanUpdatePath = false; // don't allow path updates when not aligned to tilemap grid
                    }
                }
                //else
                //{
                //    CanUpdatePath = true;
                //}
            }
            //else
            //{
            //    CanUpdatePath = true;
            //}
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            if (!Globals.GHOSTS_MOVING)
            {
                return;
            }
            Collider.UpdatePosition(Position);
            Animation.Update(gameTime);
            if (timeout > 0)
            {
                timeout -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if  (vulnerable > 0)
            {
                vulnerable -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        internal Point GetCurrentPositionInTileMap()
        {
            return new Point((int)Math.Round(Position.X / Globals.PACMAN_TILESIZE), (int)Math.Round(Position.Y / Globals.PACMAN_TILESIZE));
        }

        internal void MoveTowardsPosition(Vector2 pos, GameTime gameTime)
        {
            var velocity = new Vector2(0, 0);
            if (pos.X == Position.X)
            {
                // vertical
                if (Position.Y < pos.Y)
                {
                    velocity.Y += 1;
                } else
                {
                    velocity.Y -= 1;
                }
            } else
            {
                // horizontal
                if (Position.X < pos.X)
                {
                    velocity.X += 1;
                    HorizontalFlipped = false;
                }
                else
                {
                    velocity.X -= 1;
                    HorizontalFlipped = true;
                }
            }
            Position += velocity * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void DebugDraw(SpriteBatch spriteBatch, Color debugColor)
        {
            Collider.DebugDraw(spriteBatch, debugColor);

            if (path != null)
            {
                Pathfinding.DrawPath(path, ColorTint, DebugPathOffset);
            }
        }

        internal void SpawnParticlesAtPosition(Vector2 position, int amount)
        {
            var newParticle = particle.Copy();
            newParticle.Position = position;
            ParticleSystem.Instance.Spawn(newParticle, amount, 16, 20);
        }
    }
}
