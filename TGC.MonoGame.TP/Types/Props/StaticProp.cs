using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Helpers.Collisions;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Types.Props;

public abstract class StaticProp : Resource
{
    private PropReference Prop;
    
    public BoundingBox Box;
    public bool Destroyed = false;

    public StaticProp(PropReference modelReference)
    {
        Reference = modelReference.Prop;
        Prop = modelReference;
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation *
                Matrix.CreateTranslation(Prop.Position);
    }
    
    public StaticProp(PropReference modelReference, Vector3 position)
    {
        Reference = modelReference.Prop;
        Prop = modelReference;
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation *
                Matrix.CreateTranslation(position);
    }

    public override void Load(ContentManager content)
    {
        base.Load(content);
        Model.Root.Transform = World;
        Box = BoundingVolumesExtension.CreateAABBFrom(Model);
        Box = new BoundingBox(Box.Min * Reference.Scale + World.Translation, Box.Max * Reference.Scale + World.Translation);
    }
    
    public abstract void Update(ICollidable collidable);
    public abstract void CollidedWith(ICollidable other);
}