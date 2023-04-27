/* using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    // PISO
    public class Piso
    {
        private Vector3[] ColorPallette;
        private int[] ColorIndex;
        private Matrix[] WorldMatrixs;
        internal Vector3 PosicionInicial;
        private Effect Effect = TGCGame.GameContent.E_BasicShader;
        private readonly float TileSize = 100f;
        private readonly int Ancho;
        private readonly int Alto;
        private readonly int CantidadBaldosas;

        public Piso(int Ancho, int Alto, Vector3 PosicionInicial)
        {
            this.PosicionInicial = PosicionInicial; 
            this.Ancho = Ancho;
            this.Alto = Alto;
            CantidadBaldosas = 1;

            WorldMatrixs = new Matrix[CantidadBaldosas];
            ColorIndex = new int[CantidadBaldosas];
            ColorPallette = new Vector3 []{
                new Color(254, 183, 129   ).ToVector3(),
                new Color(233, 130, 89   ).ToVector3(),
                new Color(186, 83, 68   ).ToVector3(),
                new Color(104, 55, 59   ).ToVector3(),
                new Color(76, 38, 39   ).ToVector3(),
                new Color(117, 115, 162   ).ToVector3(),
                new Color(78, 76, 121   ).ToVector3(),
                new Color(45, 48, 85   ).ToVector3(),
                new Color(39, 89, 97   ).ToVector3(),
                new Color(61, 118, 98   ).ToVector3(),
            };
            
            Matrix Scale = Matrix.CreateScale(TileSize*Ancho, 0f, TileSize*Alto);

            ColorIndex[0] = 0;
            WorldMatrixs[0] = Scale * Matrix.CreateTranslation(PosicionInicial);
        }
        public Vector3 getCenter(){
            return PosicionInicial+new Vector3(TileSize/2,0f,TileSize/2);
        }
        public Piso Azul(){
            ColorPallette = new Vector3 []{
                new Color(19, 38, 47).ToVector3()
            };
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
            return this;
        }

        public Piso Oficina(){
            //Pallette link : https://coolors.co/palette/edc4b3-e6b8a2-deab90-d69f7e-cd9777-c38e70-b07d62-9d6b53-8a5a44-774936
            ColorPallette = new Vector3 []{
                new Color(237, 196, 179     ).ToVector3()
            };
            //SetIndexRandom();
            return this;
        }
        public Piso Cocina(){
            ColorPallette = new Vector3 []{
                new Color(246, 216, 174).ToVector3()
            };

            //SetIndexIntercalado();
            return this;
        }
        public Piso Banio(){
            ColorPallette = new Vector3 []{
                new Color(113, 196, 189   ).ToVector3()
            };
            return this;
        }
        public void Draw(Matrix view, Matrix projection)
        {
            Effect.Parameters["DiffuseColor"]?.SetValue(ColorPallette[ColorIndex[0]]);
            Effect.Parameters["Texture"]?.SetValue(TGCGame.GameContent.T_PisoMadera);
            Effect.Parameters["World"].SetValue(WorldMatrixs[0]);
            TGCGame.GameContent.G_Quad.Draw(Effect);

            /* for(int i=0;i<CantidadBaldosas;i++)
            {
                Effect.Parameters["DiffuseColor"]?.SetValue(ColorPallette[ColorIndex[i]]);
                Effect.Parameters["Texture"]?.SetValue(TGCGame.GameContent.T_PisoMadera);
                Effect.Parameters["World"].SetValue(WorldMatrixs[i]);
                TGCGame.GameContent.G_Quad.Draw(Effect);
            } 
       }
    }
} 

*/