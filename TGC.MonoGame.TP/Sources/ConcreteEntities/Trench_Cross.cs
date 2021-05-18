using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Entities;

namespace TGC.MonoGame.TP.ConcreteEntities
{
    internal class Trench_Cross : StaticPhysicEntity
    {
        protected override Model Model() => TGCGame.content.M_Trench_Cross;
        protected override Texture2D[] Textures() => TGCGame.content.T_Trench;
        protected override Vector3 Scale => Vector3.One * DeathStar.trenchScale;
        protected override TypedIndex Shape => TGCGame.content.Sh_Trench_Cross;
    }
}