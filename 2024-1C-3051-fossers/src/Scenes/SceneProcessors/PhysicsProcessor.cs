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
        }

    }

    public void IntegrateVelocities(RigidBody rigidBody, float dt)
    {

        Vector3 forces = Gravity;
        Vector3 torques = Vector3.Zero;

        rigidBody.IntegrateVelocities(forces,torques,dt);

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



}
