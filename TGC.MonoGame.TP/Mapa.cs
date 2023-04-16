using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    // PISO
    public class Mapa
    {
        private Vector3[] Colors;
        private int[] ColorIndex;
        private Matrix[] WorldMatrixs;
        private Effect Effect;
        private readonly float TileSize = 500f;
        private readonly int Width = 10;
        private readonly int Hight = 10;
        private readonly int TileQuantity;

        public Mapa()
        {
            TileQuantity = Width * Hight;

            WorldMatrixs = new Matrix[TileQuantity];
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
            
            Matrix Scale = Matrix.CreateScale(TileSize, 0f, TileSize);

            for(int i=0;i<Width;i++)
            {
                for(int j=0;j<Hight;j++)
                {
                    var indiceColor = ( Convert.ToInt32(Random.Shared.NextSingle()*100) )%Colors.Length;
                    ColorIndex[i*Width+j] = indiceColor;
                    WorldMatrixs[i*Width+j] = Scale * Matrix.CreateTranslation(TileSize*i*2, 0, TileSize*j*2);
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
            for(int i=0;i<TileQuantity;i++)
            {
                Effect.Parameters["DiffuseColor"].SetValue(Colors[ColorIndex[i]]);
                Effect.Parameters["World"].SetValue(WorldMatrixs[i]);
                TGCGame.GeometriesManager.Quad.Draw(Effect);
            }
        }
    }
}