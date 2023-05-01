using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    // PISO
    public class Piso
    {
        private const float S_METRO = TGCGame.S_METRO;
        
        private Vector3 ColorDefault = new Color(19, 38, 47).ToVector3();
        private Texture2D TexturaBaldosa = TGCGame.GameContent.T_PisoMadera;
        private float CantidadBaldosas = 1f;
        private Matrix World;
        internal Vector3 PosicionInicial;
        private Effect Effect = TGCGame.GameContent.E_BasicShader;
        private readonly int MetrosAncho;
        private readonly int MetrosLargo;
        //private readonly int MetrosCuadrados;

        public Piso(int metrosAncho, int metrosLargo, Vector3 posicionInicial)
        {
            PosicionInicial = posicionInicial; 
            MetrosAncho = metrosAncho;
            MetrosLargo = metrosLargo;
            
            Matrix Scale = Matrix.CreateScale(S_METRO * metrosAncho, 0f, S_METRO * metrosLargo);
            World = Scale 
                    * Matrix.CreateTranslation(PosicionInicial);
        }
        public Vector3 getVerticeExtremo(){
            return PosicionInicial + ( new Vector3(MetrosLargo,0f,MetrosAncho) * S_METRO );
        }
        public Vector3 getCenter() => this.getVerticeExtremo()/2;
        
        public Piso ConColor(Color color){
            ColorDefault = color.ToVector3();
            return this;
        }
        public Piso ConTextura(Texture2D texturaPiso, float cantidadBaldosas = 1){
            Effect = TGCGame.GameContent.E_TextureTiles;
            CantidadBaldosas = cantidadBaldosas;
            TexturaBaldosa = texturaPiso;
            return this;
        }
        public void Draw()
        {
            Effect.Parameters["DiffuseColor"]?.SetValue(ColorDefault);
            Effect.Parameters["Texture"]?.SetValue(TexturaBaldosa);
            Effect.Parameters["TileAmount"]?.SetValue(CantidadBaldosas);
            Effect.Parameters["World"].SetValue(World);
            TGCGame.GameContent.G_Quad.Draw(Effect);
        }
    }
}