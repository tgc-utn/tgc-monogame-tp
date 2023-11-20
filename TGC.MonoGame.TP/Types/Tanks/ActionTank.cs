using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Types.Tanks;

public abstract class ActionTank
{
    public bool isEnemy { get; set; }
    
    public abstract void Update(GameTime gameTime, Tank tank);

    public abstract void Respawn(Tank tank);
}