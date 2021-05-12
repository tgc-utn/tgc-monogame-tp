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
        private Vector3 scale;

        protected abstract S Shape();
        protected abstract float Mass();
        private readonly BodyHandle handle;

        internal PhysicEntity(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            handle = TGCGame.physicSimulation.CreateDynamic(position, rotation, Shape(), Mass());
            this.scale = scale;
        }

        protected override Matrix GeneralWorldMatrix()
        {
            RigidPose pose = TGCGame.physicSimulation.GetBody(handle).Pose;
            return Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(pose.Orientation.ToQuaternion()) * Matrix.CreateTranslation(pose.Position.ToVector3());
        }
    }
}