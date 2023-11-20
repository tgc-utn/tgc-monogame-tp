using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Types.References;

public enum PropType { Small, Large, Dome};

public class PropReference
{
    public ModelReference Prop { get; }
    public Vector3 Position { get; }
    public PropType PropType { get; }
    public Repetition Repetitions { get; } = new Repetition(1, new FunctionUnique());

    public PropReference(ModelReference prop, Vector3 position, PropType propType, Repetition repetitions = null)
    {
        Prop = prop;
        Position = position;
        PropType = propType;
        Repetitions = repetitions ?? new Repetition();
    }
}