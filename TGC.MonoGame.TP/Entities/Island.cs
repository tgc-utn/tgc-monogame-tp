using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Camera;

namespace TGC.MonoGame.TP.Entities;

public class Island
{
    public const string ContentFolder3D = "Models/";
    public const string ContentFolderEffects = "Effects/";
    private Model Model { get; set; }
    private Effect Effect { get; set; }
    private Matrix World { get; set; }

    public Island(Matrix world)
    {
        World = world;
    }
    
    public void LoadContent(ContentManager content, string modelPath)
    {
        Model = content.Load<Model>(ContentFolder3D + modelPath);
        Effect = content.Load<Effect>(ContentFolderEffects + "BasicShader");

        foreach (var mesh in Model.Meshes)
        {
            // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            foreach (var meshPart in mesh.MeshParts)
            {
                meshPart.Effect = Effect;
            }
        }
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