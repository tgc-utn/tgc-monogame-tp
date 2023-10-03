namespace TGC.MonoGame.TP.Types.References;

public class TankReference
{
    public ModelReference Tank { get; }
    public string TurretBoneName;
    public string CannonBoneName;
    
    public TankReference(ModelReference tank, string turretBoneName, string cannonBoneName)
    {
        Tank = tank;
        TurretBoneName = turretBoneName;
        CannonBoneName = cannonBoneName;
    }
}