using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Types;

public class Scenary : Resource
{
    public ScenaryReference Scene { get; }

    public Scenary(ScenaryReference model, Vector3 position)
    {
        Scene = model;
        Reference = model.Scenary;
        World = Matrix.CreateScale(Reference.Scale,1,Reference.Scale) * Reference.Rotation * Matrix.CreateTranslation(position);
    }
    
    public List<Vector3> GetSpawnPoints(int numberOfTanks, bool isAlies)
    {
        return GetCircularPoints(numberOfTanks, isAlies ? Scene.AliesSpawn : Scene.EnemiesSpawn);
    }
    
    public List<Vector3> GetCircularPoints(int numberObjs, Vector3 centerPosition, float circleRadius = 20f)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < numberObjs; i++)
        {
            float angle = MathHelper.TwoPi * i / numberObjs;
            Vector3 point = centerPosition + circleRadius * new Vector3((float)Math.Cos(angle), 0f, (float)Math.Sin(angle));
            points.Add(point);
        }
        return points;
    }
    
}