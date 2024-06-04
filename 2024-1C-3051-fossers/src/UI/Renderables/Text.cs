using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.UIKit;

public class TextUI : IUIRenderable
{
    public string Text;
    private Color _color;
    private SpriteFont _font;
    private float _fontSize;

    public TextUI(string text, float fontSize, Color color)
    {
        Text = text;
        _color = color;
        _font = ContentRepoManager.Instance().GetSpriteFont("tenada/Tenada");
        _fontSize = fontSize;
    }

    public void Draw(Scene scene, UI ui)
    {
        Vector3 center = ui.Position;
        Vector2 textSize = _font.MeasureString(Text) * _fontSize;
        Vector2 textPosition = new Vector2(center.X - textSize.X / 2, center.Y - textSize.Y / 2);
        scene.GetSpriteBatch().DrawString(_font, Text, textPosition, _color, 0f, Vector2.Zero, _fontSize, SpriteEffects.None, 0f);
    }
}