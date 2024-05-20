
using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Common;
using WarSteel.Scenes.SceneProcessors;


namespace WarSteel.Scenes.Main;

public class MainScene : Scene
{
    public MainScene(GraphicsDeviceManager Graphics) : base(Graphics)
    {

    }

    public override void Initialize()
    {
        LightProcessor light = new LightProcessor(Color.AliceBlue);

        light.AddLightSource(new LightSource(Color.Red, new Vector3(0, 500, 0)));

        AddSceneProcessor(light);
        AddSceneProcessor(physics);

        Camera camera = new(new Vector3(0, 800, -500), Graphics.GraphicsDevice.Viewport.AspectRatio, MathHelper.PiOver2, 0.1f, 300000f);
        camera.AddComponent(new MouseController(0.01f));

        Player player = new Player();
        player.Initialize(this);

        Map map = new Map();
        map.Initialize(this);

        camera.Follow(GetEntityByName("player"));
        SetCamera(camera);

        base.Initialize();
    }
}