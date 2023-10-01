using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.Types;

public class Repetition
{
    public int Repetitions { get; }
    public Function FunctionRef { get; }

    public Repetition(int repetitions = 1, Function functionType = null)
    {
        Repetitions = repetitions;
        FunctionRef = functionType ?? new FunctionUnique();
    }
}