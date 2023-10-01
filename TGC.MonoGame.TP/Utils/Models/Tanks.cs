using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Types;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Models;

public class Tanks
{
    public static readonly ModelReference KF51 = new ModelReference(
        $"{ContentFolder.Models}/tanks/kf51/source/kf51",
        0.1f,
        Matrix.CreateRotationX((float)Math.PI / 2) * Matrix.CreateRotationY((float)Math.PI / 2),
        Color.Yellow
    );

    public static readonly ModelReference T90 = new ModelReference(
        $"{ContentFolder.Models}/tanks/T90/T90",
        1f,
        Matrix.CreateRotationX((float)Math.PI * 3 / 2),
        Color.DarkOrchid
    );

    /*
     public static readonly ModelReference Panzer = new ModelReference(
        $"{ContentFolder.Models}/tanks/Panzer/Panzer",
        0.1f,
        Matrix.CreateRotationX((float)Math.PI / 2),
        Color.DarkSeaGreen
    );
    */
}