using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Entities;

namespace WarSteel.Managers
{
    public class EntitiesManager
    {
        private Dictionary<string, Entity> entities = new Dictionary<string, Entity>();


        public static EntitiesManager _INSTANCE = null;
        public static void SetUpInstance() => _INSTANCE = new EntitiesManager();
        public static EntitiesManager Instance() => _INSTANCE;

        // methods
        public void Add(Entity entity)
        {
            entities.Add(entity.Id, entity);
        }

        public Dictionary<string, Entity> Entities() => entities;

        public Entity[] GetAll()
        {
            Entity[] array = new Entity[entities.Count];
            int index = 0;

            foreach (var componentPair in entities)
            {
                array[index] = componentPair.Value;
                index++;
            }

            return array;
        }

        public Entity GetById(string id)
        {
            if (entities.ContainsKey(id))
                return entities[id];
            else
                return null;
        }

        public Entity GetByName(string name)
        {
            foreach (var entity in entities.Values)
            {
                if (entity.Name == name)
                    return entity;
            }
            return null;
        }

        public List<Entity> GetByTag(string tag)
        {
            List<Entity> entitiesWithTag = new List<Entity>();

            foreach (var entity in entities.Values)
            {
                if (Array.Exists(entity.Tags, t => t == tag))
                    entitiesWithTag.Add(entity);
            }

            return entitiesWithTag;
        }

        public void DestroyAll()
        {
            foreach (var entity in entities.Values)
            {
                entity.OnDestroy();
            }
            entities.Clear();
        }

        public void DestroyById(string id)
        {
            if (entities.ContainsKey(id))
            {
                entities[id].OnDestroy();
                entities.Remove(id);
            }
        }

        public void DestroyByName(string name)
        {
            Entity entity = GetByName(name);
            if (entity != null)
            {
                entity.OnDestroy();
                entities.Remove(entity.Id);
            }
        }

        public void DestroyByTag(string tag)
        {
            List<Entity> entitiesWithTag = GetByTag(tag);
            foreach (var entity in entitiesWithTag)
            {
                entity.OnDestroy();
                entities.Remove(entity.Id);
            }
        }

        public void InitializeAll()
        {
            foreach (var entity in entities.Values)
            {
                entity.Initialize();
            }
        }

        public void LoadContentAll()
        {
            foreach (var entity in entities.Values)
            {
                entity.LoadContent();
            }
        }

        public void DrawAll(Camera camera)
        {
            foreach (var entity in entities.Values)
            {
                entity.Draw(camera);
            }
        }

        public void UpdateAll(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                entity.Update(gameTime);
            }
        }

        public void UnloadAll()
        {
            foreach (var entity in entities.Values)
            {
                entity.OnDestroy();
            }
            entities.Clear();
        }
    }
}
