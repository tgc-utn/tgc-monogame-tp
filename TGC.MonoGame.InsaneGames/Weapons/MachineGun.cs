using Microsoft.Xna.Framework;

namespace TGC.MonoGame.InsaneGames.Weapons
{
    class MachineGun : Weapon
    {

        public MachineGun () : base("armas/rifle/machine-gun") {}
        public override void Initialize(TGCGame game) {
            World = Matrix.CreateScale(1);
            base.Initialize(game);
        } 

    }
}