using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.References;

namespace TGC.MonoGame.TP.Props.PropType;

public abstract class Prop
{
    private Model Model;
    private ModelReference Reference;
    private Effect Effect;
    private Matrix World;
    
    public void Load(ContentManager content, Effect effect)
    {
        Model = content.Load<Model>(Reference.Path);
        Effect = effect;
        foreach (var modelMeshPart in Model.Meshes.SelectMany(modelMesh => modelMesh.MeshParts))
        {
            modelMeshPart.Effect = Effect;
        }
    }
    
    public abstract void Update(GameTime gameTime);
    
    public virtual void Draw(Matrix view, Matrix projection)
    {
        Model.Root.Transform = World;

        // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
        Effect.Parameters["View"].SetValue(view);
        Effect.Parameters["Projection"].SetValue(projection);
        Effect.Parameters["DiffuseColor"].SetValue(Reference.Color.ToVector3());

        // Draw the model.
        foreach (var mesh in Model.Meshes)
        {
            Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * World);
            mesh.Draw();
        }
    }
}
