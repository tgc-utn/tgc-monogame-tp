using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Types.Props;

public class StaticProp
{
    private PropReference Reference;
    private Model Model;
    private Effect Effect;
    private Matrix World;

    public StaticProp(PropReference modelReference)
    {
        Reference = modelReference;
        World = Matrix.CreateScale(Reference.Prop.Scale) * Reference.Prop.Rotation *
                Matrix.CreateTranslation(Reference.Position);
    }
    
    public StaticProp(PropReference modelReference, Vector3 position)
    {
        Reference = modelReference;
        World = Matrix.CreateScale(Reference.Prop.Scale) * Reference.Prop.Rotation *
                Matrix.CreateTranslation(position);
    }

    public void Load(ContentManager content, Effect effect)
    {
        Model = content.Load<Model>(Reference.Prop.Path);
        Effect = effect;
        foreach (var modelMeshPart in Model.Meshes.SelectMany(modelMesh => modelMesh.MeshParts))
            modelMeshPart.Effect = Effect;
    }

    public void Draw(Matrix view, Matrix projection)
    {
        Model.Root.Transform = World;

        // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
        Effect.Parameters["View"].SetValue(view);
        Effect.Parameters["Projection"].SetValue(projection);
        Effect.Parameters["DiffuseColor"].SetValue(Reference.Prop.Color.ToVector3());

        // Draw the model.
        foreach (var mesh in Model.Meshes)
        {
            Effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * World);
            mesh.Draw();
        }
    }

    public void Update(GameTime gameTime)
    {
        // Destruir prop
        return;
    }
}