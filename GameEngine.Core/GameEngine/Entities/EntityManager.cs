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

        public SpriteBatch SpriteBatch { get; private set; }
        public GraphicsDeviceManager Graphics { get; private set; }

        public EntityManager(SpriteBatch spriteBatch, GraphicsDeviceManager graphics) {
            SpriteBatch = spriteBatch;
            Graphics = graphics;
        }

        // TODO: load and save entities
        //public void LoadEntities() { }
        //public void SaveEntities() { }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void RemoveEntity(int index)
        {
            entities.RemoveAt(index);
        }

        public void UpdateEntities(GameTime gameTime, KeyboardState keyboardState)
        {
            entities.ForEach(e =>
            {
                e.Update(gameTime, keyboardState, Graphics, this);
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

        public bool IsColliding(Entity current, string tag)
        {
            return entities.Any(e => 
                e.Tag == tag 
                && current.BoundingBox.Intersects(e.BoundingBox)
            );
        }

        public Entity? GetEntityByTag(string tag)
        {
            return entities.FirstOrDefault(e => e.Tag == tag);
        }
    }
}
