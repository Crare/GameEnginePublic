using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GameEngine.Core.GameEngine.Particles
{
    public sealed class ParticleSystem
    {
        private static ParticleSystem instance = null; // singleton
        private static readonly object padlock = new();
        private int lastParticleIndex = 0;

        public static ParticleSystem Instance
        {
            get
            {
                // locks the Singleton instance to be once created by locking one of its objects.
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ParticleSystem();
                    }
                    return instance;
                }
            }
        }

        public Particle[] Particles;
        public Vector2 Gravity = new(0, 1);
        public static int MAX_PARTICLES = 30000;
        public List<int> ActiveParticles = new();
        public bool ParticleListModified = false;

        public SpriteBatch SpriteBatch { get; private set; }
        public GraphicsDeviceManager Graphics { get; private set; }

        ParticleSystem()
        {
            Particles = new Particle[MAX_PARTICLES];
            ParticleEvents.OnParticleDestroy += OnParticleDestroy;
            ParticleEvents.OnParticlesReset += OnParticlesReset;
        }

        public void Init(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            SpriteBatch = spriteBatch;
            Graphics = graphics;
        }

        public void OnParticlesReset()
        {
            Particles = new Particle[MAX_PARTICLES];
            ActiveParticles = new();
        }

        public void OnParticleDestroy(int index)
        {
            Particles[index] = null;
            var item = ActiveParticles.SingleOrDefault(p => p == index);
            ActiveParticles.Remove(item);
        }

        /// <summary>
        /// Spawns multiple particles.
        /// </summary>
        /// <param name="particle">Type of particle</param>
        /// <param name="amount">how many will be spawned</param>
        /// <param name="spread">how far from the original particle position particles will be spawned</param>
        public void Spawn(Particle particle, int amount, int spread, float momentum)
        {
            if (ActiveParticles.Count == MAX_PARTICLES)
            {
                return;
            }

            Random rnd = new();
            for (int i = 0; i < amount; i++)
            {
                var randX = rnd.Next((int)particle.Position.X - spread, (int)particle.Position.X + spread);
                var randY = rnd.Next((int)particle.Position.Y - spread, (int)particle.Position.Y + spread);
                var velX = randX < particle.Position.X ? -momentum : momentum;
                var velY = randY < particle.Position.Y ? -momentum : momentum;

                var newParticle = particle.Copy();
                newParticle.Index = GetNewParticleIndex();
                newParticle.Position = new Vector2(randX, randY);
                newParticle.Velocity = new Vector2(velX, velY);

                Particles[newParticle.Index] = newParticle;
                ActiveParticles.Add(newParticle.Index);
            }
        }

        public void Spawn(Particle particle)
        {
            if (ActiveParticles.Count == MAX_PARTICLES)
            {
                return;
            }

            particle.Index = GetNewParticleIndex();
            Particles[particle.Index] = particle;
            ActiveParticles.Add(particle.Index);
        }

        public void Update(GameTime gameTime)
        {
            for(int i = 0; i < ActiveParticles.Count; i++)
            {
                var particleIndex = ActiveParticles[i];
                Particles[particleIndex].Update(gameTime, Gravity);
            }
        }

        public void Draw() {
            for (int i = 0; i < ActiveParticles.Count; i++)
            {
                var particleIndex = ActiveParticles[i];
                Particles[particleIndex].Draw(SpriteBatch, Graphics);
            }
        }

        private int GetNewParticleIndex()
        {
            lastParticleIndex += 1;
            if (lastParticleIndex >= MAX_PARTICLES)
            {
                lastParticleIndex = 0;
                while (ActiveParticles.Contains(lastParticleIndex))
                {
                    lastParticleIndex++;
                }
            }
            return lastParticleIndex;
        }
    }
}
