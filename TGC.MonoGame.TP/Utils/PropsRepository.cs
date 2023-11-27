using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Types.Props;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils;

public static class PropsRepository
{
    public static StaticProp InitializeProp(PropReference modelReference, Vector3 position)
    {
        return modelReference.PropType switch
        {
            PropType.Small => new SmallStaticProp(modelReference, position),
            PropType.Large => new LargeStaticProp(modelReference, position),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public static LimitProp InitializeLimitProp(PropReference modelReference, Vector3 position)
    {
        return new LimitProp(modelReference, position);
    }
}