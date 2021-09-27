using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.MonoGame.Samples.Cameras
{
    /// <summary>
    ///     The minimum behavior that a camera should have.
    /// </summary>
    class FollowCamera
    {
        public const float AxisDistanceToTarget = 25f;
        public const float AngleFollowSpeed = 0.015f;
        public const float AngleThreshold = 0.85f;

        public Matrix Projection { get; private set; }

        public Matrix View { get; private set; }

        private Vector3 CurrentRightVector { get; set; } = Vector3.Right;

        private float RightVectorInterpolator { get; set; } = 0f;

        private Vector3 PastRightVector { get; set; } = Vector3.Right;

        public FollowCamera(float aspectRatio)
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 3f, aspectRatio, 0.1f, 100000f);
        }

        public void Update(GameTime gameTime, Matrix followedMundo)
        {
            //tiempo
            var tiempoPasado = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            //posicion de la matriz de mi mundo
            var followedPosition = followedMundo.Translation;
            var followedRight = followedMundo.Right;
            if (Vector3.Dot(followedRight, PastRightVector) > AngleThreshold)
            {
                // Incremento el Interpolator
                RightVectorInterpolator += tiempoPasado * AngleFollowSpeed;

                // No permito que Interpolator pase de 1
                RightVectorInterpolator = MathF.Min(RightVectorInterpolator, 1f);

                // Calculo el vector Derecha a partir de la interpolacion
                // Esto mueve el vector Derecha para igualar al vector Derecha que sigo
                // En este caso uso la curva x^2 para hacerlo mas suave
                // Interpolator se convertira en 1 eventualmente
                CurrentRightVector = Vector3.Lerp(CurrentRightVector, followedRight, RightVectorInterpolator * RightVectorInterpolator);
            }
            else
                // Si el angulo no pasa de cierto limite, lo pongo de nuevo en cero
                RightVectorInterpolator = 0f;

            PastRightVector = followedRight;
            //Calculo la posion de la camara
            //tomo la posicion de la camara y agrego un offset en eje Y y derecho
            var offsetedPosition = followedPosition + CurrentRightVector * AxisDistanceToTarget + Vector3.Up * AxisDistanceToTarget;
            //cacluclo vector de arriba actualizado
            var forward = (followedPosition - offsetedPosition);
            forward.Normalize();
            var right = Vector3.Cross(forward, Vector3.Up);
            var cameraCorrectUp = Vector3.Cross(right, forward);
            View = Matrix.CreateLookAt(offsetedPosition, followedPosition, cameraCorrectUp);
        }
    }
}