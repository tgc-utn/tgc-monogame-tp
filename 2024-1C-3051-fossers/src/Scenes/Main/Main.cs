
using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Common;
using WarSteel.Scenes.SceneProcessors;
using Microsoft.Xna.Framework.Graphics;


namespace WarSteel.Scenes.Main;

public class MainScene : Scene
{
    public MainScene(GraphicsDeviceManager Graphics, SpriteBatch SpriteBatch) : base(Graphics, SpriteBatch)
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

        // add skybox
        AddEntityBeforeRun(new SkyBox());

        Tank player = new Tank("player");
        player.Transform.Position = new Vector3(0, 400, 0);

        AddEntityBeforeRun(player);

        Camera camera = new(new Vector3(0, 800, -500), GraphicsDeviceManager.GraphicsDevice.Viewport.AspectRatio, GraphicsDeviceManager.GraphicsDevice, MathHelper.PiOver2, 0.1f, 300000f);
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