using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP.Entities
{
    internal abstract class StaticPhysicEntity<S> : StaticEntity, ICollitionHandler where S : unmanaged, IConvexShape
    {
        protected abstract S Shape { get; }

        internal void Instantiate(Vector3 position) => Instantiate(position, Quaternion.Identity);
        internal void Instantiate(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
            StaticHandle handle = TGCGame.physicSimulation.CreateStatic(position, rotation, Shape);
            TGCGame.physicSimulation.collitionEvents.RegisterCollider(handle, this);
            TGCGame.world.Register(this);
        }

        public virtual bool HandleCollition(ICollitionHandler other) => false;
    }
}