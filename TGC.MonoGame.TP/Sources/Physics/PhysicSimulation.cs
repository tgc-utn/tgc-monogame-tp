using System;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;
using BEPUVector3 = System.Numerics.Vector3;

namespace TGC.MonoGame.TP.Physics
{
    internal class PhysicSimulation
    {
        private readonly Simulation simulation;
        internal readonly BufferPool bufferPool = new BufferPool();
        private readonly SimpleThreadDispatcher threadDispatcher;

        private readonly BEPUVector3 gravity = new BEPUVector3();
        private const float timestep = 1 / 60f;

        internal readonly CollitionEvents collitionEvents = new CollitionEvents();

        internal PhysicSimulation()
        {
            threadDispatcher = new SimpleThreadDispatcher(ThreadCount());
            simulation = CreateSimulation();
        }

        private int ThreadCount() => Math.Max(1, Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1);

        private Simulation CreateSimulation() => Simulation.Create(
            bufferPool, new NarrowPhaseCallbacks(),
            new PoseIntegratorCallbacks(gravity, 0f, 0f), new PositionFirstTimestepper()
        );

        internal void Update() => simulation.Timestep(timestep, threadDispatcher);

        internal BodyReference GetBody(BodyHandle handle) => simulation.Bodies.GetBodyReference(handle);

        internal StaticHandle CreateStatic<S>(Vector3 position, Quaternion rotation, in S shape) where S : unmanaged, IConvexShape =>
            simulation.Statics.Add(new StaticDescription(
                position.ToBEPU(),
                rotation.ToBEPU(),
                new CollidableDescription(simulation.Shapes.Add(shape), 0.1f))
            );

        internal BodyHandle CreateDynamic<S>(Vector3 position, Quaternion rotation, in S shape, float mass) where S : unmanaged, IConvexShape
        {
            shape.ComputeInertia(mass, out BodyInertia inertia);
            BodyDescription bodyDescription = BodyDescription.CreateDynamic(
                new RigidPose(position.ToBEPU(),  rotation.ToBEPU()),
                new BodyVelocity(new BEPUVector3(0f, 0f, 0f)),
                inertia,
                new CollidableDescription(simulation.Shapes.Add(shape), 0.1f),
                new BodyActivityDescription(-1));
            return simulation.Bodies.Add(bodyDescription);
        }

        internal BodyHandle CreateKinematic<S>(Vector3 position, Quaternion rotation, in S shape) where S : unmanaged, IConvexShape
        {
            BodyDescription bodyDescription = BodyDescription.CreateKinematic(
                new RigidPose(position.ToBEPU(), rotation.ToBEPU()),
                new BodyVelocity(new BEPUVector3(0f, 0f, 0f)),
                new CollidableDescription(simulation.Shapes.Add(shape), 0.1f),
                new BodyActivityDescription(-1));
            return simulation.Bodies.Add(bodyDescription);
        }

        internal void Dispose()
        {
            simulation.Dispose();
            bufferPool.Clear();
            threadDispatcher.Dispose();
        }
    }
}