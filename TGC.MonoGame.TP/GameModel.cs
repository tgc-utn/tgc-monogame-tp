
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Trees;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Samples.Collisions;
using static System.Formats.Asn1.AsnWriter;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace TGC.MonoGame.TP;

public class GameModel : BaseModel
{
    public GameModel(Model model, Effect effect, float scale, Vector3 pos)
        : base(model, effect, scale, pos)
    {
        // Constructor sin lógica adicional
    }

    public GameModel(Model model, Effect effect, float scale, List<Vector3> listPos)
        : base(model, effect, scale, listPos)
    {
        // Constructor sin lógica adicional
    }

    public GameModel(Model model, Effect effect, float scale, Vector3 pos, Simulation simulation, Sphere sphere)
        : base(model, effect, scale, pos)
    {
        AddToSimulation(simulation, sphere);
    }
    
    public GameModel(Model model, Effect effect, float scale, List<Vector3> listPos, Simulation simulation, Sphere sphere)
        : base(model, effect, scale, listPos)
    {
        AddToSimulation(simulation, sphere, listPos);
    }

    public GameModel(Model model, Effect effect, float scale, Vector3 pos, Simulation simulation, Box box)
       : base(model, effect, scale, pos)
    {
        AddToSimulation(simulation, box);
    }

    public GameModel(Model model, Effect effect, float scale, List<Vector3> listPos, Simulation simulation, Box box)
       : base(model, effect, scale, listPos)
    {
        AddToSimulation(simulation, box, listPos);
    }

    public GameModel(Model model, Effect effect, float scale, Vector3 pos, Simulation simulation, ConvexHull convexHull)
       : base(model, effect, scale, pos)
    {
        AddToSimulation(simulation, convexHull);
    }
    
    public GameModel(Model model, Effect effect, float scale, List<Vector3> listPos, Simulation simulation, ConvexHull convexHull)
     : base(model, effect, scale, listPos)
    {
        AddToSimulation(simulation, convexHull , listPos);
    }

    private void AddToSimulation(Simulation simulation, Sphere sphere)
    {
        simulation.Statics.Add(new StaticDescription(
            new System.Numerics.Vector3(Position.X, Position.Y, Position.Z),
            simulation.Shapes.Add(sphere)
        ));
    }
    
    private void AddToSimulation(Simulation simulation, Sphere sphere, List<Vector3> listPos)
    {
        foreach (var Position in listPos)
            simulation.Statics.Add(new StaticDescription(
                new System.Numerics.Vector3(Position.X, Position.Y, Position.Z),
                simulation.Shapes.Add(sphere)
            ));
    }
    
    private void AddToSimulation(Simulation simulation, Box box)
    {
        simulation.Statics.Add(new StaticDescription(
            new System.Numerics.Vector3(Position.X, Position.Y, Position.Z),
            simulation.Shapes.Add(box)
        ));
    }
    
    private void AddToSimulation(Simulation simulation, Box box, List<Vector3> listPos)
    {
        foreach (var pos in listPos)
            simulation.Statics.Add(new StaticDescription(
                new System.Numerics.Vector3(pos.X, pos.Y, pos.Z),
                simulation.Shapes.Add(box)
            ));
    }
    
    private void AddToSimulation(Simulation simulation, ConvexHull convexHull)
    {
        simulation.Statics.Add(new StaticDescription(
            new System.Numerics.Vector3(Position.X, Position.Y, Position.Z),
            simulation.Shapes.Add(convexHull)
        ));
    }
    
    private void AddToSimulation(Simulation simulation, ConvexHull convexHull , List<Vector3> listPos)
    {
        foreach (var Position in listPos)
        simulation.Statics.Add(new StaticDescription(
            new System.Numerics.Vector3(Position.X + 2f, Position.Y, Position.Z ),
            simulation.Shapes.Add(convexHull)
        ));
    }

    public void Draw(List<Matrix> Worlds)
    {
        Matrix world;
        //var texture = ((BasicEffect) Model.Meshes.FirstOrDefault()?.MeshParts.FirstOrDefault()?.Effect)?.Texture;
        foreach (Matrix World in Worlds)
        {
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
                    meshPart.Effect.Parameters["World"]?.SetValue(world);
                    Effect.Parameters["ModelTexture"]?.SetValue(texture);
                }

                mesh.Draw();
            }
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


