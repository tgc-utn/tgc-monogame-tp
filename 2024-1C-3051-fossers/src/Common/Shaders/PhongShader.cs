
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Entities;
using WarSteel.Managers;
using WarSteel.Scenes;
using Vector3 = Microsoft.Xna.Framework.Vector3;

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
        Effect = ContentRepoManager.Instance().GetEffect("PhongShader");
    }

    public override void ApplyEffects(Scene scene)
    {
        Matrix[] matrices = new Matrix[LIGHTSOURCE_LIMIT];

        List<LightSource> sources = scene.GetLightSources();

        for (int i = 0; i < LIGHTSOURCE_LIMIT; i++)
        {

            if (i < sources.Count)
            {
                matrices[i] = new Matrix();
                LightSource source = sources[i];

                 matrices[i] = new Matrix(
                    source.Color.R, source.Position.X, source.Direction.X, source.ConeAngle,
                    source.Color.G, source.Position.Y, source.Direction.Y, 0,
                    source.Color.B, source.Position.Z, source.Direction.Z, 0,
                    1, 0, 0, 0
                );
            }
            else
            {
                matrices[i] = new Matrix(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }

            // Effect.Parameters["LightSources"].Elements[i].SetValue(matrices[i]);
        }


        Color color = scene.GetAmbientLightColor();
        Effect.Parameters["AmbientLight"].SetValue(color.ToVector3());
        Effect.Parameters["LightSourceCount"].SetValue(sources.Count);
        Effect.Parameters["MaterialProperties"].SetValue(new Vector3(_ambient, _diffuse, _specular));
        Effect.Parameters["Texture"].SetValue(_texture);
       



    }
}
