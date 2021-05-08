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

        public Vector2 delta;
        public float turnSpeed = 60f;
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
        public float MouseSensitivity { get; set; } = 10f;

        private void CalculateView()
        {
            View = Matrix.CreateLookAt(Position, Position + FrontDirection, UpDirection);
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            var elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            changed = false;
            
            delta = Vector2.Zero;
            
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
            var currentTurnSpeed = turnSpeed;
            if (keyboardState.IsKeyDown(Keys.LeftShift))
                currentMovementSpeed *= 5f;

            currentMovementSpeed *= elapsedTime;
            currentTurnSpeed *= elapsedTime;

            if (keyboardState.IsKeyDown(Keys.A) )
            {
                Position += -RightDirection * currentMovementSpeed ;
                changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                Position += RightDirection * currentMovementSpeed;
                changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.W) )
            {
                Position += FrontDirection * currentMovementSpeed;
                changed = true;
            }
            if (keyboardState.IsKeyDown(Keys.S) )
            {
                Position += -FrontDirection * currentMovementSpeed;
                changed = true;
            }
            if (ArrowsLookEnabled)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    Pitch += currentTurnSpeed;
                    delta.Y += currentTurnSpeed;
                    if (Pitch > 89.0f)
                        Pitch = 89.0f;
                    changed = true;
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    Pitch -= currentTurnSpeed;
                    delta.Y -= currentTurnSpeed;
                    if (Pitch < -89.0f)
                        Pitch = -89.0f;

                    changed = true;
                }
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    Yaw -= currentTurnSpeed;
                    delta.X -= currentTurnSpeed;
                    if (Yaw < 0)
                        Yaw += 360;
                    Yaw %= 360;
                    changed = true;
                }
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    Yaw += currentTurnSpeed;
                    delta.X += currentTurnSpeed;
                    Yaw %= 360;
                    changed = true;
                }
            }
            if (changed)
                UpdateCameraVectors();
        }
        
        public Vector2 pastMousePosition;

        float maxMouseDelta = 3;
        private void ProcessMouse(float time)
        {
            
            var mouseState = Mouse.GetState();
            var mousePosition = mouseState.Position.ToVector2();
            var mouseDelta = mousePosition - pastMousePosition;
            mouseDelta *= MouseSensitivity * time;

            //Evito movimientos muy rapidos con mouse
            mouseDelta.X = MathHelper.Clamp(mouseDelta.X, -maxMouseDelta, maxMouseDelta);
            mouseDelta.Y = MathHelper.Clamp(mouseDelta.Y, -maxMouseDelta, maxMouseDelta);

            delta = mouseDelta;
            //System.Diagnostics.Debug.WriteLine("delta " + Math.Round(mouseDelta.X, 2) +"|" + Math.Round(mouseDelta.Y, 2));
            
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