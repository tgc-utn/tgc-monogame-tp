
using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using WarSteel.Entities;

public abstract class Collider {


    private ColliderShape _colliderShape;
    private Type _type;


    public ColliderShape ColliderShape{
        get => _colliderShape;
    }

    public Type Type{
        get => _type;
    }

    public Collider(ColliderShape colliderShape){
        _colliderShape = colliderShape;
        _type = GetType();
    }

    public abstract void OnCollide(Collider other);

}

public interface ColliderShape {

    public abstract IShape GetShape();

    public abstract BodyInertia GetInertia(DynamicBody body);

}

public class BoxCollider : ColliderShape {

    private float _height;
    private float _width;
    private float _length;

    public BoxCollider(float height, float width, float length)  {
        _height = height;
        _width = width;
        _length = length;
    }

    public  BodyInertia GetInertia(DynamicBody body)
    {
        return ((Box) GetShape()).ComputeInertia(body.Mass);
    }

    public  IShape GetShape()
    {
        return new Box(_width,_height,_length);
    }

}



