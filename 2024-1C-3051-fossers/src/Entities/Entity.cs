using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Managers;

namespace WarSteel.Entities;

public class Entity
{
    // identifiers
    public string Id { get; }
    public string Name { get; }
    public string[] Tags { get; }

    public Component[] Modifiers {get;}

    public Transform Transform { get; }
    protected Renderable _renderable { get; set; }

    public Entity(string name, string[] tags, Transform transform, Component[] modifiers)
    {
        // creates a random unique identifier
        Id = Guid.NewGuid().ToString();
        Name = name;
        Tags = tags;
        Transform = transform;
        Modifiers = modifiers;
        _renderable = null;
    }

    public Entity(string name, string[] tags, Transform transform, Renderable renderable)
    {
        // creates a random unique identifier
        Id = Guid.NewGuid().ToString();
        Name = name;
        Tags = tags;
        Transform = transform;
        _renderable = renderable;
    }

    public virtual void Initialize(Camera camera) { }
    public virtual void LoadContent(Camera camera) { }
    public virtual void Draw(Camera camera)
    {
        if (_renderable != null)
            _renderable.Draw(Transform.World, camera);
    }
    public virtual void Update(GameTime gameTime, Camera camera)
    {
        foreach(var m in Modifiers){
            m.UpdateEntity(this,gameTime,nearbyEntities);
        }
        Transform.UpdateWorldMatrix();
    }
    public virtual void OnDestroy() {}
}