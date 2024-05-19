
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Scenes;
using Vector3 = System.Numerics.Vector3;


class PhysicsProcessor : ISceneProcessor
{
    private Simulation _simulation;

    private Dictionary<RigidBody, BodyHandle> simIndex = new Dictionary<RigidBody, BodyHandle>();
    private Dictionary<RigidBody, StaticHandle> simIndexStatic = new Dictionary<RigidBody, StaticHandle>();

    public void Draw(Scene scene) { }

    public void Initialize(Scene scene)
    {

        BufferPool bufferPool = new BufferPool();
        SolveDescription solveDescription = new SolveDescription(1, 1, 8);

        _simulation = Simulation.Create(bufferPool, new NarrowPhaseCallbacks(), new PoseIntegratorCallbacks(), solveDescription);

        RigidBody[] rigidBodies = scene.GetEntities().FindAll(e => e.HasComponent<RigidBody>()).Select(e => e.GetComponent<RigidBody>()).ToArray();

        foreach (var r in rigidBodies)
        {
            switch (r.Collider.ColliderType)
            {
                case ColliderType.SPHERE:
                    AddAsSphere(r);
                    break;
                case ColliderType.BOX:
                    AddAsBox(r);
                    break;
            }
        }
    }

    public void AddAsSphere(RigidBody r)
    {
        SphereCollider collider = (SphereCollider)r.Collider;
        Sphere sphere = new Sphere(collider.Radius);
        TypedIndex index = _simulation.Shapes.Add(sphere);
        BodyDescription bodyDescription = BodyDescription.CreateDynamic(
            new Vector3(r.Pos.X, r.Pos.Y, r.Pos.Z),
            new BodyInertia { InverseMass = 1 / r._mass },
            new CollidableDescription(index),
            new BodyActivityDescription(0.01f)
            );
        BodyHandle handle = _simulation.Bodies.Add(bodyDescription);
        simIndex.Add(r, handle);
    }

    public void AddAsBox(RigidBody r)
    {
        BoxCollider collider = (BoxCollider)r.Collider;
        Box box = new Box(collider.HalfWidths.X, collider.HalfWidths.Y, collider.HalfWidths.Z);
        TypedIndex index = _simulation.Shapes.Add(box);
        if (r.IsFixed)
        {
            StaticDescription staticDescription = new StaticDescription(
                new Vector3(r.Pos.X, r.Pos.Y, r.Pos.Z),
                index
            );
            StaticHandle handle = _simulation.Statics.Add(staticDescription);
            simIndexStatic.Add(r, handle);
        }
        else
        {
            BodyDescription bodyDescription = BodyDescription.CreateDynamic(
                new Vector3(r.Pos.X, r.Pos.Y, r.Pos.Z),
                box.ComputeInertia(r._mass),
                new CollidableDescription(index),
                new BodyActivityDescription(0.01f)
            );
            bodyDescription.Pose.Orientation = new System.Numerics.Quaternion(r.Orientation.X,r.Orientation.Y,r.Orientation.Z,r.Orientation.W);
            BodyHandle handle = _simulation.Bodies.Add(bodyDescription);
            simIndex.Add(r, handle);
        }

    }

    public void Update(Scene scene, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds + 0.0001f;

        _simulation.Timestep(dt);

        foreach (var r in simIndex.Keys)
        {
            BodyReference body = _simulation.Bodies[simIndex[r]];
            r.Pos = body.Pose.Position;
            r.Orientation = body.Pose.Orientation;
            r.Velocity = body.Velocity.Linear;
            r.AngularVelocity = body.Velocity.Angular;
            if (r.Forces.Concat(r.Torques).Count() > 0) body.Awake = true;
            foreach (var f in r.Forces){
                body.ApplyLinearImpulse(new Vector3(f.X,f.Y,f.Z));
            }
            foreach( var t in r.Torques){
                body.ApplyAngularImpulse(new Vector3(t.X,t.Y,t.Z));
            }
            r.Forces.Clear();
            r.Torques.Clear();
        }
    }


}


public struct NarrowPhaseCallbacks : INarrowPhaseCallbacks
{
    public bool AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b, ref float speculativeMargin)
    {
        return true;
    }

    public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB)
    {
        return true;
    }

    public bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold, out PairMaterialProperties pairMaterial) where TManifold : unmanaged, IContactManifold<TManifold>
    {
        pairMaterial.FrictionCoefficient = 1f;
        pairMaterial.MaximumRecoveryVelocity = 2f;
        pairMaterial.SpringSettings = new SpringSettings(30, 1);
        return true;
    }

    public bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB, ref ConvexContactManifold manifold)
    {
        return true;
    }

    public void Dispose()
    {

    }

    public void Initialize(Simulation simulation)
    {

    }
}

public struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks
{

    Vector3 Gravity = new Vector3(0, -100, 0);

    public PoseIntegratorCallbacks()
    {
    }

    public AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;

    public bool AllowSubstepsForUnconstrainedBodies => true;

    public bool IntegrateVelocityForKinematics => true;

    public void Initialize(Simulation simulation)
    {

    }

    public void IntegrateVelocity(Vector<int> bodyIndices, Vector3Wide position, QuaternionWide orientation, BodyInertiaWide localInertia, Vector<int> integrationMask, int workerIndex, Vector<float> dt, ref BodyVelocityWide velocity)
    {
        Vector3Wide gravityWide;
        Vector3Wide.Broadcast(Gravity * dt[0], out gravityWide);
        Vector3Wide.Add(velocity.Linear, gravityWide, out velocity.Linear);

    }

    public void PrepareForIntegration(float dt)
    {

    }
}




