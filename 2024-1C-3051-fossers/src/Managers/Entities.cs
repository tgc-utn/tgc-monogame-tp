using System.Collections.Generic;
using WarSteel.Common;
using WarSteel.Entities;

namespace WarSteel.Managers;

public class EntitiesManager
{
    private Dictionary<string, Entity> entities = new();


    public static EntitiesManager _INSTANCE = null;
    public static void SetUpInstance() => _INSTANCE = new EntitiesManager();
    public static EntitiesManager Instance() => _INSTANCE;


    // methods
    public void Add(Entity entity)
    {
        entities.Add(entity.Name, entity);
    }

    public Dictionary<string, Entity> Entities() => entities;

    public Entity[] GetAll()
    {
        Entity[] array = new Entity[entities.Count];
        int index = 0;

        foreach (var componentPair in entities)
        {
            array[index] = componentPair.Value;
            index++;
        }

        return array;
    }

    public void GetById(string id) { }
    public void GetByName(string name) { }
    public void GetByTag(string tag) { }

    public void DestroyAll() { }
    public void DestroyById() { }
    public void DestroyByName() { }
    public void DestroyByTag() { }

    public void InitializeAll() { }
    public void LoadContentAll() { }
    public void DrawAll(Camera camera) { }
    public void UpdateAll() { }
    public void UnloadAll() { }
}