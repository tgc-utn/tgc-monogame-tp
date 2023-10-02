using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Types;

public abstract class Resource
{
    protected Model Model;
    protected Effect Effect;
    public Matrix World;
    protected ModelReference Reference;
    
    public virtual void Load(ContentManager content)
    {
        Model = content.Load<Model>(Reference.Path);
        Effect = EffectsRepository.GetEffect(Reference.DrawReference, content);
        TexturesRepository.InitializeTextures(Reference.DrawReference, content);
        foreach (var modelMeshPart in Model.Meshes.SelectMany(tankModelMesh => tankModelMesh.MeshParts))
        {
            modelMeshPart.Effect = Effect;
        }
    }
    
    public virtual void Draw(Matrix view, Matrix projection)
    {
        Model.Root.Transform = World;

        Effect.Parameters["View"].SetValue(view);
        Effect.Parameters["Projection"].SetValue(projection);

        // Draw the model.
        foreach (var mesh in Model.Meshes)
        {
            EffectsRepository.SetEffectParameters(Effect, Reference.DrawReference, mesh.Name);
            Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * World);
            mesh.Draw();
        }
    }
}
