using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Managers;

namespace WarSteel.Scenes;

public class Scene
{
    protected Camera camera;
    protected EntitiesManager entities = EntitiesManager.Instance();
    protected GraphicsDeviceManager Graphics;
    public Scene(GraphicsDeviceManager Graphics)
    {
        this.Graphics = Graphics;
    }

    public virtual void Initialize()
    {
        entities.InitializeAll();
    }
    public virtual void LoadContent()
    {
        entities.LoadContentAll();
    }
    public virtual void Draw()
    {
        entities.DrawAll(camera);
    }
    public virtual void Update()
    {
        entities.UpdateAll();
    }
    public virtual void Unload()
    {
        entities.UnloadAll();
    }
}