
using System;
using System.Collections.Generic;
using System.Numerics;
using BepuPhysics;
using BepuPhysics.Collidables;
using WarSteel.Entities;

public abstract class Collider {

    protected List<string> _tags;

    protected Dictionary<string,object> _data;

    protected List<ColliderListener> _listeners = new List<ColliderListener>();

    public List<string> Tags {
        get => _tags;
    }

    public Dictionary<string, object> Data {
        get => Data;
    }

    public Collider(List<string> tags, Dictionary<string,Object> data, List<ColliderListener> listener){
        _tags = tags;
        _data = data;
        _listeners = listener;
    }

    public void OnCollide(Collider otherCollider){
        if (otherCollider != this){
        _listeners.ForEach(l => l.listen(otherCollider));
        }
    }


    public abstract IShape GetShape();

    public abstract BodyInertia GetInertia(DynamicBody body);

}

public class BoxCollider : Collider {

    private float _height;
    private float _width;
    private float _length;

    public BoxCollider(List<string> tags, Dictionary<string,object> data, List<ColliderListener> colliderListeners,float height, float width, float length) : base(tags,data,colliderListeners) {
        _height = height;
        _width = width;
        _length = length;
    }

    public override BodyInertia GetInertia(DynamicBody body)
    {
        return ((Box) GetShape()).ComputeInertia(body.Mass);
    }

    public override IShape GetShape()
    {
        return new Box(_width,_height,_length);
    }

}


public interface ColliderListener {

    public void listen(Collider collider);

}

