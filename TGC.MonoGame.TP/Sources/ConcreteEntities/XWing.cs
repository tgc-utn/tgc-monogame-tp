using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Entities;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class XWing : DynamicEntity<Sphere>
    {
        protected override Model Model() => TGCGame.modelManager.XWing;
        protected override Texture2D[] Textures() => TGCGame.textureManager.XWing;

        protected override Vector3 Scale => Vector3.One;
        protected override Sphere Shape => new Sphere(20f);
        protected override float Mass => 100f;

        public override bool HandleCollition(ICollitionHandler other)
        {
            TGCGame.soundManager.Explotion.CreateInstance().Play();
            return false;
        }
    }
}