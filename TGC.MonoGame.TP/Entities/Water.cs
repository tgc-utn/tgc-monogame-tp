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
        private int CantidadDeFilas { get; set; }

        public Water(GraphicsDevice graphicsDevice, Effect effect, int cantidadDeQuadPorLinea, Texture2D texture)
        {
            GraphicsDevice = graphicsDevice;
            CantidadDeFilas = cantidadDeQuadPorLinea;
            Quad = new SimpleQuad(graphicsDevice, effect, texture);
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
        public void Draw(Matrix posicionInicial, Matrix view, Matrix projection, float time)
        {
            float escala = 10f;
            float proximaDistancia = 0f;
            float distanciaEnX = 0f;

            Matrix world = Matrix.CreateScale(escala) * posicionInicial;
            Quad.Draw(world, view, projection, time);
            /*for (int i = 0; i < CantidadDeFilas; ++i)
            {
                for (int j = 0; j < CantidadDeFilas; ++j)
                {
                    Matrix translation = Matrix.CreateTranslation(new Vector3(distanciaEnX, 0, proximaDistancia));
                    Quad.Draw(translation * world, view, projection, time);
                    //Harcodeado porque el tamaño de cada simpleQuad es de 2x2.
                    proximaDistancia += 2f;
                }
                proximaDistancia = 0;
                distanciaEnX += 2f;
            }*/
        }
    }
}
