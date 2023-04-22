using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class Alfombra
    {
        private Matrix World;

        public Alfombra(Matrix World) {
            this.World = World;
        }
        
        internal void Draw() {
            TGCGame.GameContent.E_TextureShader.Parameters["World"].SetValue(World); 
            TGCGame.GameContent.E_TextureShader.Parameters["Texture"].SetValue(TGCGame.GameContent.T_Alfombra); 
            TGCGame.GameContent.G_Quad.Draw(TGCGame.GameContent.E_TextureShader);
        }
    }
}