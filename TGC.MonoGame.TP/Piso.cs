using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    // PISO
    public class Piso
    {
        private Vector3[] Colors;
        private int[] ColorIndex;
        private Matrix[] WorldMatrixs;
        internal Vector3 PosicionInicial;
        private Effect Effect;
        //LOS TAMAÑOS SE SUPLICAN, Por una razon que conozco pero no les voy a decir porque me llevaria mucho tiempo
        private readonly float TileSize = 500f;
        private readonly int Ancho;
        private readonly int Alto;
        private readonly int CantidadDeBaldosa;

        public Piso(int Ancho, int Alto, Vector3 PosicionInicial)
        {
            this.PosicionInicial = PosicionInicial; 
            this.Ancho = Ancho;
            this.Alto = Alto;
            CantidadDeBaldosa = Ancho * Alto;

            WorldMatrixs = new Matrix[CantidadDeBaldosa];
            ColorIndex = new int[CantidadDeBaldosa];
            Colors = new Vector3 []{
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
            
            Matrix Scale = Matrix.CreateScale(TileSize, 0f, TileSize);

            for(int i=0;i<Ancho;i++)
            {
                for(int j=0;j<Alto;j++)
                {
                    var indiceColor = ( Convert.ToInt32(Random.Shared.NextSingle()*100) )%Colors.Length;

                    ColorIndex[i*Ancho+j] = indiceColor;
                    WorldMatrixs[i*Ancho+j] = Scale * Matrix.CreateTranslation(PosicionInicial) * Matrix.CreateTranslation(TileSize*i*2, 0, TileSize*j*2);
                }
            }
        }

        public void Load(ContentManager content){
            Effect = content.Load<Effect>("Effects/BasicShader");
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            for(int i=0;i<CantidadDeBaldosa;i++)
            {
                Effect.Parameters["DiffuseColor"].SetValue(Colors[ColorIndex[i]]);
                Effect.Parameters["World"].SetValue(WorldMatrixs[i]);
                TGCGame.GeometriesManager.Quad.Draw(Effect);
            }
        }
    }
}