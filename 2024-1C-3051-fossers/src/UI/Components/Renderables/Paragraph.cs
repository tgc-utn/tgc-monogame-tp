using Microsoft.Xna.Framework;
using WarSteel.UIKit;

public class Paragraph : TextUI
{
    public Paragraph(string text) : base(text, 0.4f, Color.White)
    {
    }

    public Paragraph(string text, Color color) : base(text, 1f, color)
    {
    }

}