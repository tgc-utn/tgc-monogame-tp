using System.Collections.Generic;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Models;

public class Tanks
{
    public static readonly TankReference T90 = new TankReference(
        Props.T90,
        "Turret",
        "Cannon",
        new List<string>(){"Wheel1","Wheel2","Wheel3","Wheel4","Wheel5","Wheel6","Wheel7","Wheel8"},
        "Treadmill1",
        new List<string>{"Wheel9","Wheel10","Wheel11","Wheel12","Wheel13","Wheel14","Wheel15","Wheel16"},
        "Treadmill2"
    );
    
    public static readonly TankReference T90V2 = new TankReference(
        Props.T90V2,
        "Turret",
        "Cannon",
        new List<string>{"Wheel1","Wheel2","Wheel3","Wheel4","Wheel5","Wheel6","Wheel7","Wheel8"},
        "Treadmill1",
        new List<string>{"Wheel9","Wheel10","Wheel11","Wheel12","Wheel13","Wheel14","Wheel15","Wheel16"},
        "Treadmill2"
    );
}