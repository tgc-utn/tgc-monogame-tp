using System;
using System.ComponentModel;
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
    public Collider Collider;
    public Vector3 LinearMomentum;
    public Vector3 AngularMomentum;
    public float Mass;
    public Matrix InvInertiaTensor;


    public RigidBody(Transform transform, Collider collider, float mass, Matrix inertiaTensor)
    {
        Transform = transform;
        Collider = collider;
        Mass = mass;
        InvInertiaTensor = Matrix.Invert(inertiaTensor);
        AngularMomentum = Vector3.Zero;
        LinearMomentum = Vector3.Zero;
        Id = Guid.NewGuid().ToString();
    }

    public void Initialize(Entity self, Scene scene) { }

    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene) { }

    public void Destroy(Entity self, Scene scene)
    {
        PhysicsProcessor processor = scene.GetSceneProcessor<PhysicsProcessor>();
        if (processor != null)
        {
            processor.RemoveRigidBody(this);
        }
    }


    public void IntegrateVelocities(Vector3 forces, Vector3 torques, float dt){
        LinearMomentum += forces * dt;
        AngularMomentum += torques * dt;

        Transform.Pos += LinearMomentum / Mass * dt;
        Transform.Orientation += new Quaternion(0.5f * dt * Vector3.Transform(AngularMomentum,InvInertiaTensor),0) * Transform.Orientation;
        Transform.Orientation *= 1 / Transform.Orientation.Length();
    }

}


public class Collider : Component
{


}




