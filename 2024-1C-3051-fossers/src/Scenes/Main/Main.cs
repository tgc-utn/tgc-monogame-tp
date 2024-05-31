
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
        PhysicsProcessor physics = new PhysicsProcessor();

        light.AddLightSource(new LightSource(Color.Red, new Vector3(0, 500, 0)));

        AddSceneProcessor(light);
        AddSceneProcessor(physics);
        AddSceneProcessor(new GizmosProcessor());

        Tank player = new Tank("player");
        player.Transform.Position += Vector3.Up * 300;
        AddEntityBeforeRun(player);

        Camera camera = new(new Vector3(0, 800, -500), Graphics.GraphicsDevice.Viewport.AspectRatio,Graphics.GraphicsDevice, MathHelper.PiOver2, 0.1f, 300000f);
        camera.AddComponent(new CameraController(player.Transform));

        Map map = new Map();
        map.Initialize(this);

  
        SetCamera(camera);

        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}