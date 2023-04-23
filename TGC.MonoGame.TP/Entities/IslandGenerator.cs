using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TGC.MonoGame.TP.Entities.Islands;

namespace TGC.MonoGame.TP.Entities;

public class IslandGenerator
{
    private const string ContentFolder3D = "Models/";
    private IList<Model> IslandsModel { get; set; } = new List<Model>();
    private Effect Effect { get; set; }
    
    string[] _islandPaths = { "Island1/Island1", "Island2/Island2", "Island3/Island3" };

    public void LoadContent(ContentManager content, Effect effect)
    {
        Effect = effect;
        for (var i = 0; i < _islandPaths.Length; i++)
        {
            IslandsModel.Add(content.Load<Model>(ContentFolder3D + _islandPaths[i]));
            foreach (var mesh in IslandsModel[i].Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
        }
    }

    private Island Create(int modelNumber, Vector3 translation, float scale, float rotation = 0)
    {
        Matrix world = Matrix.CreateScale(scale) * Matrix.CreateRotationY(rotation) *  Matrix.CreateTranslation(translation);
        return new Island(IslandsModel[modelNumber], world, Effect);
    }

    public Island[] CreateRandomIslands(int qty, float maxX, float maxZ)
    {
        Island[] islands = new Island[qty];

        Random rnd = new Random();
        for (int i = 0; i < qty; i++)
        {
            float islandX = rnd.NextSingle() * maxX;
            float islandZ = rnd.NextSingle() * maxZ;
            
            Console.WriteLine("[Creating Island] X: " + islandX + " - Z: " + islandZ);

            islands[i] = Create(rnd.Next(_islandPaths.Length), new Vector3(islandX, 0, islandZ), 0.2f);
        }

        return islands;
    }
}