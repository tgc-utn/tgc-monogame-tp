using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Scenes;

namespace WarSteel.Entities;

public class Entity
{
    public string Id { get; }
    public string Name { get; }
    public string[] Tags { get; }

    public Dictionary<Type, IComponent> Components { get; private set; }

    public Transform Transform { get; }
    protected Renderable _renderable { get; set; }

    public Entity(string name, string[] tags, Transform transform, Dictionary<Type, IComponent> Components)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Tags = tags;
        Transform = transform;
        this.Components = Components;
        _renderable = null;
    }

    public Entity(string name, string[] tags, Transform transform, Renderable renderable)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Tags = tags;
        Transform = transform;
        _renderable = renderable;
    }

    public void AddComponent(IComponent c)
    {
        Components.Add(c.GetType(), c);
    }

    public T GetComponent<T>() where T : class, IComponent
    {
        return Components.TryGetValue(typeof(T), out var processor) ? processor as T : default;
    }

    public bool HasComponent<T>() where T : class, IComponent
    {
        return Components.TryGetValue(typeof(T), out var pr);
    }


    public virtual void Initialize(Scene scene)
    {
        foreach (var c in Components.Values)
        {
            c.Initialize(this, scene);
        }
    }
    public virtual void LoadContent() { }
    public virtual void Draw(Scene scene)
    {
        if (_renderable != null)
            _renderable.Draw(Transform.GetWorld(), scene);
    }
    public virtual void Update(GameTime gameTime, Scene scene)
    {
        foreach (var m in Components.Values)
        {
            m.UpdateEntity(this, gameTime, scene);
        }
    }

    public virtual void OnDestroy(Scene scene)
    {
        foreach (var m in Components.Values)
        {
            m.Destroy(this,scene);
        }
    }

}