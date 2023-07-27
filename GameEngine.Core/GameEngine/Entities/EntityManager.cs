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

        public void DrawEntities(GameTime gameTime)
        {
            entities.ForEach(e =>
            {
                e.Draw(SpriteBatch, gameTime);
            });
        }

        public void DrawRenderEntities(Texture2D debugTexture, Color debugColor, Color debugColor2)
        {
            entities.ForEach(e =>
            {
                e.DebugRender(SpriteBatch, debugTexture, debugColor, debugColor2);
            });
        }

        public bool IsColliding(Entity current, int tag, out Entity collidedEntity)
        {
            collidedEntity = entities.FirstOrDefault(e =>
                e.Tag == tag
                && current.BoundingBox.Intersects(e.BoundingBox)
            );
            return collidedEntity != null;
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
