
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

    private CollisionAction _action;


    public ColliderShape ColliderShape
    {
        get => _colliderShape;
    }

    public Collider(ColliderShape colliderShape, CollisionAction action)
    {
        _colliderShape = colliderShape;
        _action = action;
    }

    public void OnCollide(Collision collision) {
        _action.ExecuteAction(collision);
    }
}

public interface CollisionAction {
    public void ExecuteAction(Collision collision);
}

public class NoAction : CollisionAction {
    public void ExecuteAction(Collision collision){
    }

}


public interface ColliderShape
{
    public abstract IShape GetShape();

    public abstract BodyInertia GetInertia(DynamicBody body);

    public abstract void DrawGizmos(Transform transform, Gizmos gizmos);
}

public class BoxShape : ColliderShape
{
    private float _height;
    private float _width;
    private float _length;

    public BoxShape(float height, float width, float length)
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
        gizmos.DrawCube(transform.Position, new Vector3(_height, _width, _length));
    }
    
}