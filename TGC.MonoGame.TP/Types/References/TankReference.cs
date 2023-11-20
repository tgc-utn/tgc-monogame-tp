using System.Collections.Generic;

namespace TGC.MonoGame.TP.Types.References;

public class TankReference
{
    public ModelReference Tank { get; }
    public string TurretBoneName;
    public string CannonBoneName;
    public List<string> LeftWheelsBoneNames;
    public List<string> RightWheelsBoneNames;
    public string LeftTreadBoneName;
    public string RightTreadBoneName;
    
    public TankReference(ModelReference tank, string turretBoneName, string cannonBoneName, 
        List<string> leftWheelsBoneNames, string leftTrackBoneName, List<string> rightWheelsBoneNames, string rightTrackBoneName)
    {
        Tank = tank;
        TurretBoneName = turretBoneName;
        CannonBoneName = cannonBoneName;
        LeftTreadBoneName = leftTrackBoneName;
        RightTreadBoneName = rightTrackBoneName;
        LeftWheelsBoneNames = leftWheelsBoneNames;
        RightWheelsBoneNames = rightWheelsBoneNames;
    }
}