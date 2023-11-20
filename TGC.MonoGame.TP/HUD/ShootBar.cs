using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Utils.Effects;

namespace TGC.MonoGame.TP.HUD;

public class ShootBar : BarHud
{
    internal override (float X, float Y) Location() => (0f, -1.8f);
    internal override (float Ancho, float Alto) QuadSize() => (Window.Width*0.001f,Window.Heigth*0.00015f);
    
    public ShootBar(GraphicsDeviceManager graphicsDevice) : base(graphicsDevice) {}

    public override void Load(ContentManager contentManager)
    {
        Effect = contentManager.Load<Effect>(Effects.ShootHud.Path);
    }
}