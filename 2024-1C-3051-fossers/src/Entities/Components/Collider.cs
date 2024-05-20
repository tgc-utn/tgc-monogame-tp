
using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using WarSteel.Entities;

<<<<<<< HEAD
public abstract class Collider {


    private ColliderShape _colliderShape;
    private Type _type;
=======
public abstract class Collider
{
    protected List<string> _tags;

    protected Dictionary<string, object> _data;
>>>>>>> 566d4e15d7035fcd9a94c69b8e68c31774d0dbec


<<<<<<< HEAD
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

=======
    public List<string> Tags
    {
        get => _tags;
    }

    public Dictionary<string, object> Data
    {
        get => Data;
    }

    public Collider(List<string> tags, Dictionary<string, Object> data, List<ColliderListener> listener)
    {
        _tags = tags;
        _data = data;
        _listeners = listener;
    }

    public void OnCollide(Collider otherCollider)
    {
        if (otherCollider != this)
        {
            _listeners.ForEach(l => l.listen(otherCollider));
        }
    }

>>>>>>> 566d4e15d7035fcd9a94c69b8e68c31774d0dbec
    public abstract IShape GetShape();

    public abstract BodyInertia GetInertia(DynamicBody body);
}

<<<<<<< HEAD
public class BoxCollider : ColliderShape {
=======
public class BoxCollider : Collider
{
>>>>>>> 566d4e15d7035fcd9a94c69b8e68c31774d0dbec

    private float _height;
    private float _width;
    private float _length;

<<<<<<< HEAD
    public BoxCollider(float height, float width, float length)  {
=======
    public BoxCollider(List<string> tags, Dictionary<string, object> data, List<ColliderListener> colliderListeners, float height, float width, float length) : base(tags, data, colliderListeners)
    {
>>>>>>> 566d4e15d7035fcd9a94c69b8e68c31774d0dbec
        _height = height;
        _width = width;
        _length = length;
    }

    public  BodyInertia GetInertia(DynamicBody body)
    {
        return ((Box)GetShape()).ComputeInertia(body.Mass);
    }

    public  IShape GetShape()
    {
        return new Box(_width, _height, _length);
    }

}

<<<<<<< HEAD

=======
public interface ColliderListener
{

    public void listen(Collider collider);

}
>>>>>>> 566d4e15d7035fcd9a94c69b8e68c31774d0dbec

