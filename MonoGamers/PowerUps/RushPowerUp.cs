using Microsoft.Xna.Framework;
using MonoGamers.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGamers.PowerUps
{
    internal class RushPowerUp : PowerUp
    {
        Vector3 FacingDirection;
        public RushPowerUp(Vector3 position, Vector3 facingDirection) : base(position)
        {
            FacingDirection=facingDirection;
        }

        public override void Activate(MonoSphere sphere)
        {
            throw new NotImplementedException();
        }
    }
}
