using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using WarSteel.Entities;

namespace WarSteel.Scenes.SceneProcessors;

public class PhysicsProcessor : ISceneProcessor
{

    private Dictionary<string, RigidBody> SimulatedBodies = new Dictionary<string, RigidBody>();

    private Dictionary<RigidBody, List<RigidBody>> ColliderBodies = new Dictionary<RigidBody, List<RigidBody>>();

    private Vector3 Gravity;

    private float Drag;

    private float AngularDrag;

    public PhysicsProcessor(Vector3 Gravity, float Drag, float AngularDrag)
    {
        this.Gravity = Gravity;
        this.Drag = Drag;
        this.AngularDrag = AngularDrag;
    }

    public void Draw(Scene scene) { }

    public void Initialize(Scene scene) { }

    public void Update(Scene scene, GameTime gameTime)
    {

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var rigidBody in SimulatedBodies.Values)
        {
            IntegrateVelocities(rigidBody, dt);

            DetectColissions(rigidBody);

            ResolveColissions(rigidBody);
        }
    }

    public void IntegrateVelocities(RigidBody rigidBody, float dt)
    {

        if (rigidBody.isFixed) return;

        rigidBody.AddForce(Gravity);

        Vector3 dv = (rigidBody.Forces - rigidBody.Velocity * Drag) / rigidBody.Mass * dt;
        Vector3 dw = Vector3.Transform(rigidBody.Torques - rigidBody.AngularVelocity * AngularDrag, rigidBody.InvInertiaTensor) * dt;

        rigidBody.Velocity += dv;
        rigidBody.AngularVelocity += dw;

        rigidBody.Transform.Pos += rigidBody.Velocity * dt;

        Quaternion dq = Quaternion.Multiply(new Quaternion(dw.X, dw.Y, dw.Z, 0), dt / 2) * rigidBody.Transform.Orientation;
        rigidBody.Transform.Orientation += dq;
        rigidBody.Transform.Orientation = Quaternion.Multiply(rigidBody.Transform.Orientation, 1 / rigidBody.Transform.Orientation.Length());

        rigidBody.Forces = Vector3.Zero;
        rigidBody.Torques = Vector3.Zero;

    }

    public void DetectColissions(RigidBody rigidBody)
    {

        foreach (var r in SimulatedBodies.Values)
        {

            if (ColliderBodies[r].Contains(rigidBody)) break;

            if (rigidBody != r && Collides(rigidBody, r))
            {
                ColliderBodies[rigidBody].Add(r);
            }

        }
    }

    public void ResolveColissions(RigidBody A)
    {

        // TODO : Al chano lo balearon por menos. Ver si hay una solucion mejor.
        foreach (RigidBody B in ColliderBodies[A])
        {

            switch (A.Collider.GetColliderType())
            {

                case ColliderType.BOX:
                    switch (B.Collider.GetColliderType())
                    {
                        case ColliderType.BOX:
                            BoxBoxCollision(A, B);
                            break;
                        case ColliderType.SPHERE:
                            BoxSphereCollision(A, B);
                            break;
                    }
                    break;

                case ColliderType.SPHERE:
                    switch (B.Collider.GetColliderType())
                    {
                        case ColliderType.BOX:
                            BoxSphereCollision(B, A);
                            break;
                        case ColliderType.SPHERE:
                            SphereSphereCollision(A, B);
                            break;
                    }
                    break;

            }

        }
    }

    private void SphereSphereCollision(RigidBody a, RigidBody b)
    {
        SphereCollider aCollider = (SphereCollider)a.Collider;
        SphereCollider bCollider = (SphereCollider)b.Collider;

        if (aCollider.GetBoundingSphere().Intersects(bCollider.GetBoundingSphere()))
        {
            Vector3 normal = bCollider.GetCenter() - aCollider.GetCenter();
            normal.Normalize();

            b.Velocity = (2 * normal * Vector3.Dot(normal, b.Velocity) - b.Velocity) *  (1 - a.Mass / (b.Mass + a.Mass)) ;
            a.Velocity = (2 * -normal * Vector3.Dot(-normal, a.Velocity) - a.Velocity) *  (1 - b.Mass / (a.Mass + b.Mass)) ;

        }


    }

    private void BoxSphereCollision(RigidBody a, RigidBody b)
    {
        throw new NotImplementedException();
    }

    private void BoxBoxCollision(RigidBody a, RigidBody b)
    {

        BoxCollider colliderA = (BoxCollider)a.Collider;
        BoxCollider colliderB = (BoxCollider)b.Collider;

        // Check for AABB overlap
        if (colliderA.GetBoundingBox().Intersects(colliderB.GetBoundingBox()))
        {
            // Calculate collision normal
            Vector3 collisionNormal = CalculateCollisionNormal(a, b);
            a.Velocity = (2 * collisionNormal * Vector3.Dot(collisionNormal,a.Velocity) - a.Velocity) * (1 - a.Mass / (b.Mass + a.Mass)) ;
            b.Velocity = (- 2 * collisionNormal * Vector3.Dot(collisionNormal,b.Velocity) - b.Velocity) *  (1 - b.Mass / (a.Mass + b.Mass)) ;
        }

    }



    private Vector3 CalculateCollisionNormal(RigidBody a, RigidBody b)
    {

        BoxCollider aC = (BoxCollider)a.Collider;
        BoxCollider bC = (BoxCollider)b.Collider;

        BoundingBox bbA = aC.GetBoundingBox();
        BoundingBox bbB = bC.GetBoundingBox();

        float xOverlap = Math.Min(bbA.Max.X, bbB.Max.X) - Math.Max(bbA.Min.X, bbB.Min.X);
        float yOverlap = Math.Min(bbA.Max.Y, bbB.Max.Y) - Math.Max(bbA.Min.Y, bbB.Min.Y);
        float zOverlap = Math.Min(bbA.Max.Z, bbB.Max.Z) - Math.Max(bbA.Min.Z, bbB.Min.Z);

        // Determine axis of least overlap
        Vector3 collisionNormal = Vector3.Zero;
        float minOverlap = float.MaxValue;
        if (xOverlap < minOverlap)
        {
            minOverlap = xOverlap;
            collisionNormal = Vector3.UnitX;
        }
        if (yOverlap < minOverlap)
        {
            minOverlap = yOverlap;
            collisionNormal = Vector3.UnitY;
        }
        if (zOverlap < minOverlap)
        {
            collisionNormal = Vector3.UnitZ;
        }

        return collisionNormal;
    }

    public void RemoveRigidBody(RigidBody body)
    {
        SimulatedBodies.Remove(body.Id);
    }

    public void AddRigidBody(RigidBody body)
    {
        SimulatedBodies.Add(body.Id, body);
        ColliderBodies.Add(body, new List<RigidBody>());
    }

    public static bool Collides(RigidBody r1, RigidBody r2)
    {
        return r1.Collider.GetBoundingBox().Intersects(r2.Collider.GetBoundingBox());
    }

}
