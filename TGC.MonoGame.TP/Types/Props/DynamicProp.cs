using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Types;

namespace TGC.MonoGame.TP.Props.PropType;

public abstract class DynamicProp : Resource
{
    protected Quaternion Rotation { get; }
    public abstract void Update(float dTime, KeyboardState keyboard);
}