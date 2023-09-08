using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.MonoGame.TP
{
    /// <summary>
    /// Una camara que sigue objetos
    /// </summary>
    class FollowCamera
    {
        private const float AxisDistanceToTarget = 500f;

        private const float AngleFollowSpeed = 0.015f;

        private const float AngleThreshold = 0.85f;

        public Matrix Projection { get; private set; }

        public Matrix View { get; private set; }

        private Vector3 CurrentRightVector { get; set; } = Vector3.Backward;

        private float RightVectorInterpolator { get; set; } = 0f;

        private Vector3 PastRightVector { get; set; } = Vector3.Right;

        public FollowCamera(float aspectRatio)
        {
            // Orthographic camera
            //Projection = Matrix.CreateOrthographic(1920f, 1080f, 0.01f, 10000f);

            // Perspective camera
            // Uso 60° como FOV, aspect ratio, pongo las distancias a near plane y far plane en 0.1 y 100000 (mucho) respectivamente
            Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 3f, aspectRatio, 0.1f, 100000f);
        }

        public void Update(GameTime gameTime, Matrix followedWorld)
        {
            // Por ahora que solo mire el centro del mundo desde arriba
            View = Matrix.CreateLookAt(Vector3.UnitZ * 15, Vector3.Zero, Vector3.Up);
        }
    }
}
