
using System;
using System.Collections.Generic;
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
using Quaternion = System.Numerics.Quaternion;
using Vector3 = System.Numerics.Vector3;


public class PhysicsProcessor : ISceneProcessor
{
    private Simulation _simulation;

    private List<(StaticBody, StaticHandle)> _staticBodies = new List<(StaticBody, StaticHandle)>();
    private List<(DynamicBody, BodyHandle)> _dynamicBodies = new List<(DynamicBody, BodyHandle)>();

    public PhysicsProcessor()
    {
        BufferPool bufferPool = new BufferPool();
        SolveDescription solveDescription = new SolveDescription(30, 5);


        _simulation = Simulation.Create(bufferPool, new NarrowPhaseCallbacks(this), new PoseIntegratorCallbacks(), solveDescription);

    }

    public void Draw(Scene scene) { }

    public void Initialize(Scene scene)
    {
    }

    public void AddBody(RigidBody r)
    {
        r.Build(this);
    }

    public void RemoveDynamicBody(DynamicBody r)
    {
        foreach (var (b, h) in _dynamicBodies)
        {
            if (b == r)
            {
                _simulation.Bodies.Remove(h);
                _dynamicBodies.Remove((b, h));
                break;
            }
        }

    }

    public void RemoveStaticBody(StaticBody r)
    {
        foreach (var (b, h) in _staticBodies)
        {
            if (b == r)
            {
                _simulation.Statics.Remove(h);
                _staticBodies.Remove((b, h));
                break;
            }
        }
    }


    public void Update(Scene scene, GameTime gameTime)
    {

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        dt = dt == 0 ? 0.0001f : dt;

        _simulation.Timestep(dt);


        foreach (var (r, k) in _dynamicBodies)
        {
            BodyReference body = _simulation.Bodies[k];
            body.Awake = true;

            Microsoft.Xna.Framework.Matrix m = Microsoft.Xna.Framework.Matrix.CreateFromQuaternion(body.Pose.Orientation);

            r.Transform.Position = body.Pose.Position - Microsoft.Xna.Framework.Vector3.Transform(r.Offset,m);
            r.Transform.Orientation = body.Pose.Orientation;

            body.ApplyLinearImpulse(new Vector3(r.Force.X, r.Force.Y, r.Force.Z));
            body.ApplyAngularImpulse(new Vector3(r.Torque.X, r.Torque.Y, r.Torque.Z));
            body.ApplyLinearImpulse(-body.Velocity.Linear * r.Drag);
            body.ApplyAngularImpulse(-body.Velocity.Angular * r.AngularDrag);

        }
    }

    internal TypedIndex AddShape(Collider collider)
    {
        IShape shape = collider.ColliderShape.GetShape();

        if (shape is Box boxShape)
        {
            return _simulation.Shapes.Add(boxShape);
        }

        if (shape is Sphere sphereShape)
        {
            return _simulation.Shapes.Add(sphereShape);
        }


        if (shape is ConvexHull hullShape)
        {
            return _simulation.Shapes.Add(hullShape);
        }


        else throw new InvalidOperationException("Unsupported shape added!");

    }

    internal void AddStatic(StaticBody body, StaticDescription staticDescription)
    {
        StaticHandle handle = _simulation.Statics.Add(staticDescription);
        _staticBodies.Add((body, handle));
    }

    internal void AddDynamic(DynamicBody body, BodyDescription bodyDescription)
    {
        BodyHandle handle = _simulation.Bodies.Add(bodyDescription);
        _dynamicBodies.Add((body, handle)); ;
    }

    internal DynamicBody FindDynamic(BodyHandle handle)
    {
        foreach (var (a, b) in _dynamicBodies)
        {
            if (handle == b) return a;
        }
        return null;
    }

    internal StaticBody FindStatic(StaticHandle handle)
    {
        foreach (var (a, b) in _staticBodies)
        {
            if (handle == b) return a;
        }
        return null;
    }

    public RigidBody GetRigidBodyFromCollision(CollidableReference r)
    {
        if (r.Mobility == CollidableMobility.Static)
        {
            return FindStatic(r.StaticHandle);
        }
        if (r.Mobility == CollidableMobility.Dynamic)
        {
            return FindDynamic(r.BodyHandle);
        }
        return null;
    }


}


public struct NarrowPhaseCallbacks : INarrowPhaseCallbacks
{

    private PhysicsProcessor _processor;

    public NarrowPhaseCallbacks(PhysicsProcessor processor)
    {
        _processor = processor;
    }

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
        pairMaterial.FrictionCoefficient = 0.1f;
        pairMaterial.MaximumRecoveryVelocity = 100000f;
        pairMaterial.SpringSettings = new SpringSettings(30, 3);

        RigidBody A = _processor.GetRigidBodyFromCollision(pair.A);
        RigidBody B = _processor.GetRigidBodyFromCollision(pair.B);
        A.Collider.OnCollide(new Collision(B.Entity));
        B.Collider.OnCollide(new Collision(A.Entity));


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

    private Vector3 _gravity = new Vector3(0, -100, 0);

    private float _dragCoeff = 0.2f;

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
        Vector3Wide dvGrav;
        Vector3Wide.Broadcast(_gravity, out gravityWide);
        Vector3Wide.Scale(gravityWide, dt, out dvGrav);
        Vector3Wide.Add(velocity.Linear, dvGrav, out velocity.Linear);
        Vector3Wide.Scale(velocity.Linear, -Vector.Multiply(_dragCoeff, dt), out Vector3Wide drag);
        Vector3Wide.Add(velocity.Linear, drag, out velocity.Linear);

    }

    public void PrepareForIntegration(float dt)
    {

    }
}




