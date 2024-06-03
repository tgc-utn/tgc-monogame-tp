using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Scenes;

namespace WarSteel.UIKit;

public class Button : IUIRenderable
{
    private Texture2D _texture;

    public Button(Texture2D texture)
    {
        _texture = texture;
    }

    public void Draw(Scene scene, UI ui)
    {
        Vector3 pos = ui.Position;
        scene.GetSpriteBatch().Draw(_texture, new Rectangle((int)pos.X, (int)pos.Y, (int)ui.Height, (int)ui.Width), Color.White);
    }
}