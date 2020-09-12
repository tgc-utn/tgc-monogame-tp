using Microsoft.Xna.Framework;

namespace TGC.MonoGame.InsaneGames.Weapons
{
    class MachineGun : Weapon
    {

        public MachineGun () : base("armas/rifle/mp5k") {}
        public override void Initialize(TGCGame game) {
            World = Matrix.CreateScale(0.1f);
            base.Initialize(game);
        } 

    }
}