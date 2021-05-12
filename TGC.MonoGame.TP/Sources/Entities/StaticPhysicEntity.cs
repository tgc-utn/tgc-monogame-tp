using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Entities
{
    internal abstract class StaticPhysicEntity<S> : StaticEntity where S : unmanaged, IConvexShape
    {
        protected abstract S Shape { get; }

        internal void Instantiate(Vector3 position) => Instantiate(position, Quaternion.Identity);
        internal void Instantiate(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
            TGCGame.physicSimulation.CreateStatic(position, rotation, Shape);
            TGCGame.world.Register(this);
        }
    }
}