using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Types.References;

namespace TGC.MonoGame.TP.Utils;

public static class TexturesRepository
{
    public static void InitializeTextures(DrawReference referenceDrawReference, ContentManager content)
    {
        switch (referenceDrawReference)
        {
            case TextureReference textureReference:
                textureReference.SetTexture(content.Load<Texture2D>(textureReference.Path));
                break;
            case MultiTextureReference multiTextureReference:
                foreach (var meshTextureRelation in multiTextureReference.Relations)
                    meshTextureRelation.Texture.SetTexture(content.Load<Texture2D>(meshTextureRelation.Texture.Path));
                break;
            case ColorReference _:
                break;
            case BasicTextureReference textureReference:
                textureReference.SetTexture(content.Load<Texture2D>(textureReference.Path));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(referenceDrawReference));
        }
    }
}