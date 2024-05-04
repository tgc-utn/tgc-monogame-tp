using System.Drawing;
using Microsoft.Xna.Framework;
using WarSteel.Scenes;
using Color = Microsoft.Xna.Framework.Color;

namespace WarSteel.Entities;

class LightComponent : Component {

    private Color _color;
    private Vector3 _localDirection;

    private Vector3 _localPosition;

    public LightComponent(Color light, Vector3 localPosition){
        _color = light;
        _localPosition = localPosition;
    } 

    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene){

        Vector3 worldPosition = Vector3.Transform(_localPosition, self.Transform.GetWorld());
        scene.AddLightSource(new LightSource(_color,worldPosition));

    }

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