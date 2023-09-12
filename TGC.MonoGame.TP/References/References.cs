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
    public int MeshIndex { get; } = -1;

    public ModelReference(string model, float scale, Matrix normal, Color color)
    {
        Path = model;
        Scale = scale;
        Rotation = normal;
        Color = color;
    }
    
    public ModelReference(string model, float scale, Matrix normal, Color color, int meshIndex) : this(model, scale, normal, color)
    {
        MeshIndex = meshIndex;
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
    public int Repetitions { get; }
    
    public PropReference(ModelReference prop, Vector3 position, int repetitions = 1)
    {
        Prop = prop;
        Position = position;
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
        //EdificioEnorme
        public static readonly ModelReference Building_House_0 = new ModelReference(
            $"{ContentFolder.Models}/props/buildings/buildings",
            0.01f,
            Matrix.Identity,
            Color.DarkRed,
            9
        );
        
        public static readonly ModelReference Building_House_1 = new ModelReference(
            $"{ContentFolder.Models}/props/buildings/buildings",
            0.01f,
            Matrix.Identity,
            Color.DarkRed,
            1
        );
        
        public static readonly ModelReference Building_House_2 = new ModelReference(
            $"{ContentFolder.Models}/props/buildings/buildings",
            0.01f,
            Matrix.Identity,
            Color.DarkRed,
            2
        );

        public static readonly ModelReference Building_House_3 = new ModelReference(
            $"{ContentFolder.Models}/props/buildings/buildings",
            0.01f,
            Matrix.Identity,
            Color.DarkRed,
            3
        );

        public static readonly ModelReference Building_House_4 = new ModelReference(
			$"{ContentFolder.Models}/props/buildings/buildings",
			0.01f,
			Matrix.Identity,
			Color.DarkRed,
			4
		);

        public static readonly ModelReference Building_House_5 = new ModelReference(
			$"{ContentFolder.Models}/props/buildings/buildings",
			0.01f,
			Matrix.Identity,
			Color.DarkRed,
			5
		);

        public static readonly ModelReference Building_House_6 = new ModelReference(
            $"{ContentFolder.Models}/props/buildings/buildings",
            0.01f,
            Matrix.Identity,
            Color.DarkRed,
            6
        );

        public static readonly ModelReference Building_House_7 = new ModelReference(
            $"{ContentFolder.Models}/props/buildings/buildings",
            0.01f,
            Matrix.Identity,
            Color.DarkRed,
            14
        );

        public static readonly ModelReference Building_House_8 = new ModelReference(
            $"{ContentFolder.Models}/props/buildings/buildings",
            0.01f,
            Matrix.Identity,
            Color.DarkRed,
            21
        );

        public static readonly ModelReference Building_House_9 = new ModelReference(
            $"{ContentFolder.Models}/props/buildings/buildings",
            0.01f,
            Matrix.Identity,
            Color.DarkRed,
            26
        );
        
        public static readonly ModelReference Farm = new ModelReference(
            $"{ContentFolder.Models}/props/medieval-farm/vlase",
            0.012f,
            Matrix.CreateRotationY((float)Math.PI * 3/2),
            Color.Green
        );

        public static readonly ModelReference Farm_2 = new ModelReference(
            $"{ContentFolder.Models}/props/medieval-farm/vlase",
            0.012f,
            Matrix.CreateRotationY((float)Math.PI / 2),
            Color.Orchid
        );


        //Roca medio talisman
        public static readonly ModelReference Rock_0 = new ModelReference(
            $"{ContentFolder.Models}/props/rocks/rocks",
            0.075f,
            Matrix.Identity,
            Color.Yellow,
            11
        );
        
        public static readonly ModelReference Rock_1 = new ModelReference(
            $"{ContentFolder.Models}/props/rocks/rocks",
            0.012f,
            Matrix.Identity,
            Color.DarkBlue,
            3
        );
        
        public static readonly ModelReference Rock_2 = new ModelReference(
            $"{ContentFolder.Models}/props/rocks/rocks",
            0.05f,
            Matrix.Identity,
            Color.DarkOrange,
            5
        );
        
        public static readonly ModelReference Rock_3 = new ModelReference(
            $"{ContentFolder.Models}/props/rocks/rocks",
            0.05f,
            Matrix.Identity,
            Color.DarkBlue,
            6
        );

        public static readonly ModelReference Wall = new ModelReference(
            $"{ContentFolder.Models}/props/walls/source/walls",
            0.02f,
            Matrix.Identity,
            Color.DarkGray,
            6
        );

        public static readonly ModelReference Wall_2 = new ModelReference(
            $"{ContentFolder.Models}/props/walls/source/walls",
            0.05f,
            Matrix.CreateRotationY((float)Math.PI / 2),
            Color.DarkGray,
            7
        );

        public static readonly ModelReference Wall_3 = new ModelReference(
            $"{ContentFolder.Models}/props/walls/source/walls",
            0.05f,
            Matrix.Identity,
            Color.DarkGray,
            7
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
        public static readonly ScenaryReference Plane = new ScenaryReference(
            $"{ContentFolder.Models}/scenary/plane",
            20f,
            Matrix.CreateRotationX((float) Math.PI / 4),
            Color.Black,
            new Vector3(-350f, 0f, 0f),
            new Vector3(350f, 0f, 0f),
            new List<PropReference>
            {   
                //Ciudad 1
                new PropReference(Props.Building_House_1, new Vector3(150f, 0f, 155f), 4),
                new PropReference(Props.Building_House_2, new Vector3(180f, 0f, 175f), 4),
                new PropReference(Props.Building_House_3, new Vector3(210f, 0f, 205f), 4),
                new PropReference(Props.Building_House_4, new Vector3(120f, 0f, 115f), 4),
                new PropReference(Props.Building_House_5, new Vector3(200f, 0f, 150f), 4),
                new PropReference(Props.Building_House_6, new Vector3(230f, 0f, 185f), 4),
                new PropReference(Props.Building_House_7, new Vector3(160f, 0f, 95f), 4),
                new PropReference(Props.Building_House_8, new Vector3(120f, 0f, 100f), 4),
                new PropReference(Props.Building_House_9, new Vector3(70f, 0f, 195f), 4),

                //Ciudad 2
                new PropReference(Props.Building_House_1, new Vector3(150f, 0f, -195f), 4),
                new PropReference(Props.Building_House_2, new Vector3(180f, 0f, -175f), 4),
                new PropReference(Props.Building_House_3, new Vector3(210f, 0f, -145f), 4),
                new PropReference(Props.Building_House_4, new Vector3(120f, 0f, -235f), 4),
                new PropReference(Props.Building_House_5, new Vector3(200f, 0f, -200f), 4),
                new PropReference(Props.Building_House_6, new Vector3(230f, 0f, -165f), 4),
                new PropReference(Props.Building_House_7, new Vector3(160f, 0f, -255f), 4),
                new PropReference(Props.Building_House_8, new Vector3(120f, 0f, -250f), 4),
                new PropReference(Props.Building_House_9, new Vector3(70f, 0f, -155f), 4),

                //Ciudad 3
                new PropReference(Props.Building_House_1, new Vector3(-200f, 0f, 155f), 4),
                new PropReference(Props.Building_House_2, new Vector3(-170f, 0f, 175f), 4),
                new PropReference(Props.Building_House_3, new Vector3(-140f, 0f, 205f), 4),
                new PropReference(Props.Building_House_4, new Vector3(-230f, 0f, 115f), 4),
                new PropReference(Props.Building_House_5, new Vector3(-150f, 0f, 150f), 4),
                new PropReference(Props.Building_House_6, new Vector3(-120f, 0f, 185f), 4),
                new PropReference(Props.Building_House_7, new Vector3(-210f, 0f, 95f), 4),
                new PropReference(Props.Building_House_8, new Vector3(-230f, 0f, 100f), 4),
                new PropReference(Props.Building_House_9, new Vector3(-280f, 0f, 195f), 4),

                //Ciudad 4
                new PropReference(Props.Building_House_1, new Vector3(-200f, 0f, -195f), 4),
                new PropReference(Props.Building_House_2, new Vector3(-170f, 0f, -175f), 4),
                new PropReference(Props.Building_House_3, new Vector3(-140f, 0f, -145f), 4),
                new PropReference(Props.Building_House_4, new Vector3(-230f, 0f, -235f), 4),
                new PropReference(Props.Building_House_5, new Vector3(-150f, 0f, -200f), 4),
                new PropReference(Props.Building_House_6, new Vector3(-120f, 0f, -165f), 4),
                new PropReference(Props.Building_House_7, new Vector3(-210f, 0f, -255f), 4),
                new PropReference(Props.Building_House_8, new Vector3(-230f, 0f, -250f), 4),
                new PropReference(Props.Building_House_9, new Vector3(-280f, 0f, -155f), 4),
                
                //Murallas Centro
                new PropReference(Props.Wall, new Vector3(-20f, 0f, 15f), 6),
                new PropReference(Props.Wall_2, new Vector3(150f, 0f, 80f), 1),
                new PropReference(Props.Wall_2, new Vector3(300f, 0f, 80f), 1),
                new PropReference(Props.Wall_2, new Vector3(-100f, 0f, 80f), 1),
                new PropReference(Props.Wall_2, new Vector3(-250f, 0f, 80f), 1),
             
                //Murallas Izquierda
                new PropReference(Props.Wall_2, new Vector3(100f, 0f, -265f), 1),
                new PropReference(Props.Wall_2, new Vector3(300f, 0f, -265f), 1),
                new PropReference(Props.Wall_2, new Vector3(-50f, 0f, -265f), 1),
                new PropReference(Props.Wall_2, new Vector3(-250f, 0f, -265f), 1),
                
                //Murallas Derecha
                new PropReference(Props.Wall_2, new Vector3(100f, 0f, 430f), 1),
                new PropReference(Props.Wall_2, new Vector3(300f, 0f, 430f), 1),
                new PropReference(Props.Wall_2, new Vector3(-50f, 0f, 430f), 1),
                new PropReference(Props.Wall_2, new Vector3(-250f, 0f, 430f), 1),

                //Murallas Bases
                new PropReference(Props.Wall_3, new Vector3(265f, 0f, 215f), 1),
                new PropReference(Props.Wall_3, new Vector3(265f, 0f, -145f), 1),
                new PropReference(Props.Wall_3, new Vector3(-440f, 0f, 215f), 1),
                new PropReference(Props.Wall_3, new Vector3(-440f, 0f, -145f), 1),
                
                //Granjas
                new PropReference(Props.Farm, new Vector3(350f, 0f, 0f), 1),
                new PropReference(Props.Farm, new Vector3(350f, 0f, 350f), 1),
                new PropReference(Props.Farm, new Vector3(350f, 0f, -350f), 1),
                new PropReference(Props.Farm_2, new Vector3(-350f, 0f, 0f), 1),
                new PropReference(Props.Farm_2, new Vector3(-350f, 0f, 350f), 1),
                new PropReference(Props.Farm_2, new Vector3(-350f, 0f, -350f), 1),
                
                //Rocas
                new PropReference(Props.Rock_0, new Vector3(-335f, 0, -20f), 1),
                new PropReference(Props.Rock_2, new Vector3(40f, 0, 40f), 1),
                new PropReference(Props.Rock_3, new Vector3(-10f, 0, 40f), 1),
                new PropReference(Props.Rock_2, new Vector3(-20f, 0, 40f), 1),
                new PropReference(Props.Rock_3, new Vector3(-50f, 0, 40f), 1),
                new PropReference(Props.Rock_0, new Vector3(-220f, 0, -20f), 1),
                new PropReference(Props.Rock_0, new Vector3(-180f, 0, -20f), 1),
                new PropReference(Props.Rock_2, new Vector3(130f, 0, 40f), 1),
                new PropReference(Props.Rock_3, new Vector3(100f, 0, 40f), 1),
                new PropReference(Props.Rock_0, new Vector3(-100f, 0, -20f), 1),


                new PropReference(Props.Rock_0, new Vector3(-335f, 0, -120f), 1),
                new PropReference(Props.Rock_2, new Vector3(40f, 0, -50f), 1),
                new PropReference(Props.Rock_3, new Vector3(-10f, 0, -50f), 1),
                new PropReference(Props.Rock_2, new Vector3(-20f, 0, -50f), 1),
                new PropReference(Props.Rock_3, new Vector3(-50f, 0, -50f), 1),
                new PropReference(Props.Rock_0, new Vector3(-220f, 0, -120f), 1),
                new PropReference(Props.Rock_0, new Vector3(-180f, 0, -120f), 1),
                new PropReference(Props.Rock_2, new Vector3(130f, 0, -50f), 1),
                new PropReference(Props.Rock_3, new Vector3(100f, 0, -50f), 1),
                new PropReference(Props.Rock_0, new Vector3(-100f, 0, -120f), 1),

                new PropReference(Props.Rock_0, new Vector3(-335f, 0, -370f), 1),
                new PropReference(Props.Rock_2, new Vector3(40f, 0, -300f), 1),
                new PropReference(Props.Rock_3, new Vector3(-10f, 0, -300f), 1),
                new PropReference(Props.Rock_2, new Vector3(-20f, 0, -300f), 1),
                new PropReference(Props.Rock_3, new Vector3(-50f, 0, -300f), 1),
                new PropReference(Props.Rock_0, new Vector3(-220f, 0, -370f), 1),
                new PropReference(Props.Rock_0, new Vector3(-180f, 0, -370f), 1),
                new PropReference(Props.Rock_2, new Vector3(130f, 0, -300f), 1),
                new PropReference(Props.Rock_3, new Vector3(100f, 0, -300f), 1),
                new PropReference(Props.Rock_0, new Vector3(-100f, 0, -370f), 1),

                new PropReference(Props.Rock_0, new Vector3(-335f, 0, 235f), 1),
                new PropReference(Props.Rock_2, new Vector3(40f, 0, 305f), 1),
                new PropReference(Props.Rock_3, new Vector3(-10f, 0, 305f), 1),
                new PropReference(Props.Rock_2, new Vector3(-20f, 0, 305f), 1),
                new PropReference(Props.Rock_3, new Vector3(-50f, 0, 305f), 1),
                new PropReference(Props.Rock_0, new Vector3(-220f, 0, 235f), 1),
                new PropReference(Props.Rock_0, new Vector3(-180f, 0, 235f), 1),
                new PropReference(Props.Rock_2, new Vector3(130f, 0, 305f), 1),
                new PropReference(Props.Rock_3, new Vector3(100f, 0, 305f), 1),
                new PropReference(Props.Rock_0, new Vector3(-100f, 0, 235f), 1),
            }
        );
        
        public static readonly ScenaryReference Desert = new ScenaryReference(
            $"{ContentFolder.Models}/scenary/scenary",
            0.5f,
            Matrix.Identity,
            Color.DarkSalmon,
            new Vector3(-200f, 0f, -100f),
            new Vector3(200f, 50f, 100f),
            new List<PropReference>
            {
                new PropReference(Props.Building_House_0, new Vector3(0f, 10f, 0f), 5),
                new PropReference(Props.Building_House_0, new Vector3(20f, 10f, 20f), 5),
                new PropReference(Props.Building_House_0, new Vector3(30f, 10f, 30f), 5),
                new PropReference(Props.Building_House_0, new Vector3(40f, 10f, 100f), 5),
                new PropReference(Props.Building_House_0, new Vector3(70f, 10f, 100f), 5),
                new PropReference(Props.Building_House_0, new Vector3(-60f, 10f, -100f), 5),
                new PropReference(Props.Wall, new Vector3(100f, 10f, 0f), 5),
                new PropReference(Props.Wall, new Vector3(-100f, 10f, 0f), 5),
                new PropReference(Props.Rock_0, new Vector3(90f, 10f, 100f), 5),
                new PropReference(Props.Rock_0, new Vector3(-100f, 10f, 100f), 5),
                new PropReference(Props.Rock_0, new Vector3(110f, 10f, -100f), 5),
                new PropReference(Props.Rock_0, new Vector3(-120f, 10f, 100f), 5),
                new PropReference(Props.Rock_0, new Vector3(130f, 10f, -100f), 5),
                new PropReference(Props.Rock_0, new Vector3(-140f, 10f, 100f), 5),
                new PropReference(Props.Rock_0, new Vector3(150f, 10f, -100f), 5),
                new PropReference(Props.Rock_0, new Vector3(-160f, 10f, 100f), 5),
                new PropReference(Props.Farm, new Vector3(250f, 100f, 150f), 1),
                new PropReference(Props.Farm, new Vector3(-250f, 100f, -150f), 1),
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
