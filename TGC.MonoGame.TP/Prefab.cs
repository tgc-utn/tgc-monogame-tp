using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Collisions;

namespace TGC.MonoGame.TP;

public static class Prefab
{
    public static readonly List<Matrix> PlatformMatrices =  new();
    public static readonly List<Matrix> RampMatrices =  new();
    public static readonly List<BoundingBox> PlatformAbb =  new();
    public static readonly List<MovingPlatform> MovingPlatforms =  new();
    public static readonly List<OrientedBoundingBox> RampObb =  new();
    
    public static void CreateSquareCircuit(Vector3 offset)
    {
        // Platform
        // Side platforms
        CreatePlatform(new Vector3(50f, 6f, 200f), Vector3.Zero + offset);
        CreatePlatform(new Vector3(50f, 6f, 200f), new Vector3(300f, 0f, 0f) + offset);
        CreatePlatform(new Vector3(200f, 6f, 50f), new Vector3(150f, 0f, -200f) + offset);
        CreatePlatform(new Vector3(200f, 6f, 50f), new Vector3(150f, 0f, 200f) + offset);
            
        // Corner platforms
        CreatePlatform(new Vector3(50f, 6f, 80f), new Vector3(0f, 9.5f, -185f) + offset);
        CreatePlatform(new Vector3(50f, 6f, 80f), new Vector3(0f, 9.5f, 185f) + offset);
        CreatePlatform(new Vector3(50f, 6f, 80f), new Vector3(300f, 9.5f, -185f) + offset);
        CreatePlatform(new Vector3(50f, 6f, 80f), new Vector3(300f, 9.5f, 185f) + offset);
            
        // Center platform
        // La idea sería que se vaya moviendo 
        CreateMovingPlatform(new Vector3(50f, 6f, 100f), new Vector3(150f, 0f, 0f) + offset);
            
        CreateRamps(offset);
    }

    private static void CreateMovingPlatform(Vector3 scale, Vector3 position)
    {
        var platformWorld = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
        var platformBoundingBox = BoundingVolumesExtensions.FromMatrix(platformWorld);
        var movingPlatform = new MovingPlatform(platformWorld, scale, position, platformBoundingBox);
        MovingPlatforms.Add(movingPlatform);
    }

    public static void UpdateMovingPlatforms()
    {
        foreach (var movingPlatform in MovingPlatforms)
        {
            movingPlatform.Update();
        }
    }
    
    public static void CreateBridge()
    {
        // Platform
        CreatePlatform(new Vector3(90f, 6f, 30f), new Vector3(-50f, 0f, 0f));
        CreatePlatform(new Vector3(30f, 6f, 30f), new Vector3(-120f, 0f, 0f));
        CreatePlatform(new Vector3(30f, 6f, 30f), new Vector3(-160f, 0f, 0f));

        // Ramp
        CreateRamp(new Vector3(30f, 6f, 30f), new Vector3(-190f, 5f, 0f), 0f, -0.3f);
    }
    
    public static void CreateSwitchbackRamp()
    {
        const float heightIncrement = 29;
        var height = -24f;
        var rampSize = new Vector3(200f, 6f, 50f);
        var platformSize = new Vector3(50f, 6f, 100f);
        for (var floors = 0; floors < 6; floors++) {
            height += heightIncrement;
            CreateRamp(rampSize, new Vector3(-800f, height, 0f), 0f, -0.3f);
            height += heightIncrement;
            CreatePlatform(platformSize, new Vector3(-920f, height, 25f));
            height += heightIncrement;
            CreateRamp(rampSize, new Vector3(-800f, height, 50f), 0f, 0.3f);
            height += heightIncrement;
            CreatePlatform(platformSize, new Vector3(-680f, height, 25f));
        }

        CreateBridgeMaze(height);
        CreateMaze(height);
    }

    private static void CreateBridgeMaze(float height)
    {
        // Platform
        CreatePlatform(new Vector3(50f, 6f, 30f), new Vector3(-620f, height, 0f));
        CreatePlatform(new Vector3(50f, 6f, 25f), new Vector3(-560f, height, 0f));
        CreatePlatform(new Vector3(50f, 6f, 20f), new Vector3(-500f, height, 0f));
        CreatePlatform(new Vector3(50f, 6f, 15f), new Vector3(-440f, height, 0f));
            
        // Ramp
        CreateRamp(new Vector3(30f, 6f, 15f), new Vector3(-390f, height, 0f), 0f, 0.3f);
    }

    private static void CreateMaze(float height)
    {
        // Entrance platform
        CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(-300f, height, 0f));

        // Maze platform
        CreatePlatform(new Vector3(750f, 6f, 750f), new Vector3(100f, height, 0f));

