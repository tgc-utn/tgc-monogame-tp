using System;

using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Scenes;
using WarSteel.Scenes.SceneProcessors;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace WarSteel.Entities;

public class RigidBody : IComponent
{
    public string Id;
    private Transform _transform;
    public float _mass;
    private Matrix _inertiaTensor;
    private bool _isFixed;
    private Vector3 _velocity;
    private Vector3 _angularVelocity;
    private Collider _collider;

    public Vector3 Pos
    {
        get => _transform.Pos;
        set => _transform.Pos = value;
    }

    public Quaternion Orientation
    {
        get => _transform.Orientation;
        set => _transform.Orientation = value;
    }
    
    public Vector3 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    public Vector3 AngularVelocity
    {
        get => _angularVelocity;
        set => _angularVelocity = value;
    }

    public Matrix InertiaTensor
    {
        get => _inertiaTensor;
    }

    public Collider Collider
    {
        get => _collider;
    }

    public bool IsFixed
    {
        get => _isFixed;
    }

    public RigidBody(Transform transform, float mass, Matrix inertiaTensor,  Collider collider, bool isFixed = false)
    {
        _transform = transform;
        _mass = mass;
        _inertiaTensor = inertiaTensor;
        Id = Guid.NewGuid().ToString();
        _isFixed = isFixed;
        _collider = collider;
        _velocity = Vector3.Zero;
        _angularVelocity = Vector3.Zero;
    }

    public void Initialize(Entity self, Scene scene) { }

    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene) { }

    public void ApplyImpulse(OVector3 impulse)
    {
        _linearMomentum += impulse.Vector;
        _angularMomentum += Vector3.Cross(Vector3.Transform(impulse.Origin, _transform.GetWorld()), impulse.Vector);
    }

    public void ApplyForces(float dt)
    {
        foreach (var f in _constForces.Concat(_forces))
        {
            OVector3 Force = f.Invoke(this);
            _linearMomentum += Force.Vector * dt;
            _angularMomentum += Vector3.Cross(Force.Origin, Force.Vector) * dt;
        }

        _forces.Clear();
    }

    public void IntegrateVelocity(float dt)
    {
        _transform.Pos += Velocity * dt;
        _transform.Orientation += new Quaternion(0.5f * dt * AngularVelocity, 0) * _transform.Orientation;
        _transform.Orientation *= 1 / _transform.Orientation.Length();
    }

    public Vector3 GetVelocityOfPoint(Vector3 p){
        return Velocity + Vector3.Cross(p - Pos, AngularVelocity);
    }

    public void Destroy(Entity self, Scene scene) { }
}







