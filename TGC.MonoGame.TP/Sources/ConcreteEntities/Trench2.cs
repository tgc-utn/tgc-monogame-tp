using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Entities;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class Trench2 : StaticPhysicEntity
    {
        protected override Model Model() => TGCGame.content.M_Trench2;
        protected override Texture2D[] Textures() => TGCGame.content.T_Trench2;
        protected override Vector3 Scale => Vector3.One / 100f;
        protected override TypedIndex Shape => TGCGame.content.Sh_Sphere20;
    }
}