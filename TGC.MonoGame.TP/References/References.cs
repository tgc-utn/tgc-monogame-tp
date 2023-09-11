using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Props.PropType;
using TGC.MonoGame.TP.Tanks;

namespace TGC.MonoGame.TP.References;

public class ModelReference
{
    public string Path { get; }
    public float Scale { get; }
    public Matrix Rotation { get; }
    public Color Color { get; }

    public ModelReference(string model, float scale, Matrix normal, Color color)
    {
        Path = model;
        Scale = scale;
        Rotation = normal;
        Color = color;
    }
}

public class ScenaryReference : ModelReference
{
    public Vector3 EnemiesSpawn { get; }
    public Vector3 AliesSpawn { get; }
    public List<PropReference> PropsReference { get; }
    public ScenaryReference(string model, float scale, Matrix normal, Color color, Vector3 enemySpawn, Vector3 alieSpawn, List<PropReference> propsReference) : base(model, scale, normal, color)
    {   
        EnemiesSpawn = enemySpawn;
        AliesSpawn = alieSpawn;
        PropsReference = propsReference;
    }
}

public class PropReference
{
    public ModelReference Prop { get; }
    public Vector3 Position { get; }
    public int MeshIndex { get; }
    public int Repetitions { get; }
    
    public PropReference(ModelReference prop, Vector3 position, int meshIndex, int repetitions = 1)
    {
        Prop = prop;
        Position = position;
        MeshIndex = meshIndex;
        Repetitions = repetitions;
    }
}

public class EffectReference
{
    public string Path { get; }

    public EffectReference(string effect)
    {
        Path = effect;
    }
}

public static class ContentFolder
{
    public const string Models = "Models";
    public const string Effects = "Effects";
    public const string Music = "Music";
    public const string Sounds = "Sounds";
    public const string SpriteFonts = "SpriteFonts";
    public const string Textures = "Textures";
}

public static class Models
{
    public static class Props
    {
        public static readonly ModelReference Building = new ModelReference(
            $"{ContentFolder.Models}/props/buildings/buildings",
            0.01f,
            Matrix.Identity,
            Color.DarkRed
        );

        public static readonly ModelReference Farm = new ModelReference(
            $"{ContentFolder.Models}/props/medieval-farm/vlase",
            0.012f,
            Matrix.Identity,
            Color.DarkCyan
        );

        public static readonly ModelReference Rock = new ModelReference(
            $"{ContentFolder.Models}/props/rocks/rocks",
            0.012f,
            Matrix.Identity,
            Color.DarkBlue
        );

        public static readonly ModelReference Wall = new ModelReference(
            $"{ContentFolder.Models}/props/walls/source/walls",
            0.05f,
            Matrix.Identity,
            Color.DarkGray
        );
    }

    public static class Tank
    {
        public static readonly ModelReference KF51 = new ModelReference(
            $"{ContentFolder.Models}/tanks/kf51/source/kf51",
            0.1f,
            Matrix.CreateRotationX((float)Math.PI / 2) * Matrix.CreateRotationY((float)Math.PI / 2),
            Color.DarkGreen
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

    public static class Scenary
    {
        public static readonly ScenaryReference Desert = new ScenaryReference(
            $"{ContentFolder.Models}/scenary/scenary",
            0.5f,
            Matrix.Identity,
            Color.DarkSalmon,
            new Vector3(-200f, 0f, -100f),
            new Vector3(200f, 50f, 100f),
            new List<PropReference>
            {
                new PropReference(Props.Building, new Vector3(0f, 10f, 0f), 1, 5),
                new PropReference(Props.Building, new Vector3(20f, 10f, 20f), 2, 5),
                new PropReference(Props.Building, new Vector3(30f, 10f, 30f), 3, 5),
                new PropReference(Props.Building, new Vector3(40f, 10f, 100f), 4, 5),
                new PropReference(Props.Building, new Vector3(70f, 10f, 100f), 5, 5),
                new PropReference(Props.Building, new Vector3(-60f, 10f, -100f), 6, 5),
                new PropReference(Props.Wall, new Vector3(100f, 10f, 0f), 7, 5),
                new PropReference(Props.Wall, new Vector3(-100f, 10f, 0f), 7, 5),
                new PropReference(Props.Rock, new Vector3(90f, 10f, 100f), 1, 5),
                new PropReference(Props.Rock, new Vector3(-100f, 10f, 100f), 2, 5),
                new PropReference(Props.Rock, new Vector3(110f, 10f, -100f), 3, 5),
                new PropReference(Props.Rock, new Vector3(-120f, 10f, 100f), 4, 5),
                new PropReference(Props.Rock, new Vector3(130f, 10f, -100f), 5, 5),
                new PropReference(Props.Rock, new Vector3(-140f, 10f, 100f), 6, 5),
                new PropReference(Props.Rock, new Vector3(150f, 10f, -100f), 7, 5),
                new PropReference(Props.Rock, new Vector3(-160f, 10f, 100f), 8, 5),
                new PropReference(Props.Farm, new Vector3(250f, 100f, 150f), -1, 1),
                new PropReference(Props.Farm, new Vector3(-250f, 100f, -150f), -1, 1),
            }
        );
    }
}

public static class Effects
{
    public static readonly EffectReference BasicShader = new EffectReference(
        $"{ContentFolder.Effects}/BasicShader"
    );
}
