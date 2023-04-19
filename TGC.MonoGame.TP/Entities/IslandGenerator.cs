using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Entities;

public class IslandGenerator
{
    public const string ContentFolder3D = "Models/";
    private IList<Model> IslandsModel { get; set; } = new List<Model>();
    private Effect Effect { get; set; }
    public void LoadContent(ContentManager content, Effect effect)
    {
        Effect = effect;
        string[] islandPaths = { "Island1/Island1", "Island2/Island2", "Island3/Island3" };
        for (var i = 0; i < islandPaths.Length; i++)
        {
            IslandsModel.Add(content.Load<Model>(ContentFolder3D + islandPaths[i]));
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

    public Island Create(int modelNumber, Vector3 translation, float scale, float rotation = 0)
    {
        Matrix world = Matrix.CreateScale(scale) * Matrix.CreateRotationY(rotation) *  Matrix.CreateTranslation(translation);
        return new Island(IslandsModel[modelNumber], world, Effect);
    }
}