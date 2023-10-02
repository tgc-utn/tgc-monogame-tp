using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Helpers.Collisions;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Types.Props;

public class LargeStaticProp : StaticProp
{
    public LargeStaticProp(PropReference modelReference) : base(modelReference) {}
    public LargeStaticProp(PropReference modelReference, Vector3 position) : base(modelReference, position) {}

    public override void Update(ICollidable collidable)
    {
        var intersects = Box.Intersects(collidable.GetBoundingBox());
        if (intersects)
            CollidedWith(collidable);
    }

    public override void CollidedWith(ICollidable other)
    {
        other.CollidedWithLargeProp();
        Console.WriteLine("Me choco un: " + other.GetType().Name);
    }
}