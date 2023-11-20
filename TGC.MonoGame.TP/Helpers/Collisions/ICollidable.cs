using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Helpers.Collisions;

public interface ICollidable // hice interface para que el proyectil tambien pueda implementarla
{
    public void CollidedWithSmallProp();
    public void CollidedWithLargeProp();
    public bool VerifyCollision(BoundingBox box);
} 
/* a los props les va a llegar que colisionaron y ellos les van a decir a los
 tanques y proyectiles que colisionaron con ellos */