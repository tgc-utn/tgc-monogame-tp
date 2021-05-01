using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Sources.Entities
{
    class TIE : Entity
    {
        protected override Model Model() => ModelManager.TIE;
        protected override Texture2D[] Textures() => TextureManager.TIE;
        public TIE(Vector3 position, Quaternion rotation) : base(position, rotation, Vector3.One / 100f) { }
    }
}