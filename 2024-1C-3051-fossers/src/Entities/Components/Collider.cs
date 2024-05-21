
using System;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Entities;
using WarSteel.Managers.Gizmos;

public class Collision
{
    public Entity Entity;

    public Collision(Entity entity)
    {
        Entity = entity;
    }
}

public class Collider
{
    private ColliderShape _colliderShape;
    private Type _type;


    public ColliderShape ColliderShape
    {
        get => _colliderShape;
    }

    public Type Type
    {
        get => _type;
    }

    public Collider(ColliderShape colliderShape)
    {
        _colliderShape = colliderShape;
        _type = GetType();
    }

    public virtual void OnCollide(Collision collision) { }
}

public interface ColliderShape
{
    public abstract IShape GetShape();

    public abstract BodyInertia GetInertia(DynamicBody body);

    public abstract void DrawGizmos(Transform transform, Gizmos gizmos);
}

public class BoxCollider : ColliderShape
{
    private float _height;
    private float _width;
    private float _length;

    public BoxCollider(float height, float width, float length)
    {
        _height = height;
        _width = width;
        _length = length;
    }

    public BodyInertia GetInertia(DynamicBody body)
    {
        return ((Box)GetShape()).ComputeInertia(body.Mass);
    }

    public IShape GetShape()
    {
        return new Box(_width, _height, _length);
    }

    public void DrawGizmos(Transform transform, Gizmos gizmos)
    {
        gizmos.DrawCube(transform.Pos, new Vector3(_height, _width, _length));
    }
}