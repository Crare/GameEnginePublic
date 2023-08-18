using GameEngine.Core.EntityManagement;
using GameEngine.Core.GameEngine.Audio;
using GameEngine.Core.GameEngine.Collision;
using GameEngine.Core.GameEngine.Particles;
using GameEngine.Core.GameEngine.Sprites;
using GameEngine.Core.SpriteManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Pacman.Globals;

namespace Pacman.GameObjects
{
    public class SmallDot : Dot
    {
        public SmallDot(Vector2 position, Texture2D texture)
            : base(position, new Rectangle((int)position.X, (int)position.Y, 4, 4), Globals.PacmanTags.DotSmall)
        {
            var anim = new Rectangle[4];
            anim[0] = new Rectangle(0,0,16,16);
            anim[1] = new Rectangle(16, 0, 16, 16);
            anim[2] = new Rectangle(32, 0, 16, 16);
            anim[3] = new Rectangle(48, 0, 16, 16);
            Animation = new SpriteAnimation(texture, 0, 10, true, anim);
            particle = new Particle(1, texture, new Vector2(4, 4), Position, Color.White, DepthLayer, anim[0]);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
            if (entityManager.IsColliding(this, (int)PacmanTags.Pacman, out var collidedEntity))
            {
                SpawnParticlesAtPosition(Position, 2);
                PacmanEventSystem.SmallDotPicked();
                entityManager.RemoveEntity(Id);
                //AudioManager.Instance.PlaySound((int)Globals.PacmanSoundEffects.pickupSmallDot);
            }
        }

        public SmallDot Copy()
        {
            return new SmallDot(Position, Animation.Texture);
        }
    }

    public class BigDot : Dot
    {
        public BigDot(Vector2 position, Texture2D texture)
            : base(position, new Rectangle((int)position.X, (int)position.Y, 8, 8), Globals.PacmanTags.DotBig)
        {
            var anim = new Rectangle[3];
            anim[0] = new Rectangle(64, 0, 16, 16);
            anim[1] = new Rectangle(80, 0, 16, 16);
            anim[2] = new Rectangle(96, 0, 16, 16);
            Animation = new SpriteAnimation(texture, 0, 5, true, anim);
            particle = new Particle(1, texture, new Vector2(4, 4), Position, Color.White, DepthLayer, anim[0]);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            base.Update(gameTime, keyboardState, renderTarget2D, entityManager);
            if (entityManager.IsColliding(this, (int)PacmanTags.Pacman, out var collidedEntity))
            {
                PacmanEventSystem.BigDotPicked();
                SpawnParticlesAtPosition(Position, 5);
                entityManager.RemoveEntity(Id);
                AudioManager.Instance.PlaySound((int)Globals.PacmanSoundEffects.pickupBigDot);
            }
        }

        public BigDot Copy()
        {
            return new BigDot(Position, Animation.Texture);
        }
    }

    public class Dot : Entity, ICollidable, IHasSpriteAnimation
    {
        internal Particle particle;
        public BoxCollider Collider { get; set; }

        public SpriteAnimation Animation { get; set; }

        public bool HorizontalFlipped { get; set; }
        public float DepthLayer { get; set; }

        public Dot(Vector2 position, Rectangle boundingBox, Globals.PacmanTags tag)
            : base(position, (int)tag)
        {
            Collider = new BoxCollider(boundingBox);
            DepthLayer = (int)Globals.SPRITE_LAYER_BACKGROUND;
        }


        public override void DebugDraw(SpriteBatch spriteBatch, Color debugColor)
        {
            Collider.DebugDraw(spriteBatch, debugColor);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Animation.Draw(spriteBatch, Position, false, DepthLayer);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, RenderTarget2D renderTarget2D, EntityManager entityManager)
        {
            Animation.Update(gameTime);
        }

        internal void SpawnParticlesAtPosition(Vector2 position, int amount)
        {
            var newParticle = particle.Copy();
            newParticle.Position = position;
            ParticleSystem.Instance.Spawn(newParticle, amount, 16, 20);
        }
    }
}
