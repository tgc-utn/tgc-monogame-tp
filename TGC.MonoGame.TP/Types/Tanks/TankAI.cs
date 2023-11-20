using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Types.Tanks;

public class TankAI : Tank
{
    public TankAI(TankReference model, Vector3 position, GraphicsDeviceManager graphicsDeviceManager) : base(model, position, graphicsDeviceManager)
    {
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        //TODO aca implementar todo lo de la IA
    }
}