        // Center platform to go next level, tendria que moverse hacia arriba hasta 900f
        CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(100f, height, 0f));

        // Border Walls
        CreatePlatform(new Vector3(750f, 50f, 6f), new Vector3(100f, height + 25f, 375f));
        CreatePlatform(new Vector3(750f, 50f, 6f), new Vector3(100f, height + 25f, -375f));
        CreatePlatform(new Vector3(6f, 50f, 750f), new Vector3(475f, height + 25f, 0f));
        CreatePlatform(new Vector3(6f, 50f, 350f), new Vector3(-275f, height + 25f, 200f));
        CreatePlatform(new Vector3(6f, 50f, 350f), new Vector3(-275f, height + 25f, -200f));

        // Vertical Walls from largest to shortest
        CreatePlatform(new Vector3(6f, 50f, 250f), new Vector3(225f, height + 25f, -50f));
        CreatePlatform(new Vector3(6f, 50f, 250f), new Vector3(275f, height + 25f, -200f));
        CreatePlatform(new Vector3(6f, 50f, 200f), new Vector3(-125f, height + 25f, 225f));
        CreatePlatform(new Vector3(6f, 50f, 200f), new Vector3(-75f, height + 25f, -75f));
        CreatePlatform(new Vector3(6f, 50f, 200f), new Vector3(-25f, height + 25f, 175f));
        CreatePlatform(new Vector3(6f, 50f, 200f), new Vector3(-25f, height + 25f, -125f));
        CreatePlatform(new Vector3(6f, 50f, 150f), new Vector3(-225f, height + 25f, 250f));
        CreatePlatform(new Vector3(6f, 50f, 150f), new Vector3(-225f, height + 25f, -100f));
        CreatePlatform(new Vector3(6f, 50f, 150f), new Vector3(25f, height + 25f, 0f));
        CreatePlatform(new Vector3(6f, 50f, 150f), new Vector3(275f, height + 25f, 250f));
        CreatePlatform(new Vector3(6f, 50f, 150f), new Vector3(275f, height + 25f, 50f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(-225f, height + 25f, 75f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(-175f, height + 25f, -25f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(-175f, height + 25f, -225f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(-125f, height + 25f, 25f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(-125f, height + 25f, -225f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(75f, height + 25f, 325f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(75f, height + 25f, 175f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(75f, height + 25f, 25f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(75f, height + 25f, -225f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(125f, height + 25f, -25f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(125f, height + 25f, -225f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(175f, height + 25f, 275f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(175f, height + 25f, 25f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(225f, height + 25f, -325f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(325f, height + 25f, -25f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(375f, height + 25f, 125f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(375f, height + 25f, -275f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(425f, height + 25f, 75f));
        CreatePlatform(new Vector3(6f, 50f, 100f), new Vector3(425f, height + 25f, -125f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-225f, height + 25f, -300f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-175f, height + 25f, 250f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-175f, height + 25f, 100f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-175f, height + 25f, -350f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-75f, height + 25f, 300f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-75f, height + 25f, 200f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-75f, height + 25f, -300f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-25f, height + 25f, 350f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(-25f, height + 25f, -300f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(25f, height + 25f, 150f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(25f, height + 25f, -150f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(25f, height + 25f, -300f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(75f, height + 25f, -350f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(125f, height + 25f, 100f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(175f, height + 25f, -150f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(175f, height + 25f, -300f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(225f, height + 25f, 150f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(325f, height + 25f, 250f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(325f, height + 25f, -150f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(375f, height + 25f, -100f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(425f, height + 25f, 200f));
        CreatePlatform(new Vector3(6f, 50f, 50f), new Vector3(425f, height + 25f, -250f));

        // Columns
        CreatePlatform(new Vector3(6f, 50f, 6f), new Vector3(125f, height + 25f, 225f));
        CreatePlatform(new Vector3(6f, 50f, 6f), new Vector3(125f, height + 25f, -325f));
        CreatePlatform(new Vector3(6f, 50f, 6f), new Vector3(325f, height + 25f, -275f));

        // Horizontal walls from largest to shortest
        CreatePlatform(new Vector3(200f, 50f, 6f), new Vector3(-125f, height + 25f, 125f));
        CreatePlatform(new Vector3(200f, 50f, 6f), new Vector3(225f, height + 25f, 125f));
        CreatePlatform(new Vector3(150f, 50f, 6f), new Vector3(200f, height + 25f, -225f));
        CreatePlatform(new Vector3(150f, 50f, 6f), new Vector3(100f, height + 25f, -125f));
        CreatePlatform(new Vector3(150f, 50f, 6f), new Vector3(-50f, height + 25f, 75f));
        CreatePlatform(new Vector3(150f, 50f, 6f), new Vector3(150f, height + 25f, 175f));
        CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(325f, height + 25f, -325f));
        CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(-125f, height + 25f, -275f));
        CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(-25f, height + 25f, -225f));
        CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(-125f, height + 25f, -125f));
        CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(325f, height + 25f, -125f));
        CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(125f, height + 25f, -75f));
        CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(375f, height + 25f, 25f));
        CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(-225f, height + 25f, 175f));
        CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(325f, height + 25f, 175f));
        CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(25f, height + 25f, 225f));
        CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(225f, height + 25f, 225f));
        CreatePlatform(new Vector3(100f, 50f, 6f), new Vector3(-25f, height + 25f, 275f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-100f, height + 25f, -325f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(450f, height + 25f, -325f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-250f, height + 25f, -275f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(50f, height + 25f, -275f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(200f, height + 25f, -275f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-200f, height + 25f, -225f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(350f, height + 25f, -225f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(450f, height + 25f, -225f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-200f, height + 25f, -175f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(100f, height + 25f, -175f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(200f, height + 25f, -175f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(400f, height + 25f, -175f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-150f, height + 25f, -75f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(0f, height + 25f, -75f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(400f, height + 25f, -75f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-100f, height + 25f, -25f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(100f, height + 25f, -25f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(350f, height + 25f, -25f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(450f, height + 25f, -25f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-200f, height + 25f, 25f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(0f, height + 25f, 25f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(150f, height + 25f, 25f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(100f, height + 25f, 75f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(200f, height + 25f, 75f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(350f, height + 25f, 75f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(50f, height + 25f, 125f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-50f, height + 25f, 175f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-200f, height + 25f, 225f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(400f, height + 25f, 225f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(100f, height + 25f, 275f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(250f, height + 25f, 275f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(350f, height + 25f, 275f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(450f, height + 25f, 275f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(-150f, height + 25f, 325f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(50f, height + 25f, 325f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(150f, height + 25f, 325f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(250f, height + 25f, 325f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(350f, height + 25f, 325f));
        CreatePlatform(new Vector3(50f, 50f, 6f), new Vector3(450f, height + 25f, 325f));
    }

    private static void CreateRamps(Vector3 offset)
    {
        // Ramp
        // Side ramps
        CreateRamp(new Vector3(50f, 6f, 50f), new Vector3(0f, 5f, -125f) + offset, 0.2f, 0f);
        CreateRamp(new Vector3(50f, 6f, 50f), new Vector3(300f, 5f, -125f) + offset, 0.2f, 0f);
        CreateRamp(new Vector3(50f, 6f, 50f), new Vector3(0f, 5f, 125f) + offset, -0.2f, 0f);
        CreateRamp(new Vector3(50f, 6f, 50f), new Vector3(300f, 5f, 125f) + offset, -0.2f, 0f);

        // Corner ramps
        CreateRamp(new Vector3(35f, 6f, 50f), new Vector3(40f, 5f, -200f) + offset, 0f, -0.3f);
        CreateRamp(new Vector3(35f, 6f, 50f), new Vector3(40f, 5f, 200f) + offset, 0f, -0.3f);
        CreateRamp(new Vector3(35f, 6f, 50f), new Vector3(260f, 5f, -200f) + offset, 0f, 0.3f);
        CreateRamp(new Vector3(35f, 6f, 50f), new Vector3(260f, 5f, 200f) + offset, 0f, 0.3f);

        CreateRamp(new Vector3(40f, 6f, 50f), new Vector3(45f, 5f, 0f) + offset, 0f, 0.3f);
        CreateRamp(new Vector3(40f, 6f, 50f), new Vector3(255f, 5f, 0f) + offset, 0f, -0.3f);
    }

    private static void CreateRamp(Vector3 scale, Vector3 position, float angleX, float angleZ)
    {
        var temporaryCubeAabb = BoundingVolumesExtensions.FromMatrix(Matrix.CreateScale(scale) * Matrix.CreateTranslation(position));
        var rampObb = OrientedBoundingBox.FromAABB(temporaryCubeAabb);
        rampObb.Rotate(Matrix.CreateRotationX(angleX) * Matrix.CreateRotationZ(angleZ));
        
        RampObb.Add(rampObb);

        var rampWorld = Matrix.CreateScale(scale) * Matrix.CreateRotationX(angleX) 
                                                  * Matrix.CreateRotationZ(angleZ) * Matrix.CreateTranslation(position);
        RampMatrices.Add(rampWorld);
    }
    
    private static void CreatePlatform(Vector3 scale, Vector3 position)
    {
        var platformWorld = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
        PlatformAbb.Add(BoundingVolumesExtensions.FromMatrix(platformWorld));
        PlatformMatrices.Add(platformWorld);
    }
}