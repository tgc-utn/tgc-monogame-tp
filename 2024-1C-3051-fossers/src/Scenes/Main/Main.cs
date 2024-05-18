
using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Common;
using WarSteel.Entities.Map;
using WarSteel.Managers;
using System.Collections.Generic;
using System;
using WarSteel.Scenes.SceneProcessors;

namespace WarSteel.Scenes.Main;

public class MainScene : Scene
{
    public MainScene(GraphicsDeviceManager Graphics) : base(Graphics)
    {
    }

    public override void Initialize()
    {

        AddSceneProcessor(new LightProcessor(Color.AliceBlue));
        AddSceneProcessor(new PhysicsProcessor());

        Camera camera = new(new Vector3(1000, 1000, 0), Graphics.GraphicsDevice.Viewport.AspectRatio, MathHelper.PiOver2, 0.1f, 300000f);
        camera.AddComponent(new MouseController(0.01f));

        Entity tank = new Tank("player");
        Entity ground = new Ground();
        tank.Transform.Pos = new Vector3(0,500,0);




        AddEntity(tank);
        AddEntity(ground);
        
        camera.Follow(GetEntityByName("player"));

        SetCamera(camera);
        base.Initialize();
    }


}