using BepuPhysics;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Entities
{
    internal abstract class KinematicEntity : BodyEntity
    {
        protected override BodyHandle CreateBody(Vector3 position, Quaternion rotation) =>
            TGCGame.physicSimulation.CreateKinematic(position, rotation, Shape);
    }
}