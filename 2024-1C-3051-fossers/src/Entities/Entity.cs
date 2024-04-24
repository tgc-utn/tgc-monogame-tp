using System;
using WarSteel.Common;



namespace WarSteel.Entities;

public class Entity
{
    // identifiers
    public string Id { get; }
    public string Name { get; }
    public string[] Tags { get; }

    public Transform Transform { get; }
    private Renderable _renderable { get; }

    public Entity(string name, string[] tags, Transform transform)
    {
        // creates a random unique identifier
        Id = Guid.NewGuid().ToString();
        Name = name;
        Tags = tags;
        Transform = transform;
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

    public void Initialize() { }
    public void Draw(Camera camera)
    {
        _renderable.Draw(Transform.World, camera);
    }
    public void Update() { }
    public void OnDestroy() { }
}