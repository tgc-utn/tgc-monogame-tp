using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Entities;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class TIE : NonPhysicEntity
    {
        protected override Model Model() => TGCGame.modelManager.TIE;
        protected override Texture2D[] Textures() => TGCGame.textureManager.TIE;
        internal TIE(Vector3 position, Quaternion rotation) : base(position, rotation, Vector3.One / 100f) { }
    }
}