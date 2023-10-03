using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Helpers.Collisions;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Types.Props;

public class SmallStaticProp : StaticProp
{
    public SmallStaticProp(PropReference modelReference) : base(modelReference) {}
    
    public SmallStaticProp(PropReference modelReference, Vector3 position) : base(modelReference, position) {}

    public override void CollidedWith(ICollidable other)
    {
        other.CollidedWithSmallProp();
        Console.WriteLine("Me choco un: " + other.GetType().Name + $"{DateTime.Now}");
        Destroyed = true;
    }
}