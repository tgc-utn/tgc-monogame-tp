
using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Common;
using WarSteel.Entities.Map;

namespace WarSteel.Scenes.Main;

public class MainScene : Scene
{
    public MainScene(GraphicsDeviceManager Graphics) : base(Graphics)
    {
    }

    public override void Initialize()
    {
        entities.Add(new Tank("player"));
        entities.Add(new Ground());
        camera = new Camera(new Vector3(0, 2500, 1), Graphics.GraphicsDevice.Viewport.AspectRatio, MathHelper.PiOver2, 0.1f, 3000f);

        base.Initialize();
    }

    public override void LoadContent()
    {
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        camera.Follow(entities.GetByName("player"));

        base.Update(gameTime);
    }
}