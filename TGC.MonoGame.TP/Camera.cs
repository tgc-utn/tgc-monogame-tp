using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TGC.MonoGame.TP
{
    class Camera
    {
        private Vector3 position = new Vector3(0f, 0f, 150f);
        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }

        private const float walkSpeed = 100f;
        private const float runSpeed = walkSpeed * 5;
        private const float mouseSensitivity = 5f;

        private const float fieldOfView = MathHelper.PiOver4;
        private const float nearPlaneDistance = 0.1f;
        private const float farPlaneDistance = 2000f;

        private float pitch;
        private float yaw = -90f;
        private Vector3 frontDirection = -Vector3.UnitZ;
        private Vector3 rightDirection = Vector3.Right;
        private Vector3 upDirection = Vector3.Up;

        private bool changed;
        private Vector2 pastMousePosition;
        private Point screenCenter;

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            screenCenter = new Point(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, graphicsDevice.Viewport.AspectRatio, nearPlaneDistance, farPlaneDistance);
            UpdateViewMatrix();
        }

        public void Update(GameTime gameTime)
        {
            var elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            ProcessKeyboard(elapsedTime);
            ProcessMouseMovement(elapsedTime);
            UpdateViewMatrix();
        }

        private void UpdateViewMatrix()
        {
            if (changed)
            {
                View = Matrix.CreateLookAt(position, position + frontDirection, upDirection);
                changed = false;
            }
        }

        private void ProcessKeyboard(float elapsedTime)
        {
            var keyboardState = Keyboard.GetState();
            var movementSpeed = keyboardState.IsKeyDown(Keys.LeftShift) ? runSpeed : walkSpeed;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                position += -rightDirection * movementSpeed * elapsedTime;
                changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                position += rightDirection * movementSpeed * elapsedTime;
                changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                position += frontDirection * movementSpeed * elapsedTime;
                changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                position += -frontDirection * movementSpeed * elapsedTime;
                changed = true;
            }
        }

        private void ProcessMouseMovement(float elapsedTime)
        {
            var mouseState = Mouse.GetState();

            Mouse.SetCursor(MouseCursor.No);
                var mouseDelta = mouseState.Position.ToVector2() - pastMousePosition;
                mouseDelta *= mouseSensitivity * elapsedTime;

                yaw += mouseDelta.X;
                pitch -= mouseDelta.Y;

                if (pitch > 89.0f)
                    pitch = 89.0f;
                if (pitch < -89.0f)
                    pitch = -89.0f;

                changed = true;
                UpdateCameraVectors();

            Mouse.SetPosition(screenCenter.X, screenCenter.Y);

            pastMousePosition = Mouse.GetState().Position.ToVector2();
        }

        private void UpdateCameraVectors()
        {
            // Calculate the new Front vector
            Vector3 tempFront;
            tempFront.X = MathF.Cos(MathHelper.ToRadians(yaw)) * MathF.Cos(MathHelper.ToRadians(pitch));
            tempFront.Y = MathF.Sin(MathHelper.ToRadians(pitch));
            tempFront.Z = MathF.Sin(MathHelper.ToRadians(yaw)) * MathF.Cos(MathHelper.ToRadians(pitch));

            frontDirection = Vector3.Normalize(tempFront);

            // Also re-calculate the Right and Up vector
            // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
            rightDirection = Vector3.Normalize(Vector3.Cross(frontDirection, Vector3.Up));
            upDirection = Vector3.Normalize(Vector3.Cross(rightDirection, frontDirection));
        }
    }
}