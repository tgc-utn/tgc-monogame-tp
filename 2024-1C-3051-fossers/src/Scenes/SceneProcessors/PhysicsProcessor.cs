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

        Vector3 dv = rigidBody.Forces / rigidBody.Mass * dt;
        Vector3 dw = Vector3.Transform(rigidBody.Torques, rigidBody.InvInertiaTensor) * dt;

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

    public void ResolveColissions(RigidBody rigidBody){

        if (ColliderBodies[rigidBody].Count > 0){
            rigidBody.Velocity = Vector3.Zero;
        }

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