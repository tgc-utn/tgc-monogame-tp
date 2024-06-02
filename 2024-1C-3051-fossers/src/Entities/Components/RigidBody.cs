using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Managers.Gizmos;
using WarSteel.Scenes;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace WarSteel.Entities;

public abstract class RigidBody : IComponent
{
    private Transform _transform;

    private Entity _entity;
    protected Collider _collider;
    public Vector3 Offset;

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

    public RigidBody(Collider collider, Vector3 offset)
    {
        _collider = collider;
        Offset = offset;
    }

    public virtual void Initialize(Entity self, Scene scene)
    {
        _entity = self;
        _transform = self.Transform;
        PhysicsProcessor processor = scene.GetSceneProcessor<PhysicsProcessor>();
        processor.AddBody(this);
    }

    public virtual void LoadContent(Entity self) { }

    public virtual void UpdateEntity(Entity self, GameTime gameTime, Scene scene){}

    public virtual void DrawGizmos(Gizmos gizmos)
    {
        _collider.ColliderShape.DrawGizmos(_transform.Position + Offset, gizmos);
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
    public StaticBody(Collider collider, Vector3 offset) : base(collider, offset) { }

    public override void Build(PhysicsProcessor processor)
    {
        TypedIndex index = processor.AddShape(_collider);
        Vector3 position = Transform.Position + Offset;
        StaticDescription staticDescription = new StaticDescription(
            new System.Numerics.Vector3(position.X, position.Y, position.Z),
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

    private float _dragCoeff = 0.2f;

    private float _angularDragCoeff = 0.3f;

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

    public float AngularDrag
    {
        get => _angularDragCoeff;
    }

    public float Drag
    {
        get => _dragCoeff;
    }

    public Vector3 Force
    {
        get => _forces;
    }

    public Vector3 Torque
    {
        get => _torques;
    }

    public DynamicBody(Collider collider, Vector3 offset, float mass, float dragCoeff, float angularDragCoeff) : base(collider, offset)
    {
        _mass = mass;
        _dragCoeff = dragCoeff;
        _angularDragCoeff = angularDragCoeff;
    }

    public override void Initialize(Entity entity, Scene scene){
        base.Initialize(entity,scene);
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

        Vector3 position = Transform.Position + Offset;

        BodyDescription bodyDescription = BodyDescription.CreateDynamic(

            new System.Numerics.Vector3(position.X, position.Y, position.Z),
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







