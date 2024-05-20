
using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Common;
using WarSteel.Entities.Map;
using WarSteel.Scenes.SceneProcessors;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Entities.Components;

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

        Entity tank = new Tank("player");
        tank.AddComponent(new PlayerControls());
        Entity ground = new Ground();
        tank.Transform.Rotate(new Vector3(0, 180, 0));

        AddEntity(tank);
        AddEntity(ground);

        camera.Follow(GetEntityByName("player"));

        SetCamera(camera);
        base.Initialize();
    }


}