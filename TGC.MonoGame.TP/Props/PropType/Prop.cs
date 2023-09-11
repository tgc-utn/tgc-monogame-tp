using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Drawers;

namespace TGC.MonoGame.TP.Props.PropType;

public abstract class Prop
{
    protected abstract Model Model { get; }
    protected abstract IDrawer Drawer { get; }
    protected abstract Matrix World { get; }
    protected abstract float Scale { get; }

    public void Draw()
    {
        Drawer.Draw(Model, World);
    }
}
