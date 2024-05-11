
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP;

public class GameModel 
{
    private Vector3 Position;
    private float Scale = 1f;
    private Matrix World;
    private Model Model;
    private Effect Effect;
    private List<List<Texture2D>> MeshPartTextures = new List<List<Texture2D>>();

    public GameModel(Vector3 pos, float scale) 
    {
        Position = pos;
        Scale = scale;
    }

    public GameModel(Vector3 pos)
    {
        Position = pos;
    }

    public void Load(Model model, Effect effect) {

        Effect = effect;
        Model = model;
        for (int mi = 0; mi < Model.Meshes.Count; mi++)
        {
            var mesh = Model.Meshes[mi];
            MeshPartTextures.Add(new List<Texture2D>());
            // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            for (int mpi = 0; mpi < mesh.MeshParts.Count; mpi++)
            {
                var meshPart = mesh.MeshParts[mpi];
                var texture = ((BasicEffect) meshPart.Effect).Texture;
                MeshPartTextures[mi].Add(texture);
                meshPart.Effect = Effect;
            }
        }

    }

    public void Draw() {
        for (int mi = 0; mi < Model.Meshes.Count; mi++)
        {
            var mesh = Model.Meshes[mi];
            World =  mesh.ParentBone.ModelTransform * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
            for (int mpi = 0; mpi < mesh.MeshParts.Count; mpi++)
            {
                var meshPart = mesh.MeshParts[mpi];
                var texture = MeshPartTextures[mi][mpi];
                // meshPart.Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                meshPart.Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["ModelTexture"].SetValue(texture);
            }

            mesh.Draw();
        }
    }

    public void Draw(Vector3 pos) {
        // var texture = ((BasicEffect) Model.Meshes.FirstOrDefault()?.MeshParts.FirstOrDefault()?.Effect)?.Texture;
        for (int mi = 0; mi < Model.Meshes.Count; mi++)
        {
            var mesh = Model.Meshes[mi];
            World =  mesh.ParentBone.ModelTransform * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(pos);
            for (int mpi = 0; mpi < mesh.MeshParts.Count; mpi++)
            {
                var meshPart = mesh.MeshParts[mpi];
                var texture = MeshPartTextures[mi][mpi];
                // meshPart.Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                meshPart.Effect.Parameters["World"].SetValue(World);
                Effect.Parameters["ModelTexture"].SetValue(texture);
            }

            mesh.Draw();
        }
    }
}

