using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Drawers;
using TGC.MonoGame.TP.Props.PropType;

namespace TGC.MonoGame.TP.Props;

public class PropSample : Prop
{
    protected override Model Model { get; }
    protected override IDrawer Drawer { get; }
    protected override Matrix World { get; }
    protected override float Scale { get; }

    public PropSample(Model model, IDrawer drawer, Matrix world, float scale)
    {
        Model = model;
        Drawer = drawer;
        World = world;
        Scale = scale;
    }
}