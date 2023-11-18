using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Models;

public class Tanks
{
    public static readonly TankReference KF51 = new TankReference(
        Props.KF51,
        "KF51_Turret_Msh",
        "Gun_Msh",
        new List<string>{},
        "",
        new List<string>{},
        ""
    );

    public static readonly TankReference T90 = new TankReference(
        Props.T90,
        "Turret",
        "Cannon",
        new List<string>{"Wheel1","Wheel2","Wheel3","Wheel4","Wheel5","Wheel6","Wheel7","Wheel8"},
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

    /*
     public static readonly ModelReference Panzer = new ModelReference(
        $"{ContentFolder.Models}/tanks/Panzer/Panzer",
        0.1f,
        Matrix.CreateRotationX((float)Math.PI / 2),
        new ColorReference(Color.DarkSeaGreen)
    );
    */
}