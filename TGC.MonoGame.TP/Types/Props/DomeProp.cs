using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Helpers.Collisions;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Types.Props;

public class DomeProp : StaticProp
{
    public DomeProp(PropReference modelReference, Vector3 position) : base(modelReference, position) {}
    
    public override void CollidedWith(ICollidable other)
    {
        return;
    }
}