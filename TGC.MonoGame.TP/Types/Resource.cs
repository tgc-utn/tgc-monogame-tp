using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Types;

public abstract class Resource
{
    public Model Model;
    public Effect Effect;
    public Matrix World;
    public ModelReference Reference;
    
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
    
    public virtual void Draw(Matrix view, Matrix projection, Vector3 lightPosition, Vector3 lightViewProjection)
    {
        Model.Root.Transform = World;

        Effect.Parameters["View"]?.SetValue(view);
        Effect.Parameters["Projection"]?.SetValue(projection);

        // Draw the model.
        foreach (var mesh in Model.Meshes)
        {
            EffectsRepository.SetEffectParameters(Effect, Reference.DrawReference, mesh.Name);
            var worldMatrix = mesh.ParentBone.Transform * World;
            Effect.Parameters["World"].SetValue(worldMatrix);
            Effect.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(worldMatrix)));
            Effect.Parameters["WorldViewProjection"]?.SetValue(worldMatrix * view * projection);
            Effect.Parameters["lightPosition"]?.SetValue(lightPosition);
            Effect.Parameters["eyePosition"]?.SetValue(lightViewProjection);
            mesh.Draw();
        }
    }
}
