using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{


    class TextureShader : Shader
    {

        private Texture2D _texture;


        public TextureShader(Texture2D texture){
            this._texture = texture;
            this.Effect = ContentRepository.GetInstance().GetEffect("BasicTextureShader");
        }

        public override void ApplyEffects()
        {
          this.Effect.Parameters["Texture"].SetValue(_texture);
        }

    }

}