using Microsoft.Xna.Framework;
using System;

namespace TGC.MonoGame.TP.Camaras
{
    public class FollowCamera
    {
        private const float DistanceToTarget = 20f;

        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }
        public Vector3 Position { get; private set; }

        public FollowCamera(float aspectRatio)
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 3f, aspectRatio, 0.1f, 100000f);
        }
        public void Update(GameTime gameTime, Matrix followedWorld)
        {
            var followedPosition = followedWorld.Translation;

            var offsetX = DistanceToTarget * MathF.Cos(MathF.PI / 4f);
            var offsetY = DistanceToTarget;
            var offsetZ = DistanceToTarget * MathF.Sin(MathF.PI / 4f);

            var cameraPosition = followedPosition + new Vector3(offsetX, offsetY, offsetZ);
            Position = cameraPosition;

            var forward = followedPosition - cameraPosition;
            forward.Normalize();
            var right = Vector3.Cross(forward, Vector3.Up);

            var cameraCorrectUp = Vector3.Cross(right, forward);

            View = Matrix.CreateLookAt(cameraPosition, followedPosition, cameraCorrectUp);
        }
    }
}