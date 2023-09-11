using System;
using Microsoft.Xna.Framework;
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
    public ScenaryReference(string model, float scale, Matrix normal, Color color, Vector3 enemySpawn, Vector3 alieSpawn) : base(model, scale, normal, color)
    {   
        EnemiesSpawn = enemySpawn;
        AliesSpawn = alieSpawn;
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
            1f,
            Matrix.CreateRotationX((float)Math.PI),
            Color.DarkRed
        );

        public static readonly ModelReference Farm = new ModelReference(
            $"{ContentFolder.Models}/props/medieval-farm/vlase",
            1f,
            Matrix.CreateRotationX((float)Math.PI),
            Color.DarkCyan
        );

        public static readonly ModelReference Rock = new ModelReference(
            $"{ContentFolder.Models}/props/rocks/rocks",
            1f,
            Matrix.CreateRotationX((float)Math.PI),
            Color.DarkBlue
        );

        public static readonly ModelReference Wall = new ModelReference(
            $"{ContentFolder.Models}/props/walls/source/walls",
            1f,
            Matrix.CreateRotationX((float)Math.PI),
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
            new Vector3(-30f, 0f, 0f),
            new Vector3(30f, 0f, 0f)
        );
    }
}

public static class Effects
{
    public static readonly EffectReference BasicShader = new EffectReference(
        $"{ContentFolder.Effects}/BasicShader"
    );
}
