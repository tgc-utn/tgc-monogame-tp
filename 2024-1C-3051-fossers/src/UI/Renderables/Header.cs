using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.UIKit;

public class Header : IUIRenderable
{
    private string _text;
    private Color _color;
    private SpriteFont _font;

    public Header(string text, Color color)
    {
        _text = text;
        _color = color;
        _font = ContentRepoManager.Instance().GetSpriteFont("tenada/Tenada");
    }

    public void Draw(Scene scene, UI ui)
    {
        Vector3 center = ui.Position;
        float fontSize = 1f;
        Vector2 textSize = _font.MeasureString(_text) * fontSize;
        Vector2 textPosition = new Vector2(center.X - textSize.X / 2, center.Y - textSize.Y / 2);
        scene.GetSpriteBatch().DrawString(_font, _text, textPosition, _color, 0f, Vector2.Zero, fontSize, SpriteEffects.None, 0f);
    }
}