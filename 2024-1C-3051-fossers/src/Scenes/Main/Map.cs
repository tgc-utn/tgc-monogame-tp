using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Entities.Map;

namespace WarSteel.Scenes.Main;

class Map
{
    public void Initialize(Scene scene)
    {

        Entity ground = new Ground();
        scene.AddEntityBeforeRun(ground);

        // Forest
        List<Entity> trees = EntityGenerator.Generate(new Vector3(0, -0, 0), 100, typeof(SimpleTree));
        trees.ForEach(tree => scene.AddEntityBeforeRun(tree));

        // Rocks
        List<Entity> bigRocks = EntityGenerator.Generate(new Vector3(0, -0, 0), 25, typeof(Rock), RockSize.LARGE);
        List<Entity> mediumRocks = EntityGenerator.Generate(new Vector3(0, -00, 0), 25, typeof(Rock), RockSize.MEDIUM);
        List<Entity> smallRocks = EntityGenerator.Generate(new Vector3(0, -00, 0), 25, typeof(Rock), RockSize.SMALL);

        bigRocks.ForEach(rock => scene.AddEntityBeforeRun(rock));
        mediumRocks.ForEach(rock => scene.AddEntityBeforeRun(rock));
        smallRocks.ForEach(rock => scene.AddEntityBeforeRun(rock));

        // Vegetation
        List<Entity> bush = EntityGenerator.Generate(new Vector3(0, -10, 0), 25, typeof(Bush));
        bush.ForEach(bush => scene.AddEntityBeforeRun(bush));

        // tanks
        // List<Entity> tanks = EntityGenerator.Generate(new Vector3(0, -10, 0), 5, typeof(Tank), "tank");
        // tanks.ForEach(tank => scene.AddEntity(tank));

    }

}