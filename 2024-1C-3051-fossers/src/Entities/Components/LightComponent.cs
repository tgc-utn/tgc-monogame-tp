using System.Drawing;
using Microsoft.Xna.Framework;
using WarSteel.Scenes;
using Color = Microsoft.Xna.Framework.Color;

namespace WarSteel.Entities;

class LightComponent : IComponent {

    private Color _color;

    private Vector3 _localPosition;

    public LightSource CurrentLightSource;

    public LightComponent(Color light, Vector3 localPosition){
        _color = light;
        _localPosition = localPosition;
    }

    public void Initialize(Entity self, Scene scene){}

    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene){

        Vector3 worldPosition = self.Transform.LocalToWorldPosition(_localPosition);
        CurrentLightSource = new LightSource(_color,worldPosition);

    }

    public LightSource GetLightSource(){
        return CurrentLightSource;
    }

    public void Destroy(Entity self, Scene scene){}
}

public struct LightSource {

    public Color Color;
    public Vector3 Position;

    public LightSource(Color color,  Vector3 pos) : this()
    {
        Color = color;
        Position = pos;
    }


}