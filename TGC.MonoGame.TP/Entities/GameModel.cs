
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
    public void Draw(Vector3 pos) {
        foreach (var mesh in Model.Meshes)
        {
            World =  mesh.ParentBone.ModelTransform * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(pos);
            foreach (var meshPart in mesh.MeshParts) {
                meshPart.Effect.Parameters["DiffuseColor"].SetValue(Color.Red.ToVector3());
                meshPart.Effect.Parameters["World"].SetValue(World);
            }

            mesh.Draw();
        }
    }
}

