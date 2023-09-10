﻿using Microsoft.Xna.Framework;
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
        private const float AxisDistanceToTarget = 1000f;

        private const float AngleFollowSpeed = 0.015f;

        private const float AngleThreshold = 0.85f;

        public Matrix Projection { get; private set; }

        public Matrix View { get; private set; }

        private Vector3 CurrentRightVector { get; set; } = Vector3.Right;

        private float RightVectorInterpolator { get; set; } = 0f;

        private Vector3 PastRightVector { get; set; } = Vector3.Right;

        /// <summary>
        /// Crea una FollowCamera que sigue a una matriz de mundo
        /// </summary>
        /// <param name="aspectRatio"></param>
        public FollowCamera(float aspectRatio)
        {
            // Orthographic camera
            // Projection = Matrix.CreateOrthographic(screenWidth, screenHeight, 0.01f, 10000f);

            // Perspective camera
            // Uso 60° como FOV, aspect ratio, pongo las distancias a near plane y far plane en 0.1 y 100000 (mucho) respectivamente
            Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 3f, aspectRatio, 0.1f, 100000f);
        }

        /// <summary>
        /// Actualiza la Camara usando una matriz de mundo actualizada para seguirla
        /// </summary>
        /// <param name="gameTime">The Game Time to calculate framerate-independent movement</param>
        /// <param name="followedWorld">The World matrix to follow</param>
        public void Update(GameTime gameTime, Matrix followedWorld)
        {
            // Arriba del tanque "objetivo"
            Vector3 cameraPosition = followedWorld.Translation + Vector3.UnitY * 6000;
            Vector3 cameraLookAt = followedWorld.Translation + followedWorld.Forward * 700;

            View = Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);

            //View = Matrix.CreateLookAt(new Vector3(500f,500f,500f), Vector3.Zero, Vector3.Up);
        }
    }
}