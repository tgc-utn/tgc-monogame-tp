using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.Samples.Cameras
{
    public class MyCamera : Camera
    {

        private readonly Point screenCenter;
        private bool changed;
        public bool MouseLookEnabled = true;
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

        float CurrentMovementSpeed, CurrentTurnSpeed;
        private void CalculateView()
        {
            View = Matrix.CreateLookAt(Position, Position + FrontDirection, UpDirection);
        }


        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            changed = false;

            delta = Vector2.Zero;

            CurrentMovementSpeed = MovementSpeed * elapsedTime;
            CurrentTurnSpeed = turnSpeed * elapsedTime;


            //bool changed = ProcessKeyboard();
            ProcessKeyboard();
            //muevo la camara siempre para adelante
            Position += FrontDirection * CurrentMovementSpeed;

            if (MouseLookEnabled)
                ProcessMouse(elapsedTime);

            //if (changed)
            //{
                UpdateCameraVectors();
                CalculateView();
            //}
        }
        
        private void ProcessKeyboard()
        {
            //var changed;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.W))
                CurrentMovementSpeed *= 2f;
            if (keyboardState.IsKeyDown(Keys.S))
                CurrentMovementSpeed *= 0.5f;


            //Free cam for debug
            //if (keyboardState.IsKeyDown(Keys.A) )
            //{
            //    Position += -RightDirection * currentMovementSpeed ;
            //    changed = true;
            //}
            //if (keyboardState.IsKeyDown(Keys.D))
            //{
            //    Position += RightDirection * currentMovementSpeed;
            //    changed = true;
            //}
            //if (keyboardState.IsKeyDown(Keys.W) )
            //{
            //    Position += FrontDirection * currentMovementSpeed;
            //    changed = true;
            //}
            //if (keyboardState.IsKeyDown(Keys.S) )
            //{
            //    Position += -FrontDirection * currentMovementSpeed;
            //    changed = true;
            //}



            if (ArrowsLookEnabled)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    Pitch += CurrentTurnSpeed;
                    delta.Y += CurrentTurnSpeed;
                    if (Pitch > 89.0f)
                        Pitch = 89.0f;
                    //changed = true;
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    Pitch -= CurrentTurnSpeed;
                    delta.Y -= CurrentTurnSpeed;
                    if (Pitch < -89.0f)
                        Pitch = -89.0f;

                    //changed = true;
                }
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    Yaw -= CurrentTurnSpeed;
                    delta.X -= CurrentTurnSpeed;
                    if (Yaw < 0)
                        Yaw += 360;
                    Yaw %= 360;
                    //changed = true;
                }
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    Yaw += CurrentTurnSpeed;
                    delta.X += CurrentTurnSpeed;
                    Yaw %= 360;
                    //changed = true;
                }
            }
            //debug
            //return changed;

        }
        
        public Vector2 pastMousePosition;

        float maxMouseDelta = 3;
        private void ProcessMouse(float time)
        {

            var mouseState = Mouse.GetState();
            var mousePosition = mouseState.Position.ToVector2();
            var mouseDelta = mousePosition - pastMousePosition;
            mouseDelta *= MouseSensitivity * time;
            //System.Diagnostics.Debug.WriteLine(time);
            //Evito movimientos muy rapidos con mouse
            System.Diagnostics.Debug.WriteLine(mouseDelta.X + " " + mouseDelta.Y);
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

            RightDirection = Vector3.Normalize(Vector3.Cross(FrontDirection, Vector3.Up));
            UpDirection = Vector3.Normalize(Vector3.Cross(RightDirection, FrontDirection));
        }
    }
}