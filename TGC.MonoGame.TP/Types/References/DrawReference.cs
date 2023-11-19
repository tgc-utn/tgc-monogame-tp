using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Types.References;

public enum DrawType
{
    Color,
    Texture,
    BasicTexture,
    MultiTexture,
    Font
}

public abstract class DrawReference
{
    public DrawType Type { get; set; }
}

public class ColorReference : DrawReference
{
    public Color Color { get; }

    public ColorReference(Color color)
    {
        Type = DrawType.Color;
        Color = color;
    }
}

public class TextureReference : DrawReference
{
    public string Path { get; }
    
    public Texture2D Texture { get; set; }

    public TextureReference(string path)
    {
        Type = DrawType.Texture;
        Path = path;
    }
    
    public void SetTexture(Texture2D texture)
    {
        Texture = texture;
    }
}

public class BasicTextureReference : DrawReference
{
    public string Path { get; }
    
    public Texture2D Texture { get; set; }

    public BasicTextureReference(string path)
    {
        Type = DrawType.Texture;
        Path = path;
    }
    
    public void SetTexture(Texture2D texture)
    {
        Texture = texture;
    }
}

public class MeshTextureRelation
{
    public string Mesh { get; }
    public TextureReference Texture { get; }

    public MeshTextureRelation(string mesh, string texturePath)
    {
        Mesh = mesh;
        Texture = new TextureReference(texturePath);
    }
}

public class MultiTextureReference : DrawReference
{
    public List<MeshTextureRelation> Relations { get; }

    public MultiTextureReference(List<MeshTextureRelation> relations)
    {
        Type = DrawType.MultiTexture;
        Relations = relations;
    }
}

public class FontReference : DrawReference
{
    public string Path { get; }
    
    public SpriteFont Font { get; set; }
    
    public FontReference(string path)
    {
        Type = DrawType.Font;
        Path = path;
    }
}