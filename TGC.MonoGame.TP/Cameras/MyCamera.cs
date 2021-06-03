using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class MyCamera : Camera
    {

        private readonly Point screenCenter;
        public bool MouseLookEnabled = true;
        public bool ArrowsLookEnabled = true;

        public float Pitch;
        public float Yaw = 90f;

        public Vector2 delta;
        public float turnSpeed = 60f;

        public float MapLimit;
        public int MapSize;
        public float BlockSize;
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

        float CurrentMovementSpeed, CurrentTurnSpeed, SpeedMultiplier = 1f;
        private void CalculateView()
        {
            View = Matrix.CreateLookAt(Position, Position + FrontDirection, UpDirection);
        }
        float PrevPitch, PrevYaw;
        Vector3 PrevPosition;
        public void SaveCurrentState()
        {
            PrevPitch = Pitch;
            PrevYaw = Yaw;
            PrevPosition = Position;
        }
        public void SoftReset()
        {
            Pitch = PrevPitch;
            Yaw = PrevYaw;
            Position = PrevPosition;
        }
        public void Reset()
        {
            Pitch = 0f;
            Yaw = 90f;
            Position = new Vector3(MapLimit / 2 - BlockSize / 2, 0, BlockSize / 2);
        }
        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            CurrentMovementSpeed = MovementSpeed * elapsedTime * SpeedMultiplier;
            CurrentTurnSpeed = turnSpeed * elapsedTime;

            Position += FrontDirection * CurrentMovementSpeed;

            UpdateCameraVectors();
            CalculateView();
        }
        public void UpdateVectorView()
        {
            UpdateCameraVectors();
            CalculateView();
        }
        public void ProcessKeyboard(Xwing xwing)
        {
            //var changed;
            var keyboardState = Keyboard.GetState();
            SpeedMultiplier = 1f;
            //xwing.Boosting = false;
            if (keyboardState.IsKeyDown(Keys.W))
            {
                if (xwing.Energy > 0 && !xwing.BoostLock)
                {
                    xwing.Boosting = true;
                    SpeedMultiplier = 3f;
                }
                else if (xwing.Energy == 0 && xwing.Boosting)
                {
                    xwing.BoostLock = true;
                    xwing.Boosting = false;
                }
                else if(xwing.Energy >= 3 && xwing.BoostLock)
                {
                    xwing.BoostLock = false;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                xwing.Boosting = false;
                SpeedMultiplier = 0.5f;
            }
            else
            {
                xwing.Boosting = false;
            }
            //Free cam for debug
            //if (keyboardState.IsKeyDown(Keys.LeftShift))
            //    CurrentMovementSpeed *= 10f;

            //if (keyboardState.IsKeyDown(Keys.A))
            //{
            //    Position += -RightDirection * CurrentMovementSpeed;
            //    changed = true;
            //}
            //if (keyboardState.IsKeyDown(Keys.D))
            //{
            //    Position += RightDirection * CurrentMovementSpeed;
            //    changed = true;
            //}
            //if (keyboardState.IsKeyDown(Keys.W))
            //{
            //    Position += FrontDirection * CurrentMovementSpeed;
            //    changed = true;
            //}
            //if (keyboardState.IsKeyDown(Keys.S))
            //{
            //    Position += -FrontDirection * CurrentMovementSpeed;
            //    changed = true;
            //}
            if (ArrowsLookEnabled)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    Pitch += CurrentTurnSpeed;
                    delta.Y = CurrentTurnSpeed;
                    if (Pitch > 89.0f)
                        Pitch = 89.0f;
                    //changed = true;
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    Pitch -= CurrentTurnSpeed;
                    delta.Y = -CurrentTurnSpeed;
                    if (Pitch < -89.0f)
                        Pitch = -89.0f;

                    //changed = true;
                }
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    Yaw -= CurrentTurnSpeed;
                    delta.X = -CurrentTurnSpeed;
                    if (Yaw < 0)
                        Yaw += 360;
                    Yaw %= 360;
                    //changed = true;
                }
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    Yaw += CurrentTurnSpeed;
                    delta.X = CurrentTurnSpeed;
                    Yaw %= 360;
                    //changed = true;
                }
                xwing.updateRoll(delta);
            }
            //debug
            //return changed;

        }

        public Vector2 pastMousePosition;

        float maxMouseDelta = 3;
        public void ProcessMouse(Xwing Xwing)
        {

            var mouseState = Mouse.GetState();
            //var mousePosition = mouseState.Position.ToVector2();
            //var mouseDelta = mousePosition - pastMousePosition;
            var mouseDelta = (mouseState.Position - screenCenter).ToVector2();
            Mouse.SetPosition(screenCenter.X, screenCenter.Y);
            //System.Diagnostics.Debug.WriteLine(mouseState.ScrollWheelValue);
            mouseDelta *= MouseSensitivity * 0.010f;

            //System.Diagnostics.Debug.WriteLine(time);
            //Evito movimientos muy rapidos con mouse
            //System.Diagnostics.Debug.WriteLine(mouseDelta.X + " " + mouseDelta.Y);
            mouseDelta.X = MathHelper.Clamp(mouseDelta.X, -maxMouseDelta, maxMouseDelta);
            mouseDelta.Y = MathHelper.Clamp(mouseDelta.Y, -maxMouseDelta, maxMouseDelta);

            //if (mouseDelta == Vector2.Zero)
            //    return;
            TGCGame.MutexDeltas.WaitOne();
            delta = mouseDelta;
            TGCGame.MutexDeltas.ReleaseMutex();
            //System.Diagnostics.Debug.WriteLine("delta " + Math.Round(mouseDelta.X, 2) +"|" + Math.Round(mouseDelta.Y, 2));

            Xwing.updateRoll(delta);

            Yaw += mouseDelta.X;
            if (Yaw < 0)
                Yaw += 360;
            Yaw %= 360;

            Pitch -= mouseDelta.Y;

            if (Pitch > 89.0f)
                Pitch = 89.0f;
            if (Pitch < -89.0f)
                Pitch = -89.0f;

            //changed = true;
            UpdateCameraVectors();

            //pastMousePosition = Mouse.GetState().Position.ToVector2();


        }
        private void UpdateCameraVectors()
        {
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