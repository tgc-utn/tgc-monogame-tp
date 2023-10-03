using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Types.References;

public class ModelReference
{
    public string Path { get; }
    public float Scale { get; }
    public Vector2 BBScale { get; }
    public Matrix Rotation { get; }
    public DrawReference DrawReference { get; }

    public ModelReference(string model, float scale, Matrix normal, DrawReference drawReference)
    {
        Path = model;
        Scale = scale;
        BBScale = new Vector2(scale, scale);
        Rotation = normal;
        DrawReference = drawReference;
    }
    public ModelReference(string model, float scale, Vector2 bbScale, Matrix normal, DrawReference drawReference)
    {
        Path = model;
        Scale = scale;
        BBScale = bbScale;
        Rotation = normal;
        DrawReference = drawReference;
    }
}