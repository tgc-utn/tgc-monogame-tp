using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.Samples.Cameras
{
    internal class MyCamera : Camera
    {
        
        private readonly Point screenCenter;
        private bool changed;

        public float Pitch;

        // Angles
        public float Yaw = 270f;
        public float turnSpeed = 1f;
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
        public float MouseSensitivity { get; set; } = 5f;

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
            if(keyboardState.IsKeyDown(Keys.Up))
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

            if (changed)
                UpdateCameraVectors();
        }

        private void UpdateCameraVectors()
        {
            // Calculate the new Front vector
            Vector3 tempFront;
            tempFront.X = MathF.Cos(MathHelper.ToRadians(Yaw)) * MathF.Cos(MathHelper.ToRadians(Pitch));
            tempFront.Y = MathF.Sin(MathHelper.ToRadians(Pitch));
            tempFront.Z = MathF.Sin(MathHelper.ToRadians(Yaw)) * MathF.Cos(MathHelper.ToRadians(Pitch));

            FrontDirection = Vector3.Normalize(tempFront);

            // Also re-calculate the Right and Up vector
            // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
            RightDirection = Vector3.Normalize(Vector3.Cross(FrontDirection, Vector3.Up));
            UpDirection = Vector3.Normalize(Vector3.Cross(RightDirection, FrontDirection));
        }
    }
}