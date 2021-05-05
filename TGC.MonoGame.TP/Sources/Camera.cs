using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TGC.MonoGame.TP
{
    internal class Camera
    {
        private Vector3 position = new Vector3(0f, 0f, 150f);
        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }

        private const float walkSpeed = 100f;
        private const float runSpeed = walkSpeed * 5;
        private const float mouseSensitivity = 2f;

        private const float fieldOfView = MathHelper.PiOver4;
        private const float nearPlaneDistance = 0.1f;
        private const float farPlaneDistance = 2000f;

        private float pitch, yaw = -90f;
        private Vector3 frontDirection = -Vector3.UnitZ;
        private Vector3 rightDirection = Vector3.Right;
        private Vector3 upDirection = Vector3.Up;

        private Vector2 pastMousePosition;
        private Point screenCenter;

        internal void Initialize(GraphicsDevice graphicsDevice)
        {
            screenCenter = new Point(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            Projection = CreateProjectionMatrix(graphicsDevice);
            View = CreateViewMatrix();
        }

        internal void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ProcessKeyboard(elapsedTime);
            ProcessMouseMovement(elapsedTime);
            View = CreateViewMatrix();
        }

        // Matrix

        private Matrix CreateProjectionMatrix(GraphicsDevice graphicsDevice) => Matrix.CreatePerspectiveFieldOfView(fieldOfView, graphicsDevice.Viewport.AspectRatio, nearPlaneDistance, farPlaneDistance);
        private Matrix CreateViewMatrix() => Matrix.CreateLookAt(position, position + frontDirection, upDirection);

        // Keyboard

        private float MovementSpeed() => Input.Turbo() ? runSpeed : walkSpeed;

        private void ProcessKeyboard(float elapsedTime)
        {
            Vector3 inputDirection = Input.HorizontalAxis() * rightDirection + Input.ForwardAxis() * frontDirection + Input.VerticalAxis() * upDirection;
            bool changed = !Equals(inputDirection, Vector3.Zero);
            position += changed ? Vector3.Normalize(inputDirection) * MovementSpeed() * elapsedTime : Vector3.Zero;
        }

        // Mouse

        private Vector2 CurrentMousePosition() => Mouse.GetState().Position.ToVector2();

        private void ProcessMouseMovement(float elapsedTime)
        {
            Vector2 mouseDelta = (CurrentMousePosition() - pastMousePosition) * mouseSensitivity * elapsedTime;
            yaw += mouseDelta.X;
            pitch -= mouseDelta.Y;
            pitch = Math.Clamp(pitch, -89.9f, 89.9f);

            UpdateCameraVectors();
            Mouse.SetPosition(screenCenter.X, screenCenter.Y);
            pastMousePosition = CurrentMousePosition();
        }

        private void UpdateCameraVectors()
        {
            frontDirection = Vector3.Normalize(new Vector3(
                MathF.Cos(MathHelper.ToRadians(yaw)) * MathF.Cos(MathHelper.ToRadians(pitch)),
                MathF.Sin(MathHelper.ToRadians(pitch)),
                MathF.Sin(MathHelper.ToRadians(yaw)) * MathF.Cos(MathHelper.ToRadians(pitch))
            ));
            rightDirection = Vector3.Normalize(Vector3.Cross(frontDirection, Vector3.Up));
            upDirection = Vector3.Normalize(Vector3.Cross(rightDirection, frontDirection));
        }
    }
}