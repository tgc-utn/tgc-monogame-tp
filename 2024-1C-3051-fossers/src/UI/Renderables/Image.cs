using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Managers;
using WarSteel.Scenes;

namespace WarSteel.UIKit;

public class Image : IUIRenderable
{
    private Texture2D _texture;
    private Color _color;
    private Vector2 _position;
    private Vector2 _origin;
    private float _scale;

    public Image(string texture, Color color, float scale = 1.0f)
    {
        _texture = ContentRepoManager.Instance().GetTexture(texture);
        _color = color;
        _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        _scale = scale;
    }

    public void Draw(Scene scene, UI ui)
    {
        // Calculate the scale factors required to fit the image inside the specified width and height
        float scaleX = ui.Width / _texture.Width;
        float scaleY = ui.Height / _texture.Height;

        // Use the smaller scale factor to ensure the image fits entirely inside the specified dimensions
        float scale = Math.Min(scaleX, scaleY) * _scale;

        // Calculate the position to center the image within the specified width and height
        Vector2 position = new Vector2(ui.Position.X, ui.Position.Y);

        scene.GetSpriteBatch().Draw(_texture, position, null, _color, 0f, _origin, scale, SpriteEffects.None, 0f);
    }
}