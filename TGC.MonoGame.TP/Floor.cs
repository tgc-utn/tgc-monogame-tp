using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Floor
    {
        private Vector3[] Colors;
        private int[] ColorIndex;
        private Matrix[] WorlMatrixs;
        
        private readonly float TileSize = 1000f;
        private readonly int Width = 10;
        private readonly int Hight = 10;
        private readonly int TileQuantity;

        public Floor()
        {
            TileQuantity = Width * Hight;

            WorlMatrixs = new Matrix[TileQuantity];
            ColorIndex = new int[TileQuantity];
            Colors = new Vector3[]
            {
                Color.DarkBlue.ToVector3(),
                Color.DarkGreen.ToVector3()
            };

            
            Matrix Scale = Matrix.CreateScale(TileSize, 0f, TileSize);

            for(int i=0;i<Width;i++)
            {
                for(int j=0;j<Hight;j++)
                {
                    ColorIndex[i*10+j] = (i+j)%2;
                    WorlMatrixs[i*10+j] = Scale * Matrix.CreateTranslation(TileSize*i*2, 0, TileSize*j*2);
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