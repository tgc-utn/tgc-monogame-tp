using Microsoft.Xna.Framework;
using WarSteel.Common;
using WarSteel.Managers;

namespace WarSteel.Scenes;

public class Scene
{
    protected Camera camera { get; set; }
    protected EntitiesManager entities = EntitiesManager.Instance();
    protected GraphicsDeviceManager Graphics;

    public Scene(GraphicsDeviceManager Graphics)
    {
        this.Graphics = Graphics;
    }

    public virtual void Initialize()
    {
        entities.InitializeAll(camera);
    }
    public virtual void LoadContent()
    {
        entities.LoadContentAll(camera);
    }
    public virtual void Draw()
    {
        entities.DrawAll(camera);
    }
    public virtual void Update(GameTime gameTime)
    {
        entities.UpdateAll(gameTime, camera);
    }
    public virtual void Unload()
    {
        entities.UnloadAll();
    }
}