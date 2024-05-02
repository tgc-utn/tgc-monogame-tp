
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Entities;
using WarSteel.Scenes;

namespace WarSteel.Common.Shaders;

class PhongShader : Shader
{
    private const int LIGHTSOURCE_LIMIT = 8;
    private float _ambient;
    private float _diffuse;
    private float _specular;

    private Texture2D _texture;

    public PhongShader(float ambient, float diffuse, float specular, Texture2D texture)
    {
        _ambient = ambient;
        _diffuse = diffuse;
        _specular = specular;
        _texture = texture;
    }

    public override void ApplyEffects(Scene scene)
    {
        Matrix[] matrices = new Matrix[LIGHTSOURCE_LIMIT];
        
        List<LightSource> sources = scene.GetLightSources();

        for(int i = 0; i < LIGHTSOURCE_LIMIT; i ++){
            matrices[i] = new Matrix4x4();
            LightSource source = sources[i];

            matrices[i].M11 = source.Light.Color.R;
            matrices[i].M21 = source.Light.Color.G;
            matrices[i].M31 = source.Light.Color.B;
            matrices[i].M41 = 0;

            matrices[i].M12 = source.Position.X;
            matrices[i].M22 = source.Position.Y;
            matrices[i].M32 = source.Position.Z;
            matrices[i].M42 = 0;

            matrices[i].M13 = source.Direction.X;
            matrices[i].M23 = source.Direction.Y;
            matrices[i].M33 = source.Direction.Z;
            matrices[i].M43 = 0;

            matrices[i].M14 = source.ConeAngle;
            matrices[i].M24 = source.Light.Intensity;
            matrices[i].M34 = 0;
            matrices[i].M44 = 0;

        }

        Effect.Parameters["LightSources"].SetValue(matrices);
    
    }
}
