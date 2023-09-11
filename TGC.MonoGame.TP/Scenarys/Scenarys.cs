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
    private ScenaryReference Reference;
    private Effect Effect;
    private Matrix World;

    public Scenary(ScenaryReference model, Vector3 position)
    {
        Reference = model;
        World = Matrix.Identity * Matrix.CreateScale(Reference.Scale) * Reference.Rotation * Matrix.CreateTranslation(position);
    }
    
    public List<Vector3> GetSpawnPoints(int numberOfTanks, bool isAlies)
    {
        List<Vector3> spawnPoints = new List<Vector3>();
        float circleRadius = Math.Min(numberOfTanks * 3f, 20f);
        Vector3 position = isAlies ? Reference.AliesSpawn : Reference.EnemiesSpawn;
        for (int i = 0; i < numberOfTanks; i++)
        {
            float angle = MathHelper.TwoPi * i / numberOfTanks;
            Vector3 spawnPoint = position + circleRadius * new Vector3((float)Math.Cos(angle), 0f, (float)Math.Sin(angle));
            spawnPoints.Add(spawnPoint);
        }
        return spawnPoints;
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