using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Types.References;

public enum DrawType
{
    Color,
    Texture,
    BasicTexture,
    ShadowTexture,
    ShadowBlingPhong,
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

public class ShadowBlingPhongReference : DrawReference
{
    public string Path { get; }
    
    public Texture2D Texture { get; set; }

    public ShadowBlingPhongReference(string path)
    {
        Type = DrawType.ShadowBlingPhong;
        Path = path;
    }
    
    public void SetTexture(Texture2D texture)
    {
        Texture = texture;
    }
}
public class ShadowTextureReference : DrawReference
{
    public string Path { get; }
    
    public Texture2D Texture { get; set; }

    public ShadowTextureReference(string path)
    {
        Type = DrawType.ShadowTexture;
        Path = path;
    }
    
    public void SetTexture(Texture2D texture)
    {
        Texture = texture;
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
        Type = DrawType.BasicTexture;
        Path = path;
    }
    
    public void SetTexture(Texture2D texture)
    {
        Texture = texture;
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