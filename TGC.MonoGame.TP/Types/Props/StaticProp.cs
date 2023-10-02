using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Types.Props;

public class StaticProp : Resource
{
    private PropReference Prop;

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

    public void Update(GameTime gameTime)
    {
        // Destruir prop
        return;
    }
}