using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Entities
{
    /// <summary>
    /// Esta es la primera version de 0.1 del agua.
    /// </summary>
    public class Water
    {
        private SimpleQuad Quad { get; set; }
        private GraphicsDevice GraphicsDevice { get; set; }
        private float[,] Waves { get; set; }
        private int _cantidadDeFilas;

        public Water(GraphicsDevice graphicsDevice, Effect effect, int cantidadDeQuadPorLinea)
        {
            GraphicsDevice = graphicsDevice;
            _cantidadDeFilas = cantidadDeQuadPorLinea;
            Waves = new float[50, 50];
            Quad = new SimpleQuad(graphicsDevice, effect);
        }

        // Capaz la segunda versión estaría buena que sea algo así.
        // indicando donde empieza y donde termina el mar. y capaz otro valor que indique el tamaño de los quads
        // Draw(Vector3 posicionInicial, Vector3 posicionFinal, Matrix view, Matrix projection)


        /// <summary>
        /// Recibe la posicion donde se va a empezar a generar el mar. 
        /// Empieza a dibujar de izquierda a derecha. Por lo tanto si seteamos la posicion en 0. Va a empezar a dibujar hacia la izquierda
        /// (Todo esto basado en la camara que tenemos ahora, obviamente)
        /// </summary>
        /// <param name="posicionInicial"></param>
        /// <param name="view"></param>
        /// <param name="projection"></param>
        public void Draw(Matrix posicionInicial, Matrix view, Matrix projection)
        {
            float escala = 6f;
            float proximaDistancia = 0f;
            float distanciaEnX = 0f;

            Matrix world = Matrix.CreateScale(escala) * posicionInicial;

            for(int i = 0; i < _cantidadDeFilas; ++i)
            {


                for(int j = 0; j < _cantidadDeFilas; ++j)
                {
                    //Dibujo columna
                    Quad.Draw(Matrix.CreateTranslation(new Vector3(distanciaEnX, 0f, proximaDistancia)) * world, view, projection);

                    //Harcodeado porque el tamaño de cada simpleQuad es de 2x2.
                    proximaDistancia += 2f;

                }

                proximaDistancia = 0;
                //Idem arriba
                distanciaEnX += 2f;
            }

        }

        public void UpdateWaves()
        {
            Random rnd = new Random();
            for (int i = 0; i < Waves.GetLength(0); i++)
            {
                for (int j = 0; j < Waves.GetLength(1); j++)
                {
                    Waves[i, j] = rnd.NextSingle() - 1.5f;
                }
            }
        }

        public void DrawWaves(Matrix posicionInicial, Matrix view, Matrix projection)
        {
            float proximaDistancia = 0f;
            float distanciaEnX = 0f;
            float escala = 10f;
            Matrix world = Matrix.CreateScale(new Vector3(escala, 1, escala)) * posicionInicial;
            for (int i = 0; i < Waves.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < Waves.GetLength(1) - 1; j++)
                {
                    Quad.ModifyVertexBuffer(new [,]
                    {
                        { Waves[i, j+1], Waves[i, j] },
                        { Waves[i+1, j+1], Waves[i+1, j]}
                    });
                    Quad.Draw(Matrix.CreateTranslation(new Vector3(distanciaEnX, 0f, proximaDistancia)) * world, view, projection);   
                    proximaDistancia += 2f;
                }
                proximaDistancia = 0;
                distanciaEnX += 2f;
            }
        }
    }
}
