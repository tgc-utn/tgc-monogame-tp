
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
            // Verifica si la matriz de transformación del hueso está compuesta completamente de ceros
            if (!MatrixHelper.IsZeroMatrix(mesh.ParentBone.ModelTransform))
            {
                // Aplica la transformación del hueso al mundo
                world = mesh.ParentBone.ModelTransform * Matrix.CreateScale(Scale) * World;
            }
            else
            {
                // Si la matriz del hueso está compuesta de ceros, utiliza solo la escala y la matriz del mundo
                world = Matrix.CreateScale(Scale) * World;
            }
            for (int mpi = 0; mpi < mesh.MeshParts.Count; mpi++)
            {
                var meshPart = mesh.MeshParts[mpi];
                var texture = MeshPartTextures[mi][mpi];
                meshPart.Effect.Parameters["World"].SetValue(world);
                Effect.Parameters["ModelTexture"]?.SetValue(texture);
            }

            mesh.Draw();
        }
    }
}

public class MatrixHelper
{
    // Función para verificar si una matriz 4x4 está compuesta completamente de ceros
    public static bool IsZeroMatrix(Matrix matrix)
    {
        // Comprueba si todos los elementos de la matriz son cero
        if (matrix.M11 == 0 && matrix.M12 == 0 && matrix.M13 == 0 && matrix.M14 == 0 &&
            matrix.M21 == 0 && matrix.M22 == 0 && matrix.M23 == 0 && matrix.M24 == 0 &&
            matrix.M31 == 0 && matrix.M32 == 0 && matrix.M33 == 0 && matrix.M34 == 0 &&
            matrix.M41 == 0 && matrix.M42 == 0 && matrix.M43 == 0 && matrix.M44 == 0)
        {
            return true;
        }
        return false;
    }
}

