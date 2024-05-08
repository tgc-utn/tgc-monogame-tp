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
    public Transform Transform;
    public ICollider Collider;
    public Vector3 Velocity;
    public Vector3 AngularVelocity;
    public float Mass;
    public Matrix InvInertiaTensor;
    public Vector3 DeltaVelocity;
    public Vector3 DeltaAngularVelocity;
    public Vector3 Forces;
    public Vector3 Torques;

    public bool isFixed;

    public RigidBody(Transform transform, ICollider collider, float mass, Matrix inertiaTensor, bool isFixed)
    {
        Transform = transform;
        Collider = collider;
        Mass = mass;
        InvInertiaTensor = Matrix.Invert(inertiaTensor);
        DeltaVelocity = Vector3.Zero;
        DeltaAngularVelocity = Vector3.Zero;
        Forces = Vector3.Zero;
        Torques = Vector3.Zero;
        Id = Guid.NewGuid().ToString();
        this.isFixed = isFixed;
    }

    public void Initialize(Entity self, Scene scene) { }

    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene) { }

    public void AddVelocity(Vector3 Velocity)
    {
        DeltaVelocity += Velocity;
    }

    public void AddAngularVelocity(Vector3 AngularVelocity)
    {
        DeltaAngularVelocity = AngularVelocity;
    }

    public void AddForce(Vector3 Force)
    {
        Forces += Force;
    }

    public void AddTorque(Vector3 Torque)
    {
        Torques += Torque;
    }

    public void Destroy(Entity self, Scene scene)
    {
        PhysicsProcessor processor = scene.GetSceneProcessor<PhysicsProcessor>();
        if (processor != null)
        {
            processor.RemoveRigidBody(this);
        }
    }
}

public interface ICollider
{
    public ColliderType GetColliderType();

    public BoundingBox GetBoundingBox();

}

public enum ColliderType
{
    BOX,
    SPHERE,
}

public class BoxCollider : ICollider
{

    private readonly Transform transform;
    private Vector3 min;
    private Vector3 max;

    public BoxCollider(Transform transform, Vector3 min, Vector3 max)
    {
        this.transform = transform;
        this.min = min;
        this.max = max;
    }

    public ColliderType GetColliderType()
    {
        return ColliderType.BOX;
    }

    public BoundingBox GetBoundingBox()
    {
        return new BoundingBox(Vector3.Transform(min, transform.GetWorld()), Vector3.Transform(max, transform.GetWorld()));
    }

}

public class SphereCollider : ICollider
{
    private readonly Transform transform;
    private float radius;
    private Vector3 center;

    public SphereCollider(Transform transform, float radius, Vector3 center)
    {
        this.radius = radius;
        this.center = center;
        this.transform = transform;
    }

    public ColliderType GetColliderType(){
        return ColliderType.SPHERE;
    }

    public BoundingBox GetBoundingBox(){
        Vector3 currCenter = Vector3.Transform(center,transform.GetWorld());
        return new BoundingBox(currCenter + new Vector3(currCenter.X - radius, currCenter.Y - radius, currCenter.Z - radius), currCenter + new Vector3(currCenter.X + radius, currCenter.Y + radius, currCenter.Z + radius));
    }

}


