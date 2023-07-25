using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Core.EntityManagement
{
    public class EntityManager
    {
        public List<Entity> entities = new();
        public long lastEntityId = 0;
        public SpriteBatch SpriteBatch { get; private set; }
        public RenderTarget2D RenderTarget { get; private set; }

        public EntityManager(SpriteBatch spriteBatch, RenderTarget2D renderTarget)
        {
            SpriteBatch = spriteBatch;
            RenderTarget = renderTarget;
        }

        // TODO: load and save entities
        //public void LoadEntities() { }
        //public void SaveEntities() { }

        public void AddEntity(Entity entity)
        {
            entity.Id = NewEntityId();
            entities.Add(entity);
        }

        public long NewEntityId()
        {
            lastEntityId++;
            return lastEntityId;
        }

        public void RemoveEntity(long id)
        {
            entities = entities.Where(e => e.Id != id).ToList();
        }

        public void UpdateEntities(GameTime gameTime, KeyboardState keyboardState)
        {
            entities.ForEach(e =>
            {
                e.Update(gameTime, keyboardState, RenderTarget, this);
            });
        }

        public void RenderEntities()
        {
            entities.ForEach(e =>
            {
                e.Render(SpriteBatch);
            });
        }

        public void DebugRenderEntities(Texture2D debugTexture, Color debugColor)
        {
            entities.ForEach(e =>
            {
                e.DebugRender(SpriteBatch, debugTexture, debugColor);
            });
        }

        public bool IsColliding(Entity current, int tag)
        {
            return entities.Any(e => 
                e.Tag == tag 
                && current.BoundingBox.Intersects(e.BoundingBox)
            );
        }

        public TEntity GetEntityByTag<TEntity>(int tag) where TEntity : Entity
        {
            var result = entities.FirstOrDefault(e => e.Tag == tag);
            if (result == null)
            {
                return default;
            }
            return result as TEntity;
        }
    }
}
