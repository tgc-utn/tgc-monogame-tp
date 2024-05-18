using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    private Vector3 _linearMomentum;
    private Vector3 _angularMomentum;
    public float _mass;
    private Matrix _invInertiaTensor;
    private Matrix _inertiaTensor;
    private bool _isFixed;
    private List<Func<RigidBody, OVector3>> _forces;
    private List<Func<RigidBody, OVector3>> _constForces;

    private Collider _collider;

    public Vector3 Velocity
    {
        get => _linearMomentum / _mass;
        set => _linearMomentum = value * _mass;
    }

    public Vector3 Pos
    {
        get => _transform.Pos;
        set => _transform.Pos = value;
    }

    public Vector3 AngularVelocity
    {
        get => Vector3.Transform(_angularMomentum, _invInertiaTensor);
        set => _angularMomentum = Vector3.Transform(value, _inertiaTensor);
    }

    public List<Func<RigidBody, OVector3>> Forces
    {
        get => _forces.Concat(_constForces).ToList();
    }

    public bool IsFixed
    {
        get => _isFixed;
    }

    public Collider Collider
    {
        get => _collider;
    }

    public RigidBody(Transform transform, float mass, Matrix inertiaTensor, List<Func<RigidBody, OVector3>> constForces, Collider collider, bool isFixed = false)
    {
        _transform = transform;
        _mass = mass;
        _inertiaTensor = inertiaTensor;
        _invInertiaTensor = Matrix.Invert(inertiaTensor);
        _angularMomentum = Vector3.Zero;
        _linearMomentum = Vector3.Zero;
        _constForces = constForces;
        _forces = new List<Func<RigidBody, OVector3>>();
        Id = Guid.NewGuid().ToString();
        _isFixed = isFixed;
        _collider = collider;
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







