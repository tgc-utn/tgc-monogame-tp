using System;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Scenes;

namespace WarSteel.Entities;

public abstract class RigidBody : IComponent
{
    private Transform _transform;
    private Entity _entity;
    protected Collider _collider;

    public Collider Collider
    {
        get => _collider;
    }

    public Transform Transform
    {
        get => _transform;
    }

    public Entity Entity
    {
        get => _entity;
    }

    public RigidBody(Transform transform, Collider collider)
    {
        _transform = transform;
        _collider = collider;
    }

    public void Initialize(Entity self, Scene scene)
    {
        PhysicsProcessor processor = scene.GetSceneProcessor<PhysicsProcessor>();
        _entity = self;
        processor.AddBody(this);
    }

    public virtual void UpdateEntity(Entity self, GameTime gameTime, Scene scene)
    {
        _collider.ColliderShape.DrawGizmos(_transform, scene.Gizmos);
    }

    public void Destroy(Entity self, Scene scene)
    {
        PhysicsProcessor processor = scene.GetSceneProcessor<PhysicsProcessor>();
        RemoveSelf(processor);
    }

    public abstract void Build(PhysicsProcessor processor);

    public abstract void RemoveSelf(PhysicsProcessor processor);
}

public class StaticBody : RigidBody
{
    public StaticBody(Transform transform, Collider collider) : base(transform, collider) { }

    public override void Build(PhysicsProcessor processor)
    {
        TypedIndex index = processor.AddShape(_collider);
        StaticDescription staticDescription = new StaticDescription(
            new System.Numerics.Vector3(Transform.Position.X, Transform.Position.Y, Transform.Position.Z),
            index
        );
        processor.AddStatic(this, staticDescription);
    }

    public override void RemoveSelf(PhysicsProcessor processor)
    {
        processor.RemoveStaticBody(this);
    }

}

public class DynamicBody : RigidBody
{
    private Vector3 _velocity;
    private Vector3 _angularVelocity;
    private float _mass;

    private Vector3 _forces = Vector3.Zero;

    private Vector3 _torques = Vector3.Zero;

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

    public float Mass
    {
        get => _mass;
    }

    public Vector3 Force
    {
        get => _forces;
    }

    public Vector3 Torque
    {
        get => _torques;
    }

    public DynamicBody(Transform transform, Collider collider, float mass) : base(transform, collider)
    {
        _mass = mass;
    }

    public override void UpdateEntity(Entity self, GameTime time, Scene scene)
    {
        _forces *= 0;
        _torques *= 0;
        base.UpdateEntity(self, time, scene);
    }

    public void ApplyForce(Vector3 force)
    {
        _forces += force;
    }

    public void ApplyTorque(Vector3 torque)
    {
        _torques += torque;
    }

    public override void Build(PhysicsProcessor processor)
    {
        TypedIndex index = processor.AddShape(_collider);

        BodyDescription bodyDescription = BodyDescription.CreateDynamic(

            new System.Numerics.Vector3(Transform.Position.X, Transform.Position.Y, Transform.Position.Z),
            _collider.ColliderShape.GetInertia(this),
            new CollidableDescription(index, 0.01f),

            new BodyActivityDescription(1000f)
        );

        processor.AddDynamic(this, bodyDescription);
    }

    public override void RemoveSelf(PhysicsProcessor processor)
    {
        processor.RemoveDynamicBody(this);
    }
}







