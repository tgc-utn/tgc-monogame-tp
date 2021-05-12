using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Entities;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    class Trench2 : NonPhysicEntity
    {
        protected override Model Model() => TGCGame.modelManager.Trench2;
        protected override Texture2D[] Textures() => TGCGame.textureManager.Trench2;
        public Trench2(Vector3 position, Quaternion rotation) : base(position, rotation, Vector3.One / 100f) { }
    }
}