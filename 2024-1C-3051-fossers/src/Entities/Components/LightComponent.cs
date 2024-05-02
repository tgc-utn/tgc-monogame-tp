using System.Drawing;
using Microsoft.Xna.Framework;
using WarSteel.Scenes;
using Color = Microsoft.Xna.Framework.Color;

namespace WarSteel.Entities;

class LightComponent : Component {

    private Light _light;
    private Vector3 _localDirection;

    private Vector3 _localPosition;

    private float _coneAngle;

    public LightComponent(Light light, Vector3 localDirection, Vector3 localPosition, float coneAngle){
        _light = light;
        _localDirection = localDirection;
        _coneAngle = coneAngle;
        _localPosition = localPosition;
    } 

    public void UpdateEntity(Entity self, GameTime gameTime, Scene scene){

        Vector3 worldDirection = Vector3.Transform(_localDirection,self.Transform.GetWorld()) - self.Transform.Pos;
        Vector3 worldPosition = Vector3.Transform(_localPosition, self.Transform.GetWorld());
        scene.AddLightSource(new LightSource(_light,worldDirection,_coneAngle,worldPosition));

    }

}

public struct LightSource {

    public Light Light;
    public Vector3 Direction;
    public float ConeAngle;
    public Vector3 Position;

    public LightSource(Light light, Vector3 localDirection, float coneAngle, Vector3 pos) : this()
    {
        Light = light;
        Direction = localDirection;
        ConeAngle = coneAngle;
        Position = pos;
    }
}

public struct Light {

    public Color Color;
    public float Intensity;

    public Light(Color color, float intensity){
        Color = color;
        Intensity = intensity;
    }

}