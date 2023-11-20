using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Models;

public class Tanks
{
    public static readonly TankReference KF51 = new TankReference(
        Props.KF51,
        "KF51_Turret_Msh",
        "Gun_Msh"
    );

    public static readonly TankReference T90 = new TankReference(
        Props.T90,
        "Turret",
        "Cannon"
    );
    
    public static readonly TankReference T90V2 = new TankReference(
        Props.T90V2,
        "Turret",
        "Cannon"
    );
}