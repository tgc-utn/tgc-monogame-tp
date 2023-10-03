using System;
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
                effect.Parameters["BaseTexture"].SetValue(textureReference.Texture);
                break;
            case MultiTextureReference multiTextureReference:
                var meshTextureRelation = multiTextureReference.Relations.Find(relation => relation.Mesh == meshName);
                effect.Parameters["BaseTexture"].SetValue(meshTextureRelation.Texture.Texture);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(referenceDrawReference));
        }
    }
}