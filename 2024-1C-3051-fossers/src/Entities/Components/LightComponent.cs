using System.Drawing;
using Microsoft.Xna.Framework;
using WarSteel.Scenes;
using Color = Microsoft.Xna.Framework.Color;

namespace WarSteel.Entities;

class LightComponent : Component {

    private Color _color;
    private Vector3 _localDirection;

    private Vector3 _localPosition;

    private float _coneAngle;

    public LightComponent(Color light, Vector3 localDirection, Vector3 localPosition, float coneAngle){
        _color = light;
        _localDirection = localDirection;
        _coneAngle = coneAngle;
        _localPosition = localPosition;
    } 

    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene){

        Vector3 worldDirection = Vector3.Transform(_localDirection,self.Transform.GetWorld()) - self.Transform.Pos;
        Vector3 worldPosition = Vector3.Transform(_localPosition, self.Transform.GetWorld());
        scene.AddLightSource(new LightSource(_color,worldDirection,_coneAngle,worldPosition));

    }

}

public struct LightSource {

    public Color Color;
    public Vector3 Direction;
    public float ConeAngle;
    public Vector3 Position;

    public LightSource(Color color, Vector3 localDirection, float coneAngle, Vector3 pos) : this()
    {
        Color = color;
        Direction = localDirection;
        ConeAngle = coneAngle;
        Position = pos;
    }
}