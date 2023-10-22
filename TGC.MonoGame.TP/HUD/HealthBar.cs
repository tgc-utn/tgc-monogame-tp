using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.Utils.Effects;
using Color = System.Drawing.Color;

namespace TGC.MonoGame.TP.HUD;

public class HealthBar : BarHud
{
    internal override (float X, float Y) Location() => (0f, -2f);

    public HealthBar(GraphicsDeviceManager graphicsDevice) : base(graphicsDevice) {}

    public override void Load(ContentManager contentManager)
    {
        Effect = contentManager.Load<Effect>(Effects.HealthHud.Path);
    }
}