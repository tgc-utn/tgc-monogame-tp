﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP;

public static class Prefab
{
    public static readonly List<Matrix> PlatformMatrices =  new List<Matrix>();
    
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
        CreatePlatform(new Vector3(50f, 6f, 100f), new Vector3(150f, 0f, 0f) + offset);
            
        // Ramp
        // Side ramps
        CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(0f, 5f, -125f) + offset, Matrix.CreateRotationX(0.2f));
        CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(300f, 5f, -125f) + offset, Matrix.CreateRotationX(0.2f));
        CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(0f, 5f, 125f) + offset, Matrix.CreateRotationX(-0.2f));
        CreatePlatform(new Vector3(50f, 6f, 50f), new Vector3(300f, 5f, 125f) + offset, Matrix.CreateRotationX(-0.2f));
            
        // Corner ramps
        CreatePlatform(new Vector3(35f, 6f, 50f), new Vector3(40f, 5f, -200f) + offset, Matrix.CreateRotationZ(-0.3f));
        CreatePlatform(new Vector3(35f, 6f, 50f), new Vector3(40f, 5f, 200f) + offset, Matrix.CreateRotationZ(-0.3f));
        CreatePlatform(new Vector3(35f, 6f, 50f), new Vector3(260f, 5f, -200f) + offset, Matrix.CreateRotationZ(0.3f));
        CreatePlatform(new Vector3(35f, 6f, 50f), new Vector3(260f, 5f, 200f) + offset, Matrix.CreateRotationZ(0.3f));
            
        CreatePlatform(new Vector3(40f, 6f, 50f), new Vector3(45f, 5f, 0f) + offset, Matrix.CreateRotationZ(0.3f));
        CreatePlatform(new Vector3(40f, 6f, 50f), new Vector3(255f, 5f, 0f) + offset, Matrix.CreateRotationZ(-0.3f));
    }
    
    private static void CreatePlatform(Vector3 scale, Vector3 position, Matrix rotation)
    {
        var platformWorld = Matrix.CreateScale(scale) * rotation * Matrix.CreateTranslation(position);
        PlatformMatrices.Add(platformWorld);
    }
    
    private static void CreatePlatform(Vector3 scale, Vector3 position)
    {
        var platformWorld = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
        PlatformMatrices.Add(platformWorld);
    }
}