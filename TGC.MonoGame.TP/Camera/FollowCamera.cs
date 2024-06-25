using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    /// <summary>
    /// Una camara que sigue objetos
    /// </summary>
    class FollowCamera
    {
        private const float AxisDistanceToTarget = 5f;

        private const float AngleFollowSpeed = 0.03f;

        private const float AngleThreshold = 0.85f;

        public Matrix Projection { get; private set; }

        public Matrix View { get; private set; }

        private Vector3 CurrentBackwardVector { get; set; } = Vector3.Forward;

        private float BackwardVectorInterpolator { get; set; } = 0f;

        private Vector3 PastBackwardVector { get; set; } = Vector3.Forward;
        //private Effect ballEffect;
        /// <summary>
        /// Crea una FollowCamera que sigue a una matriz de mundo
        /// </summary>
        /// <param name="aspectRatio"></param>
        public Point ScreenCenter;
        public FollowCamera(float aspectRatio, Point HalfSize)
        {
            ScreenCenter=HalfSize;
            // Orthographic camera
            // Projection = Matrix.CreateOrthographic(screenWidth, screenHeight, 0.01f, 10000f);
            //ballEffect = Content.Load<Effect>(ContentFolderEffects + "PBR");
            // Perspective camera
            // Uso 60° como FOV, aspect ratio, pongo las distancias a near plane y far plane en 0.1 y 100000 (mucho) respectivamente
            Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 3f, aspectRatio, 0.1f, 100000f);
        }

        /// <summary>
        /// Actualiza la Camara usando una matriz de mundo actualizada para seguirla
        /// </summary>
        /// <param name="gameTime">The Game Time to calculate framerate-independent movement</param>
        /// <param name="followedWorld">The World matrix to follow</param>

        private Vector2 pastMousePosition=Vector2.Zero;
        private float MouseSensitivity=0.3f;
        public Vector3 CamPosition;
        public float CamRotation=0;
        public bool camMoving=false;

        public void Update(GameTime gameTime, Matrix followedWorld)
        {
            // Obtengo el tiempo
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            
            var mouseState = Mouse.GetState();

            if (true)
            {
                var mouseDelta = mouseState.Position.ToVector2();
                if(!camMoving)
                {
                    Mouse.SetPosition(ScreenCenter.X, ScreenCenter.Y);
                    mouseDelta = new Vector2(0, ScreenCenter.Y);
                    camMoving=true;
                }
                
                mouseDelta *= MouseSensitivity * elapsedTime;
                CamRotation-=mouseDelta.X;
                followedWorld=Matrix.CreateRotationY(CamRotation) * followedWorld;
                //var size = GraphicsDevice.Viewport.Bounds.Size;
                //Mouse.SetPosition(screenCenter.X, screenCenter.Y);
                //Mouse.SetCursor(MouseCursor.Crosshair);
                Mouse.SetPosition(0, ScreenCenter.Y);
                Mouse.SetCursor(MouseCursor.Crosshair); 
                pastMousePosition=mouseState.Position.ToVector2();
            }

            /*else
            {
                Mouse.SetCursor(MouseCursor.Arrow);
                camMoving=false;
                CamRotation=0;
                //Mouse.SetPosition(ScreenCenter.X, ScreenCenter.Y);
            }*/

            // Obtengo la posicion de la matriz de mundo que estoy siguiendo
            var followedPosition = followedWorld.Translation;

            // Obtengo el vector Backward de la matriz de mundo que estoy siguiendo
            var followedBackward = followedWorld.Forward;

            // Si el producto escalar entre el vector Backward anterior
            // y el actual es mas grande que un limite,
            // muevo el Interpolator (desde 0 a 1) mas cerca de 1
            if (Vector3.Dot(followedBackward, PastBackwardVector) > AngleThreshold)
            {
                // Incremento el Interpolator
                BackwardVectorInterpolator += elapsedTime * AngleFollowSpeed;

                // No permito que Interpolator pase de 1
                BackwardVectorInterpolator = MathF.Min(BackwardVectorInterpolator, 1f);

                // Calculo el vector Backward a partir de la interpolacion
                // Esto mueve el vector Backward para igualar al vector Backward que sigo
                // En este caso uso la curva x^2 para hacerlo mas suave
                // Interpolator se convertira en 1 eventualmente
                CurrentBackwardVector = Vector3.Lerp(CurrentBackwardVector, followedBackward, BackwardVectorInterpolator );
            }
            else
                // Si el angulo no pasa de cierto limite, lo pongo de nuevo en cero
                BackwardVectorInterpolator = 0f;

            // Guardo el vector Derecha para usar en la siguiente iteracion
            PastBackwardVector = followedBackward;
            
            // Calculo la posicion del a camara
            // tomo la posicion que estoy siguiendo, agrego un offset en los ejes Y y Derecha
            var offsetedPosition = followedPosition 
                + CurrentBackwardVector * AxisDistanceToTarget
                + Vector3.Up * AxisDistanceToTarget * 5f;

            // Calculo el vector Arriba actualizado
            // Nota: No se puede usar el vector Arriba por defecto (0, 1, 0)
            // Como no es correcto, se calcula con este truco de producto vectorial

            // Calcular el vector Adelante haciendo la resta entre el destino y el origen
            // y luego normalizandolo (Esta operacion es cara!)
            // (La siguiente operacion necesita vectores normalizados)
            var forward = followedPosition - offsetedPosition;
            forward.Normalize();

            // Obtengo el vector Derecha asumiendo que la camara tiene el vector Arriba apuntando hacia arriba
            // y no esta rotada en el eje X (Roll)
            var right = Vector3.Cross(forward, Vector3.Up);

            // Una vez que tengo la correcta direccion Derecha, obtengo la correcta direccion Arriba usando
            // otro producto vectorial
            var cameraCorrectUp = Vector3.Cross(right, forward);

            // Calculo la matriz de Vista de la camara usando la Posicion, La Posicion a donde esta mirando,
            // y su vector Arriba
            CamPosition=offsetedPosition;

            View = Matrix.CreateLookAt(offsetedPosition, followedPosition, cameraCorrectUp);
            
        }
    }
}
