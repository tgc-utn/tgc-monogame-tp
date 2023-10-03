using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.References;

namespace TGC.MonoGame.TP.Props.PropType.StaticProps;

public class SmallStaticProp : StaticProp
{
    public SmallStaticProp(PropReference modelReference) : base(modelReference) {}
    
    public SmallStaticProp(PropReference modelReference, Vector3 position) : base(modelReference, position) {}

    public override void Update(ICollidable collidable)
    {
        var intersects = Box.Intersects(collidable.GetBoundingBox()); 
        if(intersects)
            CollidedWith(collidable);
    }

    public override void CollidedWith(ICollidable other)
    {
        other.CollidedWithSmallProp();
        Console.WriteLine("Me choco un: " + other.GetType().Name + $"{DateTime.Now}");
        Destroyed = true;
    }
}