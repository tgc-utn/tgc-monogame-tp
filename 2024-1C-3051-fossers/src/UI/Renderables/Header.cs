using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.UIKit;

public class Header : IUIRenderable
{
    private Texture2D _texture;
    private string _text;
    private Color _color;
    private SpriteFont _font;

    public Header(string text, Color color)
    {
        _text = text;
        _color = color;
        _font = ContentRepoManager.Instance().GetSpriteFont("Tenada");
    }

    public void Draw(Scene scene, UI ui)
    {
        Vector3 pos = ui.Position;
        scene.GetSpriteBatch().DrawString(_font, _text, new Vector2(pos.X, pos.Y), _color);
    }
}