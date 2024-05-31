using Microsoft.Xna.Framework;
using WarSteel.Entities;
using WarSteel.Scenes;

class Composite : IComponent
{

    private IComponent[] _components;

    public Composite(IComponent[] components){
        _components = components;
    }

    public void Destroy(Entity self, Scene scene)
    {
        foreach(var c in _components){
            c.Destroy(self,scene);
        }
    }

    public void Initialize(Entity self, Scene scene)
    {
        foreach(var c in _components){
            c.Initialize(self,scene);
        }
    }

    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene)
    {
        foreach(var c in _components){
            c.UpdateEntity(self,gameTime,scene);
        }
    }
}