
using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarSteel.Entities;
using WarSteel.Managers;
using WarSteel.Scenes;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Vector4 = Microsoft.Xna.Framework.Vector4;

namespace WarSteel.Common.Shaders;

class PhongShader : Shader
{
    private const int LIGHTSOURCE_LIMIT = 2;
    private float ambientCoefficient;
    private float diffuseCoefficient;
    private Texture2D texture;

    public PhongShader(float ambient, float diffuse, Texture2D texture)
    {
        ambientCoefficient = ambient;
        diffuseCoefficient = diffuse;
        this.texture = texture;
        Effect = ContentRepoManager.Instance().GetEffect("PhongShader");
    }

    public override void ApplyEffects(Scene scene)
    {

        List<LightSource> sources = scene.GetLightSources();
        Vector3[] colors = new Vector3[LIGHTSOURCE_LIMIT];
        Vector3[] positions = new Vector3[LIGHTSOURCE_LIMIT];

        for (int i = 0; i < Math.Min(sources.Count,LIGHTSOURCE_LIMIT) ; i++)
        {
            Vector3 sourceColor = sources[i].Color.ToVector3();
            Vector3 sourcePosition = sources[i].Position;
            colors[i] = sourceColor;
            positions[i] = sourcePosition;
        }

        Effect.Parameters["AmbientLight"].SetValue(scene.GetAmbientLightColor().ToVector3());
        Effect.Parameters["AmbientCoefficient"].SetValue(ambientCoefficient);
        Effect.Parameters["LightSourcePositions"].SetValue(positions);
        Effect.Parameters["LightSourceColors"].SetValue(colors);
        Effect.Parameters["DiffuseCoefficient"].SetValue(diffuseCoefficient);
        Effect.Parameters["Texture"].SetValue(texture);

    }
}
