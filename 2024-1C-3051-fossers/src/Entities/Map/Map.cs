using Microsoft.Xna.Framework.Graphics;
using WarSteel.Common;
using WarSteel.Managers;

namespace WarSteel.Entities.Map;

public class Ground : Entity
{
    public Ground() : base("ground", null, new Transform()) { }

    public override void LoadContent()
    {
        Model model = ContentRepoManager.Instance().GetModel("Map/Ground/Ground");
        _renderable = new Renderable(model);

        base.LoadContent();
    }
}