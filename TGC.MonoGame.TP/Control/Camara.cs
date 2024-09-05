using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
//como solo tendremos una projeccion presumo solo habra una camara, pero despues podemos cambiarlo,
//si quieren hacer algo como tener dos camara, una mas cerca del auto o algo parecido
namespace Control
{
    class Camara
    {
        public Vector3 posicion;
        //a si a donde este mirando la camara
        public Vector3 PuntoAtencion; 
        
        public Camara(Vector3 posicion, Vector3 PuntoAtencion)
        {
            this.posicion = posicion;
            this.PuntoAtencion = PuntoAtencion;
        }
        //para obtener la vision de la camara
        public Matrix getViewMatrix()
        {
            return Matrix.CreateLookAt(posicion, PuntoAtencion, Vector3.Up);
        }
    }
    /*
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
                PuntoAtencion = Vector3.Transform(PuntoAtencion - posicion, Matrix.CreateRotationY((3.141592654f / 6f)* deltaTime)) + posicion;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                PuntoAtencion = Vector3.Transform(PuntoAtencion - posicion, Matrix.CreateRotationY((-3.141592654f / 6f)* deltaTime)) + posicion;

            Vector3 movimiento = Vector3.Normalize(PuntoAtencion - posicion); 
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                PuntoAtencion += movimiento * deltaTime * velocidadMov;
                posicion += movimiento * deltaTime * velocidadMov;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                PuntoAtencion -= movimiento * deltaTime * velocidadMov;
                posicion -= movimiento * deltaTime * velocidadMov;
            }
        }
    }
    */
}
