
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
       
       
        AddEntity(new Ground());

        // Forest
        List<Entity> trees = EntityGenerator.Generate(new Vector3(0, -10, 0), 100, typeof(SimpleTree));
        trees.ForEach(tree => AddEntity(tree));

        // Rocks
        List<Entity> bigRocks = EntityGenerator.Generate(new Vector3(0, -10, 0), 25, typeof(Rock), RockSize.LARGE);
        List<Entity> mediumRocks = EntityGenerator.Generate(new Vector3(0, -10, 0), 25, typeof(Rock), RockSize.MEDIUM);
        List<Entity> smallRocks = EntityGenerator.Generate(new Vector3(0, -10, 0), 25, typeof(Rock), RockSize.SMALL);
        
        bigRocks.ForEach(rock => AddEntity(rock));
        mediumRocks.ForEach(rock => AddEntity(rock));
        smallRocks.ForEach(rock => AddEntity(rock));

        // Vegetation
        List<Entity> bush = EntityGenerator.Generate(new Vector3(0, -10, 0), 25, typeof(Bush));
        bush.ForEach(bush => AddEntity(bush));
        
        
        Camera camera = new(new Vector3(2000, 2000, 0), Graphics.GraphicsDevice.Viewport.AspectRatio, MathHelper.PiOver2, 0.1f, 300000f);
        
        SetCamera(camera);
      
        AddEntity(new Tank("player"));
        camera.Follow(GetEntityByName("player"));

        base.Initialize();
    }

   
}