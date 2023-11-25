using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Configuration;

namespace MonoGamers.Camera;

    /// <summary>
    ///     Camera looking at a particular point, assumes the up vector is in y.
    /// </summary>
    public class TargetCamera : Camera
    {
        /// <summary>
        ///     The direction that is "up" from the camera's point of view.
        /// </summary>
        public readonly Vector3 DefaultWorldUpVector = Vector3.Up;
        private const float CameraFollowRadius = 130f;
        private const float CameraUpDistance = 70f;
        private const float CameraRotatingVelocity = 0.06f;

        private Viewport Viewport;

        public Matrix CameraRotation { get; set; }
        private float Rotation { get; set; }
        private Vector2 PastMousePosition { get; set; }
        private Vector2 CenterMousePosition { get; set; }

        private bool Rotated { get; set; }
    /// <summary>
    ///     Camera looking at a particular direction, which has the up vector (0,1,0).
    /// </summary>
    /// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height.</param>
    /// <param name="position">The position of the camera.</param>
    /// <param name="targetPosition">The target towards which the camera is pointing.</param>
    public TargetCamera(float aspectRatio, Vector3 position, Vector3 targetPosition, Viewport viewport) : base(aspectRatio)
        {
            BuildView(position, targetPosition);
            PastMousePosition = new Vector2(viewport.Width / 2, viewport.Height / 2);
            CenterMousePosition = new Vector2(viewport.Width / 2, viewport.Height / 2);
            CameraRotation = Matrix.Identity;
            Viewport = viewport;
    }

        /// <summary>
        ///     Camera looking at a particular direction, which has the up vector (0,1,0).
        /// </summary>
        /// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="position">The position of the camera.</param>
        /// <param name="targetPosition">The target towards which the camera is pointing.</param>
        /// <param name="nearPlaneDistance">Distance to the near view plane.</param>
        /// <param name="farPlaneDistance">Distance to the far view plane.</param>
        public TargetCamera(float aspectRatio, Vector3 position, Vector3 targetPosition, float nearPlaneDistance,
            float farPlaneDistance) : base(aspectRatio, nearPlaneDistance, farPlaneDistance)
        {
            BuildView(position, targetPosition);
        }

        /// <summary>
        ///     The target towards which the camera is pointing.
        /// </summary>
        public Vector3 TargetPosition { get; set; }

        /// <summary>
        ///     Build view matrix and update the internal directions.
        /// </summary>
        /// <param name="position">The position of the camera.</param>
        /// <param name="targetPosition">The target towards which the camera is pointing.</param>
        private void BuildView(Vector3 position, Vector3 targetPosition)
        {
            Position = position;
            TargetPosition = targetPosition;
            BuildView();
        }

        /// <summary>
        ///     Build view matrix and update the internal directions.
        /// </summary>
        public void BuildView()
        {
            FrontDirection = Vector3.Normalize(TargetPosition - Position);
            RightDirection = Vector3.Normalize(Vector3.Cross(DefaultWorldUpVector, FrontDirection));
            UpDirection = Vector3.Cross(FrontDirection, RightDirection);
            View = Matrix.CreateLookAt(Position, Position + FrontDirection, UpDirection);
        }

    public override void Update(GameTime gameTime)
    {
        throw new System.NotImplementedException();
    }
    /// <inheritdoc />
    public  void UpdateCamera(GameTime gameTime, Vector3 SpherePosition, bool onMenu)
        {
        // Create a position that orbits the Sphere by its direction (Rotation)

        if(!onMenu) ProcessMouseMovement((float) gameTime.ElapsedGameTime.TotalSeconds);

         var sphereBack = Vector3.Transform(Vector3.Forward, CameraRotation);
         // Then scale the vector by a radius, to set an horizontal distance between the Camera and the Sphere
         var orbitalPosition = sphereBack * CameraFollowRadius;


         // We will move the Camera in the Y axis by a given distance, relative to the Sphere
         var upDistance = Vector3.Up * CameraUpDistance;

         // Calculate the new Camera Position by using the Sphere Position, then adding the vector orbitalPosition that sends 
         // the camera further in the back of the Sphere, and then we move it up by a given distance
         Position = SpherePosition + orbitalPosition + upDistance;

         // Set our Target as the Sphere, the Camera needs to be always pointing to it
        
        TargetPosition = SpherePosition;

        // Build our View matrix from the Position and TargetPosition
        BuildView();
    }

    private void ProcessMouseMovement(float elapsedTime)
    {

        var mouseState = Mouse.GetState();
        
        float deltaX = mouseState.X - PastMousePosition.X;
        if (deltaX > 0.001f || deltaX < 0.001f)
        {
            CameraRotation *= Matrix.CreateRotationY(deltaX*-0.2f*elapsedTime);
        }

        if (mouseState.X < 0 || mouseState.X > Viewport.Width ||
            mouseState.Y < 0 || mouseState.Y > Viewport.Height)
        {
            Mouse.SetPosition((int)CenterMousePosition.X, (int)CenterMousePosition.Y);
            PastMousePosition = CenterMousePosition;
        }
        else
        {
            PastMousePosition = mouseState.Position.ToVector2();
        }

        
    }
}