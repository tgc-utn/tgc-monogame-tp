using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils.Models;

public class Props
{
    public static readonly ModelReference BuildingHouse0 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_1",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse1 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_2",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse2 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_15",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse3 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_12",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse4 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_13",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse5 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_14",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse6 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_7",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse7 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_8",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse8 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_9",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse9 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_10",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse10 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_26",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse11 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_17",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse12 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_18",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse13 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_29",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse14 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_20",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse15 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_21",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse16 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_22",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse17 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_23",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse18 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_28",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference BuildingHouse19 = new ModelReference(
        $"{ContentFolder.Models}/props/structures/building_25",
        1f,
        Matrix.Identity,
        Textures.Props.BuildingHouse
    );

    public static readonly ModelReference Farm = new ModelReference(
        $"{ContentFolder.Models}/props/farm/farm",
        0.01f,
        Matrix.CreateRotationY((float)Math.PI / 2),
        Textures.Props.Farm
    );

    public static readonly ModelReference Farm2 = new ModelReference(
        $"{ContentFolder.Models}/props/farm/farm",
        0.01f,
        Matrix.CreateRotationY((float)Math.PI * 3/ 2),
        Textures.Props.Farm
    );

    public static readonly ModelReference Rock0 = new ModelReference(
        $"{ContentFolder.Models}/props/stones/Rock_11",
        2f,
        Matrix.Identity,
        Textures.Props.Rock0
    );

    public static readonly ModelReference Rock1 = new ModelReference(
        $"{ContentFolder.Models}/props/stones/Rock_14",
        2f,
        Matrix.Identity,
        Textures.Props.Rock1
    );

    public static readonly ModelReference Rock2 = new ModelReference(
        $"{ContentFolder.Models}/props/stones/Rock_15",
        2f,
        Matrix.Identity,
        Textures.Props.Rock2
    );

    public static readonly ModelReference Wall1 = new ModelReference(
        $"{ContentFolder.Models}/props/walls/wall_2",
        1f,
        Matrix.Identity,
        Textures.Props.Wall1
    );

    public static readonly ModelReference Wall2 = new ModelReference(
        $"{ContentFolder.Models}/props/walls/wall_3",
        1f,
        Matrix.Identity,
        Textures.Props.Wall2
    );

    public static readonly ModelReference Wall3 = new ModelReference(
        $"{ContentFolder.Models}/props/walls/wall_3",
        1f,
        Matrix.Identity,
        Textures.Props.Wall2
    );

    public static readonly ModelReference PlaneScene = new ModelReference(
        $"{ContentFolder.Models}/scenary/plane",
        34f,
        Matrix.Identity,
        Textures.Scenarios.Plane
    );
    
    public static readonly ModelReference KF51 = new ModelReference(
        $"{ContentFolder.Models}/tanks/kf51/source/kf51",
        0.1f,
        Matrix.CreateRotationX((float)Math.PI / 2) * Matrix.CreateRotationY((float)Math.PI / 2),
        Textures.Tanks.KF51
    );

    public static readonly ModelReference T90 = new ModelReference(
        $"{ContentFolder.Models}/tanks/T90/T90",
        1f,
        Matrix.CreateRotationX((float)Math.PI * 3 / 2),
        Textures.Tanks.T90
    );
    
    public static readonly ModelReference T90V2 = new ModelReference(
        $"{ContentFolder.Models}/tanks/T90/T90",
        1f,
        Matrix.CreateRotationX((float)Math.PI * 3 / 2),
        Textures.Tanks.T90V2
    );
}