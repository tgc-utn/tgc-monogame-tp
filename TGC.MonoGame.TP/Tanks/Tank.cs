﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.References;

namespace TGC.MonoGame.TP.Tanks;

public class Tank : ICollidable
{
    private Model Model;
    private ModelReference Reference;
    private Effect Effect;
    private Matrix World;

    public Tank(ModelReference model, Vector3 position)
    {
        Reference = model;
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * Matrix.CreateTranslation(position);
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

        // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
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

    public void CollidedWithSmallProp()
    {
        Console.WriteLine("Chocaste con prop chico");
        // TODO frenar un poco el tanque
    }

    public void CollidedWithLargeProp()
    {
        Console.WriteLine("Chocaste con prop grande");
        // TODO frenar el tanque del todo
    }
}