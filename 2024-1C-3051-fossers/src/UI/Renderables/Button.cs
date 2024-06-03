using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.UIKit;

public class Button : IUIRenderable
{
    private Texture2D _texture;
    private string _text;
    private SpriteFont _font;

    public Button(Texture2D texture, string text)
    {
        _texture = texture;
        _text = text;
        _font = ContentRepoManager.Instance().GetSpriteFont("tenada/Tenada");
    }

    public void Draw(Scene scene, UI ui)
    {
        Vector3 center = ui.Position;
        Vector2 buttonSize = new Vector2(ui.Width, ui.Height);
        Vector2 buttonPosition = new Vector2(center.X - buttonSize.X / 2, center.Y - buttonSize.Y / 2);
        scene.GetSpriteBatch().Draw(_texture, new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, (int)buttonSize.X, (int)buttonSize.Y), Color.White);

        float fontSize = 0.5f;
        Vector2 textSize = _font.MeasureString(_text) * fontSize;
        Vector2 textPosition = new Vector2(center.X - textSize.X / 2, center.Y - textSize.Y / 2);
        scene.GetSpriteBatch().DrawString(_font, _text, textPosition, Color.White, 0f, Vector2.Zero, fontSize, SpriteEffects.None, 0f);
    }
}