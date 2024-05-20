using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Entities;

namespace WarSteel.Scenes.SceneProcessors;

class LightProcessor : ISceneProcessor
{

    private Color AmbientLight;
    private List<LightSource> Sources;

    public LightProcessor(Color AmbientLight)
    {
        this.AmbientLight = AmbientLight;
        Sources = new List<LightSource>();
    }

    public void Draw(Scene scene)
    {
        // Sources = new List<LightSource>();
    }

    public void Initialize(Scene scene) { }

    public void Update(Scene scene, GameTime time)
    {
        List<Entity> entities = scene.GetEntities().FindAll(e => e.GetComponent<LightComponent>() != default);
        Sources.AddRange(entities.ConvertAll(e => e.GetComponent<LightComponent>().GetLightSource()));
    }

    public List<LightSource> GetLightSources()
    {
        return Sources;
    }

    public void AddLightSource(LightSource s){
        Sources.Add(s);
    }

    public Color GetAmbientColor()
    {
        return AmbientLight;
    }

}