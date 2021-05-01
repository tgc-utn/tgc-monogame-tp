using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Sources.Entities
{
    class XWing : Entity
    {
        protected override Model Model() => ModelManager.XWing;
        protected override Texture2D[] Textures() => TextureManager.XWing;
        public XWing(Vector3 position, Quaternion rotation) : base(position, rotation, Vector3.One) { }
    }
}