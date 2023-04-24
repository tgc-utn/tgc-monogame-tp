using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Entities
{
    /// <summary>
    /// Esta es la primera version de 0.1 del agua.
    /// </summary>
    public class Water
    {
        //Uso BasicEffect porque en los tutoriales lo usan
        //Quise utilizar BasicShader y no pude cargar los vertex buffer e index
        private BasicEffect Effect { get; set; }
        private SimpleQuad Quad { get; set; }

        private int _cantidadDeFilas;

        public Water(GraphicsDevice graphicsDevice, int cantidadDeQuadPorLinea)
        {
            Effect = new BasicEffect(graphicsDevice);

            _cantidadDeFilas = cantidadDeQuadPorLinea;
            Quad = new SimpleQuad(graphicsDevice, Effect);
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


    }
}
