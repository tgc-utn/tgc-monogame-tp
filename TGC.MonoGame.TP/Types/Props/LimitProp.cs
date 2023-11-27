using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Types.Props;

public class LimitProp: Resource
{  
    private PropReference Prop;
    public Vector3 Position;
    public Matrix Translation { get; set; }
    public float Angle { get; set; } = 0f;
    
    public LimitProp(PropReference modelReference)
    {
        Reference = modelReference.Prop;
        Prop = modelReference;
        Position = Prop.Position;
    }
    
    public LimitProp(PropReference modelReference, Vector3 position)
    {
        Reference = modelReference.Prop;
        Prop = modelReference;
        Position = position;
    }
    
    public override void Load(ContentManager content)
    {
        base.Load(content);
        
        Translation = Matrix.CreateTranslation(Position);
        World = Matrix.CreateScale(Reference.Scale) * Reference.Rotation * Matrix.CreateRotationY(Angle) * Translation;
        Model.Root.Transform = World;
    }
}