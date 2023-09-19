using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.References;

namespace TGC.MonoGame.TP.Scenarys;

public class Scenary
{
    private Model Model;
    public ScenaryReference Reference { get; }
    private Effect Effect;
    private Matrix World;

    public Scenary(ScenaryReference model, Vector3 position)
    {
        Reference = model;
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * Matrix.CreateTranslation(position);
    }
    
    public List<Vector3> GetSpawnPoints(int numberOfTanks, bool isAlies)
    {
        return GetCircularPoints(numberOfTanks, isAlies ? Reference.AliesSpawn : Reference.EnemiesSpawn);
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

    public void Load(ContentManager content, Effect effect)
    {
        Model = content.Load<Model>(Reference.Path);
        Effect = effect;
        foreach (var modelMeshPart in Model.Meshes.SelectMany(tankModelMesh => tankModelMesh.MeshParts))
        {
            modelMeshPart.Effect = Effect;
        }
    }
    
    public void Draw(Matrix view, Matrix projection)
    {
        Model.Root.Transform = World;

        Effect.Parameters["View"].SetValue(view);
        Effect.Parameters["Projection"].SetValue(projection);
        Effect.Parameters["DiffuseColor"].SetValue(Reference.Color.ToVector3());

        // Draw the model.
        foreach (var mesh in Model.Meshes)
        {
            Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * World);
            mesh.Draw();
        }
    }
}