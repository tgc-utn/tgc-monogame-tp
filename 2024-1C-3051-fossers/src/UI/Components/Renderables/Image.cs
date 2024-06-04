using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.UIKit;

public class Image : UIRenderable
{
    private Texture2D _texture;
    private Color _color;
    private Vector2 _origin;
    private float _scale;

    public Image(string texture, float scale = 1.0f)
    {
        _texture = ContentRepoManager.Instance().GetTexture(texture);
        _color = Color.White;
        _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        _scale = scale;
    }

    public Image(string texture, Color color, float scale = 1.0f)
    {
        _texture = ContentRepoManager.Instance().GetTexture(texture);
        _color = color;
        _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        _scale = scale;
    }

    public override void Draw(Scene scene, UI ui)
    {
        float scaleX = ui.Width / _texture.Width;
        float scaleY = ui.Height / _texture.Height;

        Vector2 position = new Vector2(ui.Position.X, ui.Position.Y);

        scene.SpriteBatch.Draw(_texture, position, null, _color, 0f, _origin, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
    }
}