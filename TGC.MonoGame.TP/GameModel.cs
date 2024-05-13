
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Samples.Collisions;

namespace TGC.MonoGame.TP;

public class GameModel 
{
    public Model Model;
    public Effect Effect;
    private float Scale = 1f;
    public List<List<Texture2D>> MeshPartTextures = new List<List<Texture2D>>();

    public GameModel(Model model, Effect effect, float scale) {

        Effect = effect;
        Model = model;
        Scale = scale;

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

    public GameModel(Model model, Effect effect) : this(model, effect, 1f) {
    }

    public void Draw(Matrix World) {
        // var texture = ((BasicEffect) Model.Meshes.FirstOrDefault()?.MeshParts.FirstOrDefault()?.Effect)?.Texture;
        Matrix world;
        for (int mi = 0; mi < Model.Meshes.Count; mi++)
        {
            var mesh = Model.Meshes[mi];
            world = mesh.ParentBone.ModelTransform * Matrix.CreateScale(Scale) * World;
            for (int mpi = 0; mpi < mesh.MeshParts.Count; mpi++)
            {
                var meshPart = mesh.MeshParts[mpi];
                var texture = MeshPartTextures[mi][mpi];
                meshPart.Effect.Parameters["World"].SetValue(world);
                Effect.Parameters["ModelTexture"].SetValue(texture);
            }

            mesh.Draw();
        }
    }
}

