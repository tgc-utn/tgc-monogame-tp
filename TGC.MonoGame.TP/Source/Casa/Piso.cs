using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    // PISO
    public class Piso
    {
        private Vector3[] ColorPallette;
        private Texture2D TexturaBaldosa = TGCGame.GameContent.T_PisoMadera;
        private int[] ColorIndex;
        private Matrix[] WorldMatrixs;
        internal Vector3 PosicionInicial;
        private Effect Effect = TGCGame.GameContent.E_BasicShader;
        private readonly float TileSize = 1000f;
        private readonly int Ancho;
        private readonly int Alto;
        private readonly int CantidadBaldosas;

        public Piso(int Ancho, int Alto, Vector3 PosicionInicial)
        {
            this.PosicionInicial = PosicionInicial; 
            this.Ancho = Ancho;
            this.Alto = Alto;
            CantidadBaldosas = Ancho * Alto;

            WorldMatrixs = new Matrix[CantidadBaldosas];
            ColorIndex = new int[CantidadBaldosas];
            ColorPallette = new Vector3 []{
                new Color(254, 183, 129   ).ToVector3(),
            };
            
            Matrix Scale = Matrix.CreateScale(TileSize, 0f, TileSize);

            for(int i=0;i<Ancho;i++)
            {
                for(int j=0;j<Alto;j++)
                {
                    var indiceColor = ( Convert.ToInt32(Random.Shared.NextSingle()*100) )%ColorPallette.Length;

                    ColorIndex[i*Ancho+j] = indiceColor;
                    WorldMatrixs[i*Ancho+j] = Scale * Matrix.CreateTranslation(PosicionInicial) * Matrix.CreateTranslation(TileSize*i, 0, TileSize*j);
                }
            }
        }
        public Vector3 getCenter(){
            return PosicionInicial+new Vector3(Ancho*TileSize/2,0f,Alto*TileSize/2);
        }
        public Piso Azul(){
            ColorPallette = new Vector3 []{
                new Color(19, 38, 47).ToVector3()
            };
            SetIndexRandom();
            return this;
        }

        public Piso Madera(){
            Effect = TGCGame.GameContent.E_TextureShader;
            return this;
        }


        public Piso ConColor(Color color){
            ColorPallette = new Vector3 []{
                color.ToVector3()
                };

            SetIndexRandom();
            return this;
        }

        public Piso Oficina(){
            //Pallette link : https://coolors.co/palette/edc4b3-e6b8a2-deab90-d69f7e-cd9777-c38e70-b07d62-9d6b53-8a5a44-774936
            ColorPallette = new Vector3 []{
                new Color(237, 196, 179     ).ToVector3(),
                new Color(230, 184, 162     ).ToVector3(),
                new Color(222, 171, 144        ).ToVector3(),
                new Color(214, 159, 126     ).ToVector3(),
                new Color(205, 151, 119     ).ToVector3(),
                new Color(195, 142, 112     ).ToVector3(),
                new Color(176, 125, 98        ).ToVector3(),
                new Color(157, 107, 83     ).ToVector3(),
                new Color(138, 90, 68     ).ToVector3(),
                new Color(119, 73, 54        ).ToVector3()
            };
            SetIndexRandom();
            return this;
        }
        public Piso Texturado(Texture2D texturaPiso){
            Effect = TGCGame.GameContent.E_TextureShader;
            TexturaBaldosa = texturaPiso;
            return this;
        }
        public Piso Cocina(){
            Effect = TGCGame.GameContent.E_TextureShader;
            TexturaBaldosa = TGCGame.GameContent.T_PisoCeramica;
            return this;
        }
        public Piso Alfombrado(){
            Effect = TGCGame.GameContent.E_TextureMirror;
            TexturaBaldosa = TGCGame.GameContent.T_PisoAlfombrado;
            return this;
        }
        public Piso Banio(){
            ColorPallette = new Vector3 []{
                new Color(113, 196, 189   ).ToVector3(),
                new Color(74, 103, 101   ).ToVector3()
            };
            SetIndexIntercalado();
            return this;
        }
        private void SetIndexIntercalado(){
            for(int i=0;i<Ancho;i++)
            {
                for(int j=0;j<Alto;j++)
                {
                    var indiceColor = (i+j)%ColorPallette.Length;
                    ColorIndex[i*Ancho+j] = indiceColor;
                }
            }
        }  
        private void SetIndexRandom(){
            for(int i=0;i<Ancho;i++)
            {
                for(int j=0;j<Alto;j++)
                {
                    var indiceColor = ( Convert.ToInt32(Random.Shared.NextSingle()*100) )%ColorPallette.Length;

                    ColorIndex[i*Ancho+j] = indiceColor;
                }
            }
        } 
        private void SetIndexCuadrado(){
            for(int i=0;i<Ancho;i++)
            {
                for(int j=0;j<Alto;j++)
                {
                    var indiceColor = (i%2==1 || j%2==1) ? 0 : 1; 
                    ColorIndex[i*Ancho+j] = indiceColor;
                }
            }
        } 
        public void Draw(Matrix view, Matrix projection)
        {
            for(int i=0;i<CantidadBaldosas;i++)
            {
                Effect.Parameters["DiffuseColor"]?.SetValue(ColorPallette[ColorIndex[i]]);
                Effect.Parameters["Texture"]?.SetValue(TexturaBaldosa);
                Effect.Parameters["World"].SetValue(WorldMatrixs[i]);
                TGCGame.GameContent.G_Quad.Draw(Effect);
            }
        }
    }
}