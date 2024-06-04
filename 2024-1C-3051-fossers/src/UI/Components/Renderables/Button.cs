using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.UIKit;

public class Button : UIRenderable
{
    private Texture2D _texture;
    public string Text;
    private SpriteFont _font;
    private float _fontSize;

    public Button(Texture2D texture, float fontSize, string text)
    {
        _texture = texture;
        Text = text;
        _fontSize = fontSize;
        _font = ContentRepoManager.Instance().GetSpriteFont("tenada/Tenada");
        SoundEffect = ContentRepoManager.Instance().GetSoundEffect("button-click");
    }

    public void SetText() { }

    public override void Draw(Scene scene, UI ui)
    {
        Vector3 center = ui.Position;
        Vector2 buttonSize = new Vector2(ui.Width, ui.Height);
        Vector2 buttonPosition = new Vector2(center.X - buttonSize.X / 2, center.Y - buttonSize.Y / 2);
        scene.SpriteBatch.Draw(_texture, new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, (int)buttonSize.X, (int)buttonSize.Y), Color.White);

        float fontSize = 0.5f;
        Vector2 textSize = _font.MeasureString(Text) * _fontSize;
        Vector2 textPosition = new Vector2(center.X - textSize.X / 2, center.Y - textSize.Y / 2);
        scene.SpriteBatch.DrawString(_font, Text, textPosition, Color.White, 0f, Vector2.Zero, fontSize, SpriteEffects.None, 0f);
    }
}