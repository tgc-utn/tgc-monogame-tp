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
        private float minimumYAngle = 0f;
        private float maximumYAngle = 70f;
        private float currentYangle = 15f;
        private float cameraRotation = 0f;
        private Point screenCenter;
        private float windowHeight;
        private float windowWidth;

        /// <summary>
        ///     Camera looking at a particular direction, which has the up vector (0,1,0).
        /// </summary>
        /// <param name="aspectRatio">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="position">The position of the camera.</param>
        /// <param name="targetPosition">The target towards which the camera is pointing.</param>
        public TargetCamera(float aspectRatio, Vector3 position, Vector3 targetPosition, Point ScreenCenter, float WindowHeight, float WindowWidth) : base(aspectRatio)
        {
            this.screenCenter = ScreenCenter;
            BuildView(position, targetPosition);
            this.windowHeight = WindowHeight;
            this.windowWidth = WindowWidth;
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

        public override void SetPosition(Vector3 position)
        {
            TargetPosition = position;
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            // This camera has no movement, once initialized with position and lookAt it is no longer updated automatically.
            var elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            ProcessMouseMovement(elapsedTime);
            ProcessKeyboardPresses(elapsedTime);
            UpdatePosition(gameTime, TargetPosition);
        }
        
        private void ProcessMouseMovement(float elapsedTime)
        {
            var mouseState = Mouse.GetState();
            if (lastScrollValue != mouseState.ScrollWheelValue){
                if (lastScrollValue < mouseState.ScrollWheelValue)
                {
                    zoom = MathF.Min(MathF.Max(minZoom,zoom - scrollSensitivity), maxZoom);
                } 
                else
                {
                    zoom = MathF.Min(MathF.Max(minZoom,zoom + scrollSensitivity), maxZoom);
                }
            }
            lastScrollValue = mouseState.ScrollWheelValue;
            Point mousePosition = mouseState.Position;
            //Define el rectangulo donde el mouse puede moverse sin afectar la camara
            float minimumYAxisVoid = screenCenter.Y - windowHeight * 20f/100f;
            float maximumYAxisVoid = screenCenter.Y + windowHeight * 20f/100f;
            float minimumXAxisVoid = screenCenter.X - windowWidth * 20f/100f;
            float maximumXAxisVoid = screenCenter.X + windowWidth * 20f/100f;
            //Definoel rectangulo donde fuera del mismo el mouse movera la camara a maxima velocidad
            float minMaxSpeedYAxis = screenCenter.Y - windowHeight * 35f/100f;
            float maxMaxSpeedYAxis = screenCenter.Y + windowHeight * 35f/100f;
            float minMaxSpeedXAxis = screenCenter.X - windowWidth * 35f/100f;
            float maxMaxSpeedXAxis = screenCenter.X + windowWidth * 35f/100f;
            //Regulacion en X
            if(mousePosition.X > maximumXAxisVoid && mousePosition.X < maxMaxSpeedXAxis) {
                float speedMultiplierX = (mousePosition.X-maximumXAxisVoid)/(maxMaxSpeedXAxis-maximumXAxisVoid);
                if(cameraRotation+(0.3f*speedMultiplierX) > 360){
                    cameraRotation = cameraRotation + (0.3f*speedMultiplierX) - 360f;
                }
                else {
                    cameraRotation += (0.3f*speedMultiplierX);
                }
            }
            else if (mousePosition.X > maxMaxSpeedXAxis) {
                if(cameraRotation+0.3f > 360){
                    cameraRotation = cameraRotation + 0.3f - 360f;
                }
                else {
                    cameraRotation += 0.3f;
                }
            }
            else if (mousePosition.X < minimumXAxisVoid && mousePosition.X > minMaxSpeedXAxis ) {
                float speedMultiplierX = (mousePosition.X-minimumXAxisVoid)/(minMaxSpeedXAxis-minimumXAxisVoid);
                if(cameraRotation-(0.3f*speedMultiplierX) < 0) {
                    cameraRotation = cameraRotation - (0.3f*speedMultiplierX) + 360f;
                }
                else {
                    cameraRotation -= (0.3f*speedMultiplierX);
                }  
            }
            else if(mousePosition.X < minMaxSpeedXAxis ) { 
                if(cameraRotation-0.3f < 0) {
                    cameraRotation = cameraRotation - 0.3f + 360f;
                }
                else {
                    cameraRotation -= 0.3f;
                } 
            }
            // Regulacion en Y
            if(mousePosition.Y > maximumYAxisVoid && mousePosition.Y < maxMaxSpeedYAxis) {
                float speedMultiplierY = (mousePosition.Y-maximumYAxisVoid)/(maxMaxSpeedYAxis-maximumYAxisVoid);
                if(currentYangle <= maximumYAngle){
                    if(currentYangle+(0.3f*speedMultiplierY) <= maximumYAngle) {
                        currentYangle += (0.3f*speedMultiplierY);
                    }
                    else {
                        currentYangle = maximumYAngle;
                    }
                }
            }
            else if (mousePosition.Y > maxMaxSpeedYAxis) {
                if(currentYangle <= maximumYAngle){
                    if(currentYangle+0.3f <= maximumYAngle) {
                        currentYangle += 0.3f;
                    }
                    else {
                        currentYangle = maximumYAngle;
                    }
                }
            }
            else if (mousePosition.Y < minimumYAxisVoid && mousePosition.Y > minMaxSpeedYAxis ) {
                float speedMultiplierY = (mousePosition.Y-minimumYAxisVoid)/(minMaxSpeedYAxis-minimumYAxisVoid);
                if(currentYangle >= minimumYAngle){
                    if(currentYangle-(0.3f*speedMultiplierY) >= minimumYAngle) {
                        currentYangle -= (0.3f*speedMultiplierY);
                    }
                    else {
                        currentYangle = minimumYAngle;
                    }
                }          
            }
            else if(mousePosition.Y < minMaxSpeedYAxis ) {
                if(currentYangle >= minimumYAngle){
                    if(currentYangle-0.3f >= minimumYAngle) {
                        currentYangle -= 0.3f;
                    }
                    else {
                        currentYangle = minimumYAngle;
                    }
                }
            }
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
            cameraPosition.Y = objectPosition.Y + 200 + zoom * (float)Math.Sin(ConvertToRadians(currentYangle));
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