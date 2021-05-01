using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Sources.Entities
{
    class Trench : Entity
    {
        protected override Model Model() => ModelManager.Trench;
        protected override Texture2D[] Textures() => TextureManager.Trench;
        public Trench(Vector3 position, Quaternion rotation) : base(position, rotation, Vector3.One / 100f) { }
    }
}