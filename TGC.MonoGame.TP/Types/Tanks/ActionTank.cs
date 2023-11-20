using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Types.Tanks;

public abstract class ActionTank
{
    public int Team { get; set; }
    public ActionTank(int team)
    {
        Team = team;
    }
    
    public abstract void Update(GameTime gameTime, Tank tank);
}