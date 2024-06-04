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

    public bool ToDestroy = false;

    public Dictionary<Type, IComponent> Components { get; private set; }

    public Transform Transform { get; }
    public Renderable Renderable { get; set; }

    public Entity(string name, string[] tags, Transform transform, Dictionary<Type, IComponent> Components)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Tags = tags;
        Transform = transform;
        this.Components = Components;
        Renderable = null;
    }

    public Entity(string name, string[] tags, Transform transform, Renderable renderable)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Tags = tags;
        Transform = transform;
        Renderable = renderable;
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

    public virtual void LoadContent()
    {
        foreach (var m in Components.Values)
        {
            m.LoadContent(this);
        }
    }
    public virtual void Draw(Scene scene)
    {
        if (Renderable != null)
            Renderable.Draw(Transform, scene);
    }
    public virtual void Update(GameTime gameTime, Scene scene)
    {
        foreach (var m in Components.Values)
        {
            m.UpdateEntity(this, gameTime, scene);
        }
    }

    public void Destroy()
    {
        ToDestroy = true;
    }

    public virtual void OnDestroy(Scene scene)
    {
        foreach (var m in Components.Values)
        {
            m.Destroy(this, scene);
        }
    }

}