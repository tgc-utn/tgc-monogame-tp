
using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Common;
using WarSteel.Entities.Map;
using WarSteel.Managers;
using System.Collections.Generic;

namespace WarSteel.Scenes.Main;

public class MainScene : Scene
{
    public MainScene(GraphicsDeviceManager Graphics) : base(Graphics)
    {
    }

    public override void Initialize()
    {
        Camera camera = new(new Vector3(2000, 2000, 0), Graphics.GraphicsDevice.Viewport.AspectRatio, MathHelper.PiOver2, 0.1f, 300000f);
        camera.AddComponent(new MouseController(0.01f));
        

        AddEntity(new Tank("player"));
        AddEntity(new Ground());

        List<Entity> trees = EntityGenerator.Generate(new Vector3(0, -10, 0), 100, typeof(SimpleTree));
        List<Entity> bigRocks = EntityGenerator.Generate(new Vector3(0, -10, 0), 25, typeof(Rock), RockSize.LARGE);
        List<Entity> mediumRocks = EntityGenerator.Generate(new Vector3(0, -10, 0), 25, typeof(Rock), RockSize.MEDIUM);
        List<Entity> smallRocks = EntityGenerator.Generate(new Vector3(0, -10, 0), 25, typeof(Rock), RockSize.SMALL);
        List<Entity> bush = EntityGenerator.Generate(new Vector3(0, -10, 0), 25, typeof(Bush));

        bigRocks.ForEach(rock => AddEntity(rock));
        mediumRocks.ForEach(rock => AddEntity(rock));
        smallRocks.ForEach(rock => AddEntity(rock));
        trees.ForEach(tree => AddEntity(tree));
        bush.ForEach(bush => AddEntity(bush));

        camera.Follow(GetEntityByName("player"));

        SetCamera(camera);
        base.Initialize();
    }


}