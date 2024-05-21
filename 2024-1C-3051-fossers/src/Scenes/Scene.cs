using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Entities;
using WarSteel.Managers.Gizmos;

namespace WarSteel.Scenes;

public class Scene
{
    private Dictionary<string, Entity> entities = new Dictionary<string, Entity>();
    protected GraphicsDeviceManager Graphics;
    protected Camera camera;
    protected PhysicsProcessor physics = new PhysicsProcessor();
    private Dictionary<Type, ISceneProcessor> SceneProcessors = new Dictionary<Type, ISceneProcessor>();
    protected Gizmos _gizmos = new Gizmos();

    public Gizmos Gizmos
    {
        get => _gizmos;
    }

    // We collect entities to remove separately to avoid changing the main list of entities
    // while we're still going through it. This prevents any mix-ups or errors that might happen
    // if we tried to remove entities directly during the loop.
    // After we've finished going through all the entities, we safely remove the collected ones.
    List<Entity> entitiesToRemove = new List<Entity>();

    public Scene(GraphicsDeviceManager graphics)
    {
        Graphics = graphics;
    }

    public void SetCamera(Camera camera)
    {
        entities.Add(camera.Id, camera);
        this.camera = camera;
    }

    public Camera GetCamera()
    {
        return camera;
    }

    public GraphicsDeviceManager GetGraphicsDevice()
    {
        return Graphics;
    }

    public void AddSceneProcessor(ISceneProcessor p)
    {
        SceneProcessors.Add(p.GetType(), p);
    }

    public void AddEntity(Entity entity)
    {
        entities.Add(entity.Id, entity);
        // adding the physics can be done via the initialize method in each entity, 
        // in that case we would be avoiding these ifs, 
        // though we would actually have to remember to do it on every implementation!
        if (entity.HasComponent<DynamicBody>())
            physics.AddBody(entity.GetComponent<DynamicBody>());
        if (entity.HasComponent<StaticBody>())
            physics.AddBody(entity.GetComponent<StaticBody>());
    }

    public void RemoveEntity(Entity entity)
    {
        if (entity != null && entities.ContainsKey(entity.Id))
            entitiesToRemove.Add(entity);
    }

    private void DeleteEntity(Entity entity)
    {
        entities.Remove(entity.Id);

        // Remove the physics body if it exists
        if (entity.HasComponent<DynamicBody>())
            physics.RemoveDynamicBody(entity.GetComponent<DynamicBody>());
        if (entity.HasComponent<StaticBody>())
            physics.RemoveStaticBody(entity.GetComponent<StaticBody>());
    }

    public T GetSceneProcessor<T>() where T : class, ISceneProcessor
    {
        return SceneProcessors.TryGetValue(typeof(T), out var processor) ? processor as T : default;
    }

    public List<Entity> GetEntities()
    {
        List<Entity> list = new List<Entity>();
        foreach (var e in entities.Values)
        {
            list.Add(e);
        }
        return list;
    }


    public Entity GetEntityByName(string name)
    {
        foreach (var entity in entities.Values)
        {
            if (entity.Name == name)
                return entity;
        }
        return null;
    }


    public virtual void Initialize()
    {
        foreach (var entity in entities.Values)
        {
            entity.Initialize(this);
        }

        foreach (var processor in SceneProcessors.Values)
        {
            processor.Initialize(this);
        }

    }

    public virtual void LoadContent()
    {
        foreach (var entity in entities.Values)
        {
            entity.LoadContent();
        }
    }

    public virtual void Draw()
    {
        foreach (var entity in entities.Values)
        {
            entity.Draw(this);
        }

        foreach (var processor in SceneProcessors.Values)
        {
            processor.Draw(this);
        }
    }

    public virtual void DrawGizmos()
    {
        _gizmos.Draw();
    }

    public virtual void Update(GameTime gameTime)
    {
        // creating a copy here to prevent weird behaviors 
        // while adding a new entity to the list and iterating over it at the same time in the game loop 
        var copyEntities = new Dictionary<string, Entity>(entities);
        foreach (var entity in copyEntities.Values)
        {
            entity.Update(gameTime, this);
        }

        foreach (var processor in SceneProcessors.Values)
        {
            processor.Update(this, gameTime);
        }

        // remove the entities 
        foreach (Entity entity in entitiesToRemove)
            DeleteEntity(entity);

        entitiesToRemove.Clear();

    }

    public virtual void Unload()
    {
        _gizmos.Dispose();
    }

}