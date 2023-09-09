using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Drawers;

public interface IDrawer
{
    void Draw(Model model, Matrix world);
}