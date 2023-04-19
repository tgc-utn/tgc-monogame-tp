using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Entities.Islands;

public class Island
{ 
    private Model Model { get; set; }
    private Effect Effect { get; set; }
    private Matrix World { get; set; }

    public Island(Model model, Matrix matrix, Effect effect)
    {
        Model = model;
        Effect = effect;
        World = matrix;
    }
    
    public void Draw()
    {
        Effect.Parameters["DiffuseColor"].SetValue(Color.Yellow.ToVector3());
        var modelMeshesBaseTransforms = new Matrix[Model.Bones.Count];
        Model.CopyAbsoluteBoneTransformsTo(modelMeshesBaseTransforms);
        foreach (var mesh in Model.Meshes)
        {
            var relativeTransform = modelMeshesBaseTransforms[mesh.ParentBone.Index];
            Effect.Parameters["World"].SetValue(relativeTransform * World);
            mesh.Draw();
        }
    }
}