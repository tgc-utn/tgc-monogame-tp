using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Entities
{
    internal abstract class KinematicEntity<S> : BodyEntity<S> where S : unmanaged, IConvexShape
    {
        protected override BodyHandle CreateBody(Vector3 position, Quaternion rotation) =>
            TGCGame.physicSimulation.CreateKinematic(position, rotation, Shape);
    }
}