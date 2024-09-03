using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
//como solo tendremos una projeccion presumo solo habra una camara, pero despues podemos cambiarlo,
//si quieren hacer algo como tener dos camara, una mas cerca del auto o algo parecido
namespace Control
{
    class Camera
    {
        //el lugar donde esta pocisionada
        public Vector3 posicion;
        //a si a donde este mirando la camara
        public Vector3 PuntoAtencion; 
        
        public Camera(Vector3 posicion, Vector3 PuntoAtencion)
        {
            this.posicion = posicion;
            this.PuntoAtencion = PuntoAtencion;
        }
        //para obtener la vision de la camara
        public Matrix getViewMatrix()
        {
            return Matrix.CreateLookAt(posicion, PuntoAtencion, Vector3.Up);
        }
        private const float velocidadMov = 100f;
        public void getInputs(float deltaTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                PuntoAtencion += Vector3.Up * velocidadMov * deltaTime;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                PuntoAtencion += Vector3.Down * velocidadMov * deltaTime;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                PuntoAtencion += Vector3.Left * velocidadMov * deltaTime;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                PuntoAtencion += Vector3.Right * velocidadMov * deltaTime;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                posicion += Vector3.Normalize(PuntoAtencion - posicion) * deltaTime * velocidadMov;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                posicion += Vector3.Normalize(posicion - PuntoAtencion) * deltaTime * velocidadMov;
        }
    }
}
