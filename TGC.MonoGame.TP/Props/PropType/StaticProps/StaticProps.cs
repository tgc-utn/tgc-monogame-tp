using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.References;

namespace TGC.MonoGame.TP.Props.PropType.StaticProps;

public abstract class StaticProp
{
    public PropReference Reference;
    public Model Model;
    protected Effect Effect;
    public Matrix World;
    public BoundingBox Box;
    public Boolean Destroyed;

    public StaticProp(PropReference modelReference)
    {
        Reference = modelReference;
        World = Matrix.CreateScale(Reference.Prop.Scale) * Reference.Prop.Rotation *
                Matrix.CreateTranslation(Reference.Position);
    }
    
    public StaticProp(PropReference modelReference, Vector3 position)
    {
        Reference = modelReference;
        Reference.Position = position;
    }

    public void Load(ContentManager content, Effect effect)
    {
        World = Matrix.CreateScale(Reference.Prop.Scale) * Reference.Prop.Rotation *
                Matrix.CreateTranslation(Reference.Position);
        Model = content.Load<Model>(Reference.Prop.Path);
        Model.Root.Transform = World;
        Effect = effect;
        if (Reference.Prop.MeshIndex == -1)
            foreach (var modelMeshPart in Model.Meshes.SelectMany(modelMesh => modelMesh.MeshParts))
                modelMeshPart.Effect = Effect;
        else
            foreach (var modelMeshPart in Model.Meshes[Reference.Prop.MeshIndex].MeshParts)
                modelMeshPart.Effect = Effect;
        
        Box = BoundingVolumesExtension.CreateAABBFrom(Model);
        Box = new BoundingBox(Box.Min + World.Translation, Box.Max + World.Translation);
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

    public abstract void Update(ICollidable collidable);

    public abstract void CollidedWith(ICollidable other);
}