namespace TGC.MonoGame.TP.Types;

public enum FunctionType { Linear, Sinusoidal, Circular, Unique } 

public abstract class Function
{
    public float StartX { get; set; } = 0;
    public float EndX { get; set; } = 0;
    public float Y { get; set; }
    public new abstract FunctionType GetType();
}

public class FunctionLinear : Function
{
    public float StartZ { get; set; }
    public float EndZ { get; set; }
    public override FunctionType GetType() { return FunctionType.Linear; }
}

public class FunctionUnique : Function
{
    public override FunctionType GetType() { return FunctionType.Unique; }
}

public class FunctionCircular : Function
{
    public override FunctionType GetType() { return FunctionType.Circular; }
    public float CenterX { get; set; }
    public float CenterZ { get; set; }
    public float Radius { get; set; }
}

public class FunctionSinusoidal : Function
{
    public override FunctionType GetType() { return FunctionType.Sinusoidal; }
    public float StartZ { get; set; }
    public float EndZ { get; set; }
    public float Amplitude { get; set; }
    public float Periods { get; set; }
    public bool UseX { get; set; }
}