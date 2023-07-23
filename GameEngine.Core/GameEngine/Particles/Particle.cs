using GameEngine.Core.GameEngine.Physics;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.GameEngine.Particles
{
    public class Particle
    {
        /// <summary>
        /// Index is set when the particle is added to the ParticleSystem.
        /// </summary>
        public int Index; 
        public Vector2 Position;
        public Vector2 Velocity;
        public Color Color;

        /// <summary>
        /// lifetime in seconds
        /// </summary>
        private double LifeTime;
        private Texture2D Texture;
        private Vector2 Size;

        private Rectangle SourceRect;
        private float DepthLayer;

        public Particle(double lifeTime, Texture2D texture, Vector2 size, Vector2 position, Color color, float depthLayer)
        {
            Position = position;
            LifeTime = lifeTime;
            Size = size;
            Texture = texture;
            Color = color;
            DepthLayer = depthLayer;

            // use piece from the center of the Texture using Size.
            SourceRect = new Rectangle(
                (int)(Texture.Width / 2 - Size.X / 2),
                (int)(Texture.Height / 2 - Size.Y / 2),
                (int)Size.X,
                (int)Size.Y
            );
        }

        public Particle Copy()
        {
            return new Particle(LifeTime, Texture, Size, Position, Color, DepthLayer);
        }

        public virtual void Update(GameTime gameTime, Vector2 gravity)
        {
            LifeTime -= gameTime.ElapsedGameTime.TotalSeconds;
            if (LifeTime < 0 )
            {
                ParticleEvents.PatricleDestroy(Index);
                return;
            }

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Velocity.X < 0)
            {
                Velocity.X += gravity.X;
                if (Velocity.X > 0)
                {
                    Velocity.Y = 0;
                }
            }
            else if (Velocity.X > 0)
            {
                Velocity.X -= gravity.X;
                if (Velocity.X < 0)
                {
                    Velocity.Y = 0;
                }
            }

            if (Velocity.Y < PhysicsGlobals.MAX_GRAVITY_Y)
            {
                Velocity.Y += gravity.Y;
                if (Velocity.Y > PhysicsGlobals.MAX_GRAVITY_Y)
                {
                    Velocity.Y = PhysicsGlobals.MAX_GRAVITY_Y;
                }
            }

        }

        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(
                Texture,
                Position,
                SourceRect,
                Color.White,
                0f, // rotation
                new Vector2(Texture.Width / 2, Texture.Height / 2), // origin
                Vector2.One, // scale
                SpriteEffects.None,
                DepthLayer
            );
        }
    }
}
