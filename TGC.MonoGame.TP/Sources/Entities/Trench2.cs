using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Sources.Entities
{
    class Trench2 : Entity
    {
        protected override Model Model() => ModelManager.Trench2;
        protected override Texture2D[] Textures() => TextureManager.Trench2;
        public Trench2(Vector3 position, Quaternion rotation) : base(position, rotation, Vector3.One / 100f) { }
    }
}