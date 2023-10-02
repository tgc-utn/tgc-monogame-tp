using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Types.References;

public class ModelReference
{
    public string Path { get; }
    public float Scale { get; }
    public Matrix Rotation { get; }
    public DrawReference DrawReference { get; }

    public ModelReference(string model, float scale, Matrix normal, DrawReference drawReference)
    {
        Path = model;
        Scale = scale;
        Rotation = normal;
        DrawReference = drawReference;
    }
}