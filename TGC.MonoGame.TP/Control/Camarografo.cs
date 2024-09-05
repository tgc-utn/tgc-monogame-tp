using Escenografia;
using Microsoft.Xna.Framework;

namespace Control
{
    class Camarografo//laquitoo
    {
        private Control.Camara camaraAsociada;
        private Matrix projeccion;
        
        public Camarografo(Vector3 posicion, Vector3 puntoDeFoco, float AspectRatio, float minVista, float maxVista)
        {
            //iniciamos la camara
            camaraAsociada = new Control.Camara(posicion, puntoDeFoco);
            projeccion = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, AspectRatio, minVista, maxVista);
        }
        public Matrix getViewMatrix()
        {
            return camaraAsociada.getViewMatrix();
        }
        public Matrix getProjectionMatrix()
        {
            return projeccion;
        }
        public void setPuntoAtencion(Vector3 PuntoAtencion)
        {
            camaraAsociada.PuntoAtencion = PuntoAtencion;
        }
    }
}