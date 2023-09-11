using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Props.PropType;

public abstract class DynamicProp : Prop
{
    protected Quaternion Rotation { get; }
    public abstract void Update(float dTime, KeyboardState keyboard);
    public override void Update(GameTime gameTime)
    {
        // Rotar - Acelerar - Movimiento
        return;
    }
}