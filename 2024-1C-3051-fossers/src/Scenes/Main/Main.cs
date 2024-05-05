
using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Common;
using WarSteel.Entities.Map;
using WarSteel.Managers;
using System.Collections.Generic;
using System;

namespace WarSteel.Scenes.Main;

public class MainScene : Scene
{
    public MainScene(GraphicsDeviceManager Graphics) : base(Graphics)
    {
    }

    public override void Initialize()
    {
        Camera camera = new(new Vector3(1000, 1000, 0), Graphics.GraphicsDevice.Viewport.AspectRatio, MathHelper.PiOver2, 0.1f, 300000f);
        camera.AddComponent(new MouseController(0.01f));

        SetAmbientLightColor(Color.Red);

        AddEntity(new Tank("player"));
        AddEntity(new Ground());
        
        camera.Follow(GetEntityByName("player"));
        GetEntityByName("player").AddComponent(new LightComponent(Color.White,new Vector3(2000,0,0)));

        SetCamera(camera);
        base.Initialize();
    }


}