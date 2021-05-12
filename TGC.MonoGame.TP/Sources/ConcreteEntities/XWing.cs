using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Entities;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class XWing : PhysicEntity<Sphere>
    {
        protected override Model Model() => TGCGame.modelManager.XWing;
        protected override Texture2D[] Textures() => TGCGame.textureManager.XWing;

        protected override Sphere Shape() => new Sphere(20f);
        protected override float Mass() => 100f;
        protected override Vector3 Scale { get; } = Vector3.One;
    }
}