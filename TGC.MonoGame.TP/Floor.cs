using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Floor
    {
        private Vector3[] Colors;
        private int[] ColorIndex;
        private Matrix[] WorlMatrixs;
        
        private readonly float TileSize = 500f;
        private readonly int Width = 10;
        private readonly int Hight = 10;
        private readonly int TileQuantity;

        public Floor()
        {
            TileQuantity = Width * Hight;

            WorlMatrixs = new Matrix[TileQuantity];
            ColorIndex = new int[TileQuantity];
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
            /* Colors = new Vector3[]
            {
                Color.DarkBlue.ToVector3(),
                Color.DarkGreen.ToVector3()
            }; */

            
            Matrix Scale = Matrix.CreateScale(TileSize, 0f, TileSize);

            for(int i=0;i<Width;i++)
            {
                for(int j=0;j<Hight;j++)
                {
                    var indiceColor = ( Convert.ToInt32(Random.Shared.NextSingle()*100) )%Colors.Length;
                    ColorIndex[i*Hight+j] = indiceColor;
                    WorlMatrixs[i*Hight+j] = Scale * Matrix.CreateTranslation(TileSize*i*2, 0, TileSize*j*2);
                }
            }
        }

        public void Draw(Effect Effect)
        {
            for(int i=0;i<TileQuantity;i++)
            {
                Effect.Parameters["DiffuseColor"].SetValue(Colors[ColorIndex[i]]);
                Effect.Parameters["World"].SetValue(WorlMatrixs[i]);
                TGCGame.GeometriesManager.Quad.Draw(Effect);
            }
        }
    }
}