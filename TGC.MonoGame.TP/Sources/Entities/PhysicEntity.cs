using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP.Entities
{
    internal abstract class PhysicEntity<S> : Entity where S: unmanaged, IConvexShape
    {
        protected Vector3 Position() => TGCGame.physicSimulation.GetBody(handle).Pose.Position.ToVector3();
        protected Quaternion Rotation() => TGCGame.physicSimulation.GetBody(handle).Pose.Orientation.ToQuaternion();
        protected virtual Vector3 Scale { get; }

        protected abstract S Shape();
        protected abstract float Mass();
        private BodyHandle handle;

        internal void Instantiate(Vector3 position) => Instantiate(position, Quaternion.Identity);
        internal void Instantiate(Vector3 position, Quaternion rotation)
        {
            handle = TGCGame.physicSimulation.CreateDynamic(position, rotation, Shape(), Mass());
            TGCGame.world.Register(this);
        }

        protected override Matrix GeneralWorldMatrix()
        {
            RigidPose pose = TGCGame.physicSimulation.GetBody(handle).Pose;
            return Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(pose.Orientation.ToQuaternion()) * Matrix.CreateTranslation(pose.Position.ToVector3());
        }
    }
}