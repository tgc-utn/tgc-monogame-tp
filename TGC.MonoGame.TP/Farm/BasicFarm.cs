using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Drawers;
using TGC.MonoGame.TP.Props.PropType;
using TGC.MonoGame.TP.References;
using Vector3 = System.Numerics.Vector3;

namespace TGC.MonoGame.TP.Farm;

public class BasicFarm : Prop
{
    protected ModelReference Reference;
   
    /*public Farm(ModelReference modelReference, Vector3 position)
    {
        Reference = modelReference;
        Position = position;

        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * Matrix.CreateTranslation(Position);
    }

    public void Load(ContentManager content, Effect effect)
    {
        Model = content.Load<Model>(Reference.Path);
        _effect = effect;
        foreach (ModelMesh farmModelMesh in Model.Meshes)
        {
            foreach (ModelMeshPart modelMeshPart in farmModelMesh.MeshParts)
            {
                modelMeshPart.Effect = _effect;
            }
        }*/
    public override void Update(GameTime gameTime)
    {
        return;
    }

    public override void Draw(Matrix view, Matrix projection)
    {
        base.Draw(view, projection);
        return;
    }

}