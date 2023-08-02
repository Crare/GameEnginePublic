using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Pathfinding;
using GameEngine.Core.GameEngine.Sprites;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Pacman.Globals;

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
            : base(position, texture, Color.Red, pathfinding, entityManager, PacmanTags.RedGhost)
        {
            timeout = 1;
        }

        public override void Restart()
        {
            timeout = 1;
            base.Restart();
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            if (!Globals.GameStarted)
            {
                return;
            }
            if (timeout > 0)
            {
                base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
                return;
            }

            // wikipedia: Blinky(red) gives direct chase to Pac-Man
            if (path == null)
            {
                var a = new Point((int)Math.Round(Position.X / PACMAN_TILESIZE), (int)Math.Round(Position.Y / PACMAN_TILESIZE));
                var pacman = entityManager.GetEntityByTag<PacmanEntity>((int)PacmanTags.Pacman);
                var b = new Point((int)Math.Round(pacman.Position.X / PACMAN_TILESIZE), (int)Math.Round(pacman.Position.Y / PACMAN_TILESIZE));
                path = Pathfinding.GetPath(a, b);
            }

            if (path != null)
            {
                var targetNode = path.FirstOrDefault();
                if (targetNode  != null)
                {
                    MoveTowardsPosition(targetNode.Position, gameTime);
                    if (Vector2.Distance(Position, targetNode.Position) < 0.1f)
                    {
                        Position = targetNode.Position;
                        path.RemoveAt(0);

                    }
                }
            }

            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
        }

        public override void DebugDraw(SpriteBatch spriteBatch, Texture2D debugTexture, Color debugColor, Color debugColor2)
        {
            base.DebugDraw(spriteBatch, debugTexture, debugColor, debugColor2);

            if (path != null)
            {
                Pathfinding.DrawPath(path);
            }
        }
    }

    /// <summary>
    /// Inky(blue) try to position behind Pac-Man, usually by cornering him;
    /// </summary>
    public class BlueGhost : Ghost
    {
        public BlueGhost(Vector2 position, Texture2D texture, PacmanPathfinding pathfinding, EntityManager entityManager)
            : base(position, texture, Color.Blue, pathfinding, entityManager, PacmanTags.BlueGhost)
        {
            timeout = 2;
        }

        public override void Restart()
        {
            timeout = 2;
            base.Restart();
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            if (!Globals.GameStarted)
            {
                return;
            }
            if (timeout > 0)
            {
                base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
                return;
            }

            // wikipedia: Pinky(pink) and Inky(blue) try to position themselves in front of Pac-Man, usually by cornering him;
            if (path == null)
            {
                var a = new Point((int)Math.Round(Position.X / PACMAN_TILESIZE), (int)Math.Round(Position.Y / PACMAN_TILESIZE));
                var pacman = entityManager.GetEntityByTag<PacmanEntity>((int)PacmanTags.Pacman);
                var b = new Point((int)Math.Round(pacman.Position.X / PACMAN_TILESIZE), (int)Math.Round(pacman.Position.Y / PACMAN_TILESIZE));

                // try to get path to the position behind of pacman
                if (pacman.Direction == PacmanDirection.right)
                {
                    b.X = Pathfinding.PathNodes[b.X - 1, b.Y]  != null ? b.X - 1 :  b.X;
                } else if (pacman.Direction == PacmanDirection.left)
                {
                    b.X = Pathfinding.PathNodes[b.X + 1, b.Y] != null ? b.X + 1 : b.X;
                }
                if (pacman.Direction == PacmanDirection.up)
                {
                    b.Y = Pathfinding.PathNodes[b.X, b.Y + 1] != null ? b.Y + 1 : b.Y;
                }
                else if (pacman.Direction == PacmanDirection.down)
                {
                    b.Y = Pathfinding.PathNodes[b.X, b.Y - 1] != null ? b.Y - 1 : b.Y;
                }
                path = Pathfinding.GetPath(a, b);
            }

            if (path != null)
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
                }
            }

            // so one of them probably targets the tile pacman was before, and  other  one targets  the tile pacman is moving to(prediction/assumption).
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
        }
    }

    /// <summary>
    /// Pinky(pink) try to position in front of Pac-Man, usually by cornering him;
    /// </summary>
    public class PinkGhost : Ghost
    {
        public PinkGhost(Vector2 position, Texture2D texture, PacmanPathfinding pathfinding, EntityManager entityManager)
            : base(position, texture, Color.Pink, pathfinding, entityManager, PacmanTags.PinkGhost)
        {
            timeout = 3;
        }

        public override void Restart()
        {
            timeout = 3;
            base.Restart();
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            if (!Globals.GameStarted)
            {
                return;
            }
            if (timeout > 0)
            {
                base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
                return;
            }

            // wikipedia: Pinky(pink) and Inky(blue) try to position themselves in front of Pac-Man, usually by cornering him;
            if (path == null)
            {
                var a = new Point((int)Math.Round(Position.X / PACMAN_TILESIZE), (int)Math.Round(Position.Y / PACMAN_TILESIZE));
                var pacman = entityManager.GetEntityByTag<PacmanEntity>((int)PacmanTags.Pacman);
                var b = new Point((int)Math.Round(pacman.Position.X / PACMAN_TILESIZE), (int)Math.Round(pacman.Position.Y / PACMAN_TILESIZE));

                // try to get path to the position in front of pacman
                if (pacman.Direction == PacmanDirection.right)
                {
                    b.X = Pathfinding.PathNodes[b.X + 1, b.Y] != null ? b.X + 1 : b.X;
                }
                else if (pacman.Direction == PacmanDirection.left)
                {
                    b.X = Pathfinding.PathNodes[b.X - 1, b.Y] != null ? b.X - 1 : b.X;
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

            if (path != null)
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
                }
            }

            // so one of them probably targets the tile pacman was before, and  other  one targets  the tile pacman is moving to(prediction/assumption).
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
        }
    }

    /// <summary>
    /// Clyde(orange) will switch between chasing Pac-Man and fleeing from him.
    /// </summary>
    public class OrangeGhost : Ghost
    {
        private bool ChasePacman = false;

        public OrangeGhost(Vector2 position, Texture2D texture, PacmanPathfinding pathfinding, EntityManager entityManager)
            : base(position, texture, Color.Orange, pathfinding, entityManager, PacmanTags.OrangeGhost)
        {
            timeout = 4;
        }

        public override void Restart()
        {
            timeout = 4;
            base.Restart();
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            if (!Globals.GameStarted)
            {
                return;
            }
            if (timeout > 0)
            {
                base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
                return;
            }

            // wikipedia: Clyde(orange) will switch between chasing Pac-Man and fleeing from him.
            
            if  (vulnerable <= 0)
            {
                if (
                    (path == null || !path.Any())
                    && ChasePacman)
                {
                    var a = new Point((int)Math.Round(Position.X / PACMAN_TILESIZE), (int)Math.Round(Position.Y / PACMAN_TILESIZE));
                    var pacman = entityManager.GetEntityByTag<PacmanEntity>((int)PacmanTags.Pacman);
                    var b = new Point((int)Math.Round(pacman.Position.X / PACMAN_TILESIZE), (int)Math.Round(pacman.Position.Y / PACMAN_TILESIZE));
                    path = Pathfinding.GetPath(a, b);
                }
                else if (
                    (path == null || !path.Any())
                    && !ChasePacman)
                {
                    var a = new Point((int)Math.Round(Position.X / PACMAN_TILESIZE), (int)Math.Round(Position.Y / PACMAN_TILESIZE));
                    var pacman = entityManager.GetEntityByTag<PacmanEntity>((int)PacmanTags.Pacman);
                    var b = new Point((int)Math.Round(pacman.Position.X / PACMAN_TILESIZE), (int)Math.Round(pacman.Position.Y / PACMAN_TILESIZE));
                    var furthestPoint = Pathfinding.GetFurthestNodePositionFromPoint(b);
                    path = Pathfinding.GetPath(a, furthestPoint);
                }
            }  else
            {
                // go back to gated area.
                if (path == null || !path.Any())
                {
                    var a = new Point((int)Math.Round(Position.X / PACMAN_TILESIZE), (int)Math.Round(Position.Y / PACMAN_TILESIZE));
                    path = Pathfinding.GetPath(a, StartingPoint);
                }
            }

            if (path != null)
            {
                var targetNode = path.FirstOrDefault();
                if (targetNode != null)
                {
                    MoveTowardsPosition(targetNode.Position, gameTime);
                    if (Vector2.Distance(Position, targetNode.Position) < 0.1f)
                    {
                        Position = targetNode.Position;
                        path.RemoveAt(0);
                        if (!path.Any())
                        {
                            ChasePacman = !ChasePacman;
                        }
                    }
                }
            }

            //  so probably: targets furthest away tile from pacman and when  reaches it,
            //  targets next  the tile the pacman is  on  and  moves there
            //  and then switches back tto furthests tile.  
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
        }
    }

    public class Ghost : Entity
    {
        private SpriteAnimation Animation;
        private Color ColorTint;
        private Color ColorVulnerable;
        internal PacmanPathfinding Pathfinding;
        internal List<PathNode> path;
        internal EntityManager EntityManager;
        internal float timeout = 8;
        internal float vulnerable = 0;
        internal Point StartingPoint;

        public Ghost(Vector2 position, Texture2D texture, Color colorTint, PacmanPathfinding pathfinding, EntityManager entityManager, PacmanTags tag)
            : base(position, new Rectangle((int)position.X, (int)position.Y, 10, 10), (float)Globals.SpriteLayers.MIDDLEGROUND, Globals.GHOST_SPEED, null, (int)tag)
        {
            ColorTint = colorTint;
            ColorVulnerable = new Color(0.5f, 0, 0.9f);
            var anim = new Rectangle[4];
            anim[0] = new Rectangle(0, 0, 16, 16);
            anim[1] = new Rectangle(16, 0, 16, 16);
            anim[2] = new Rectangle(32, 0, 16, 16);
            anim[3] = new Rectangle(48, 0, 16, 16);
            Sprite = new Sprite(texture, new Rectangle(64, 0, 16, 16));
            Animation = new SpriteAnimation(texture, 0, 10, true, anim);
            Pathfinding = pathfinding;
            EntityManager = entityManager;
        }

        public virtual void Restart()
        {
            StartingPoint = new Point((int)Math.Round(Position.X / PACMAN_TILESIZE), (int)Math.Round(Position.Y / PACMAN_TILESIZE));
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

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            if (!Globals.GameStarted)
            {
                return;
            }
            BoundingBox = new Rectangle((int)Position.X - BoundingBox.Width / 2, (int)Position.Y - BoundingBox.Height / 2, BoundingBox.Width, BoundingBox.Height);
            Animation.Update(gameTime);
            if (timeout > 0)
            {
                timeout -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if  (vulnerable >0)
            {
                vulnerable -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
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
    }
}
