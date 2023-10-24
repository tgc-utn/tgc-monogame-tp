using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Types.References;
using TGC.MonoGame.TP.Utils.Models;

namespace TGC.MonoGame.TP.Utils;

public static class EffectsRepository
{
    public static Effect GetEffect(DrawReference drawReference, ContentManager content)
    {
        return drawReference switch
        {
            ColorReference _ => BasicShader(content),
            TextureReference _ => TextureShader(content),
            MultiTextureReference _ => TextureShader(content),
            _ => throw new ArgumentOutOfRangeException(nameof(drawReference))
        };
    }
    
    public static Effect BasicShader(ContentManager content)
    {
        return content.Load<Effect>(Effects.Effects.BasicShader.Path);
    }
    
    public static Effect TextureShader(ContentManager content)
    {
        return content.Load<Effect>(Effects.Effects.TextureShader.Path);
    }

    public static void SetEffectParameters(Effect effect, DrawReference referenceDrawReference, string meshName)
    {
        switch (referenceDrawReference)
        {
            case ColorReference colorReference:
                effect.Parameters["DiffuseColor"].SetValue(colorReference.Color.ToVector3());
                break;
            case TextureReference textureReference:
                effect.Parameters["baseTexture"].SetValue(textureReference.Texture);
                effect.Parameters["ambientColor"].SetValue(new Vector3(219f, 244f, 76f));
                effect.Parameters["diffuseColor"].SetValue(new Vector3(124f, 125f, 121f));
                effect.Parameters["specularColor"].SetValue(new Vector3(71f, 71f, 65f));
                effect.Parameters["KAmbient"].SetValue(0.480f);
                effect.Parameters["KDiffuse"].SetValue(0.400f);
                effect.Parameters["KSpecular"].SetValue(0.2f);
                effect.Parameters["shininess"].SetValue(500f);
                break;
            case MultiTextureReference multiTextureReference:
                var meshTextureRelation = multiTextureReference.Relations.Find(relation => relation.Mesh == meshName);
                effect.Parameters["baseTexture"].SetValue(meshTextureRelation.Texture.Texture);
                effect.Parameters["ambientColor"].SetValue(new Vector3(219f, 244f, 76f));
                effect.Parameters["diffuseColor"].SetValue(new Vector3(124f, 125f, 121f));
                effect.Parameters["specularColor"].SetValue(new Vector3(71f, 71f, 65f));
                effect.Parameters["KAmbient"].SetValue(0.480f);
                effect.Parameters["KDiffuse"].SetValue(0.400f);
                effect.Parameters["KSpecular"].SetValue(0.2f);
                effect.Parameters["shininess"].SetValue(500f);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(referenceDrawReference));
        }
    }
}