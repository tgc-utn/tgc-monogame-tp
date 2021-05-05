using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.Samples.Cameras
{
    internal class MyCamera : Camera
    {
        
        private readonly Point screenCenter;
        private bool changed;
        public bool MouseLookEnabled = false;
        public bool ArrowsLookEnabled = true;
        
        public float Pitch;
        public float Yaw = 270f;
        
        public float turnSpeed = 0.25f;
        public MyCamera(float aspectRatio, Vector3 position, Point screenCenter) : this(aspectRatio, position)
        {
            this.screenCenter = screenCenter;
        }

        public MyCamera(float aspectRatio, Vector3 position) : base(aspectRatio)
        {
            Position = position;
            UpdateCameraVectors();
            CalculateView();
        }

        public float MovementSpeed { get; set; } = 80f;
        public float MouseSensitivity { get; set; } = 20f;

        private void CalculateView()
        {
            View = Matrix.CreateLookAt(Position, Position + FrontDirection, UpDirection);
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            var elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            changed = false;
            ProcessKeyboard(elapsedTime);
            if(MouseLookEnabled)
                ProcessMouse(elapsedTime);
            if (changed)
                CalculateView();
        }
        
        private void ProcessKeyboard(float elapsedTime)
        {
            var keyboardState = Keyboard.GetState();

            var currentMovementSpeed = MovementSpeed;
            if (keyboardState.IsKeyDown(Keys.LeftShift))
                currentMovementSpeed *= 5f;
            
            if (keyboardState.IsKeyDown(Keys.A) )
            {
                Position += -RightDirection * currentMovementSpeed * elapsedTime;
                changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                Position += RightDirection * currentMovementSpeed * elapsedTime;
                changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.W) )
            {
                Position += FrontDirection * currentMovementSpeed * elapsedTime;
                changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.S) )
            {
                Position += -FrontDirection * currentMovementSpeed * elapsedTime;
                changed = true;
            }
            if (ArrowsLookEnabled)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    Pitch += turnSpeed;
                    if (Pitch > 89.0f)
                        Pitch = 89.0f;
                    changed = true;
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    Pitch -= turnSpeed;
                    if (Pitch < -89.0f)
                        Pitch = -89.0f;

                    changed = true;
                }
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    Yaw -= turnSpeed;
                    if (Yaw < 0)
                        Yaw += 360;
                    Yaw %= 360;
                    changed = true;
                }
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    Yaw += turnSpeed;
                    Yaw %= 360;
                    changed = true;
                }
            }
            if (changed)
                UpdateCameraVectors();
        }
        
        public Vector2 pastMousePosition;

        private void ProcessMouse(float time)
        {
            
            var mouseState = Mouse.GetState();
            var mousePosition = mouseState.Position.ToVector2();
            var mouseDelta = mousePosition - pastMousePosition;
            mouseDelta *= MouseSensitivity * time;

            Yaw += mouseDelta.X;
            if (Yaw < 0)
                Yaw += 360;
            Yaw %= 360;

            Pitch -= mouseDelta.Y;

            if (Pitch > 89.0f)
                Pitch = 89.0f;
            if (Pitch < -89.0f)
                Pitch = -89.0f;

            changed = true;
            UpdateCameraVectors();
            
            Mouse.SetPosition(screenCenter.X, screenCenter.Y);
            pastMousePosition = Mouse.GetState().Position.ToVector2();
            

        }

        private void UpdateCameraVectors()
        {
            // Calculate the new Front vector
            Vector3 tempFront;
            tempFront.X = MathF.Cos(MathHelper.ToRadians(Yaw)) * MathF.Cos(MathHelper.ToRadians(Pitch));
            tempFront.Y = MathF.Sin(MathHelper.ToRadians(Pitch));
            tempFront.Z = MathF.Sin(MathHelper.ToRadians(Yaw)) * MathF.Cos(MathHelper.ToRadians(Pitch));

            FrontDirection = Vector3.Normalize(tempFront);

            RightDirection = (Vector3.Cross(FrontDirection, Vector3.Up));
            UpDirection = (Vector3.Cross(RightDirection, FrontDirection));
        }
    }
}