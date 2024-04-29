using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.Entities;

public class Entity
{
    public string Id { get; }
    public string Name { get; }
    public string[] Tags { get; }

    public Component[] Modifiers {get;}

    public Transform Transform { get; }
    protected Renderable _renderable { get; set; }

    public Entity(string name, string[] tags, Transform transform, Component[] modifiers)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Tags = tags;
        Transform = transform;
        Modifiers = modifiers;
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

    public virtual void Initialize() { }
    public virtual void LoadContent() { }
    public virtual void Draw(Scene scene)
    {
        if (_renderable != null)
            _renderable.Draw(Transform.GetWorld(), scene);
    }
    public virtual void Update(GameTime gameTime,Scene scene)
    {
        foreach(var m in Modifiers){
            m.UpdateEntity(this,gameTime,scene);
        }
    }
    public virtual void OnDestroy() {}
}