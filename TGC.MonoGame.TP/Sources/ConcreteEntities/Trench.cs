using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Entities;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class Trench : NonPhysicEntity
    {
        protected override Model Model() => TGCGame.modelManager.Trench;
        protected override Texture2D[] Textures() => TGCGame.textureManager.Trench;
        internal Trench(Vector3 position, Quaternion rotation) : base(position, rotation, Vector3.One / 100f) { }
    }
}