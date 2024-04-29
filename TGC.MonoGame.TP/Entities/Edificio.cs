using System;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using BepuPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Geometries;

namespace TGC.MonoGame.TP;

public class Edificio 
{
    private Vector3 Position;
    private float Scale = 1f;
    private Matrix World;
    private Model Model;
    private Effect Effect;

    public Edificio(Vector3 pos, float scale) 
    {
        Position = pos;
        Scale = scale;
    }

    public Edificio(Vector3 pos)
    {
        Position = pos;
    }

    public void Load(Model model, Effect effect) {

        Effect = effect;
        Model = model;

        foreach (var mesh in model.Meshes)
        {
            // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            foreach (var meshPart in mesh.MeshParts)
            {
                meshPart.Effect = Effect;
            }
        }

    }

    public void Draw() {
        foreach (var mesh in Model.Meshes)
        {
            World =  mesh.ParentBone.ModelTransform * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
            foreach (var meshPart in mesh.MeshParts) {
                meshPart.Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                meshPart.Effect.Parameters["World"].SetValue(World);
            }

            mesh.Draw();
        }
    }
}

