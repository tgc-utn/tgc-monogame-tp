using System;
using System.Collections.Generic;
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

    private List<Vector3> _forces;

    private List<Vector3> _torques;

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

    public List<Vector3> Forces
    {
        get => _forces;
    }

    public List<Vector3> Torques
    {
        get => _torques;
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
        _forces = new List<Vector3>();
        _torques = new List<Vector3>();
    }

    public void ApplyForce(Vector3 force){
        _forces.Add(force);
    }

    public void ApplyTorque(Vector3 torque){
        _torques.Add(torque);
    }

    public void Initialize(Entity self, Scene scene) { }

    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene) { }

    public void Destroy(Entity self, Scene scene) { }
}







