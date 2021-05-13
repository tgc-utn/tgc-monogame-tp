using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Entities;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class TIE : DynamicEntity<Sphere>
    {
        protected override Model Model() => TGCGame.content.M_TIE;
        protected override Texture2D[] Textures() => TGCGame.content.T_TIE;

        protected override Vector3 Scale => Vector3.One / 100f;
        protected override Sphere Shape => new Sphere(20f);
        protected override float Mass => 100f;

        internal override void Update(double elapsedTime)
        {
            BodyReference body = Body();

            Quaternion rotation = body.Pose.Orientation.ToQuaternion();
            Vector3 forward = PhysicUtils.Forward(rotation);

            body.Velocity.Linear = (forward * 100f).ToBEPU();
            body.Velocity.Angular = new Vector3(0f, 0.5f, 0).ToBEPU();
        }
    }
}