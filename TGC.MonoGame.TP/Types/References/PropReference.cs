using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Types.References;

public class PropReference
{
    public ModelReference Prop { get; }
    public Vector3 Position { get; }
    public Repetition Repetitions { get; } = new Repetition(1, new FunctionUnique());

    public PropReference(ModelReference prop, Vector3 position, Repetition repetitions = null)
    {
        Prop = prop;
        Position = position;
        Repetitions = repetitions ?? new Repetition();
    }
}