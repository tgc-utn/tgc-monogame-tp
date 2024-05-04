using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Entities;

namespace WarSteel.Scenes;

public class Scene
{
    private Dictionary<string, Entity> entities = new Dictionary<string, Entity>();
    protected GraphicsDeviceManager Graphics;
    protected Camera camera;

    private List<LightSource> lightSources = new List<LightSource>();

    private Color ambientLight = Color.Blue;

    public Scene(GraphicsDeviceManager Graphics)
    {
        this.Graphics = Graphics;
    }

    public void SetCamera(Camera camera)
    {
        entities.Add(camera.Id, camera);
        this.camera = camera;
    }

    public Camera GetCamera()
    {
        return camera;
    }

    public GraphicsDeviceManager GetGraphicsDevice()
    {
        return Graphics;
    }

    public void AddEntity(Entity entity)
    {
        entities.Add(entity.Id, entity);
    }

    public Entity GetEntityByName(string name)
    {
        foreach (var entity in entities.Values)
        {
            if (entity.Name == name)
                return entity;
        }
        return null;
    }
    public virtual void Initialize()
    {
        foreach (var entity in entities.Values)
        {
            entity.Initialize();
        }
    }

    public virtual void LoadContent()
    {
        foreach (var entity in entities.Values)
        {
            entity.LoadContent();
        }
    }

    public virtual void Draw()
    {
        foreach (var entity in entities.Values)
        {
            entity.Draw(this);
        }
        ClearLightSources();
    }
    public virtual void Update(GameTime gameTime)
    {
        foreach (var entity in entities.Values)
        {
            entity.Update(gameTime, this);
        }
    }

    public void AddLightSource(LightSource lightSource)
    {
        lightSources.Add(lightSource);
    }

    public List<LightSource> GetLightSources()
    {
        return lightSources;
    }

    public Color GetAmbientLightColor()
    {
        return ambientLight;
    }

    public void ClearLightSources(){
        lightSources = new List<LightSource>();
    }

    public void SetAmbientLightColor(Color color)
    {
        ambientLight = color;
    }
    public virtual void Unload()
    {

    }

}