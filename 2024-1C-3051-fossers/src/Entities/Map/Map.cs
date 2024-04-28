using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Managers;


namespace WarSteel.Entities.Map;

public class Map
{
    public Map() { }

    public static void Init()
    {
        EntitiesManager entities = EntitiesManager.Instance();
        entities.Add(new Ground());

        // Forest
        List<Entity> trees = ModelsGenerator.Generate(new Vector3(0, -10, 0), 100, typeof(SimpleTree));
        trees.ForEach(tree => entities.Add(tree));

        // Rocks
        List<Entity> bigRocks = ModelsGenerator.Generate(new Vector3(0, -10, 0), 25, typeof(Rock), RockSize.LARGE);
        List<Entity> mediumRocks = ModelsGenerator.Generate(new Vector3(0, -10, 0), 25, typeof(Rock), RockSize.MEDIUM);
        List<Entity> smallRocks = ModelsGenerator.Generate(new Vector3(0, -10, 0), 25, typeof(Rock), RockSize.SMALL);
        bigRocks.ForEach(rock => entities.Add(rock));
        mediumRocks.ForEach(rock => entities.Add(rock));
        smallRocks.ForEach(rock => entities.Add(rock));

        // Vegetation
        List<Entity> bush = ModelsGenerator.Generate(new Vector3(0, -10, 0), 25, typeof(Bush));
        bush.ForEach(bush => entities.Add(bush));
    }

}