using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.Samples.Cameras
{
    /// <summary>
    ///     Camera looking at a particular point, assumes the up vector is in y.
    /// </summary>
    public class TargetCamera : Camera
    {
        /// <summary>
        ///     The direction that is "up" from the camera's point of view.
        /// </summary>
        public readonly Vector3 DefaultWorldUpVector = Vector3.Up;
        
        public float MovementSpeed { get; set; } = 100f;
        public float MouseSensitivity { get; set; } = 5f;
        private float lastScrollValue = 0;
        public float scrollSensitivity = 100;
        public float maxZoom = 1200;
        public float minZoom = 240;
        public Vector3 OrientationVector { get; set; }
        private float zoom = 400f;
        private float minimumYAngle = 15f;
        private float maximumYAngle = 70f;
        private float currentYangle = 15f;
        private float cameraRotation = 0f;

        /// <summary>
        ///     Camera looking at a particular direction, which has the up vector (0,1,0).
        /// </summary>
        /// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="position">The position of the camera.</param>
        /// <param name="targetPosition">The target towards which the camera is pointing.</param>
        public TargetCamera(float aspectRatio, Vector3 position, Vector3 targetPosition) : base(aspectRatio)
        {
            BuildView(position, targetPosition);
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

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            // This camera has no movement, once initialized with position and lookAt it is no longer updated automatically.
            var elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            ProcessMouseMovement(elapsedTime);
            ProcessKeyboardPresses(elapsedTime);
        }
        
        private void ProcessMouseMovement(float elapsedTime)
        {
            var mouseState = Mouse.GetState();
            if(lastScrollValue == mouseState.ScrollWheelValue) return;
            if (lastScrollValue < mouseState.ScrollWheelValue)
            {
                zoom = MathF.Min(MathF.Max(minZoom,zoom - scrollSensitivity), maxZoom);
            }
            else
            {
                zoom = MathF.Min(MathF.Max(minZoom,zoom + scrollSensitivity), maxZoom);
            }
            lastScrollValue = mouseState.ScrollWheelValue;
            Console.Write(mouseState.ScrollWheelValue + "\n");   

        }
        

        private void ProcessKeyboardPresses(float elapsedTime) {
            var keyboardState = Keyboard.GetState();


            if (keyboardState.IsKeyDown(Keys.Up))
            {
                if(currentYangle <= maximumYAngle){
                    if(currentYangle+0.3f <= maximumYAngle) {
                        currentYangle += 0.3f;
                    }
                    else {
                        currentYangle = maximumYAngle;
                    }
                }
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                if(currentYangle >= minimumYAngle){
                    if(currentYangle-0.3f >= minimumYAngle) {
                        currentYangle -= 0.3f;
                    }
                    else {
                        currentYangle = minimumYAngle;
                    }
                }
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                if(cameraRotation+0.3f > 360){
                    cameraRotation = cameraRotation + 0.3f - 360f;
                }
                else {
                    cameraRotation += 0.3f;
                } 
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                if(cameraRotation-0.3f < 0) {
                    cameraRotation = cameraRotation - 0.3f + 360f;
                }
                else {
                    cameraRotation -= 0.3f;
                } 
            }
        }
        

        public void UpdatePosition(GameTime gameTime, Vector3 objectPosition) 
        {
            TargetPosition = objectPosition;
            var cameraPosition = Position;

            cameraPosition.X = objectPosition.X + zoom * (float)Math.Cos(ConvertToRadians(cameraRotation)) * (float)Math.Sin(ConvertToRadians(90-currentYangle));
            cameraPosition.Y = objectPosition.Y + zoom * (float)Math.Sin(ConvertToRadians(currentYangle));
            cameraPosition.Z = objectPosition.Z + zoom * (float)Math.Sin(ConvertToRadians(cameraRotation)) * (float)Math.Sin(ConvertToRadians(90-currentYangle));
            Position = cameraPosition;
            BuildView();
        }

        public float ConvertToRadians(float angle)
        {
            return (float)(Math.PI / 180) * angle;
        }
    }
}