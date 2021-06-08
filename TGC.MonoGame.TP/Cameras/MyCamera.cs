using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

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
        public float MovementSpeed { get; set; } = 80f;
        public float MouseSensitivity { get; set; } = 10f;
        float CurrentMovementSpeed, CurrentTurnSpeed, SpeedMultiplier = 1f;

        bool debugging = false;

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

        private void CalculateView()
        {
            View = Matrix.CreateLookAt(Position, Position + FrontDirection, UpDirection);
        }
        float PrevPitch, PrevYaw;
        Vector3 PrevPosition;
        bool PauseRotationDir;
        public void SaveCurrentState()
        {
            PrevPitch = Pitch;
            PrevYaw = Yaw;
            PrevPosition = Position;

            PauseRotationDir = new Random().Next(0, 2) == 0;
            PauseRotation = MathHelper.ToRadians(Yaw) + MathHelper.Pi;
            
        }

        float previousSpeedMul;
        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var Game = TGCGame.Instance;
            var lookBackCamera = Game.LookBack;
            CurrentMovementSpeed = MovementSpeed * elapsedTime * SpeedMultiplier;
            
            if (previousSpeedMul != SpeedMultiplier)
            {
                FieldOfView = SpeedMultiplier * 0.05f + DefaultFieldOfViewDegrees;
                
                //Game.Xwing.distanceToCamera = 40 - SpeedMultiplier * 1.6f;
                UpdateProjection();
                lookBackCamera.FieldOfView = FieldOfView;
                lookBackCamera.UpdateProjection();
            }
            previousSpeedMul = SpeedMultiplier;

            CurrentTurnSpeed = turnSpeed * elapsedTime;
            //continously moving forward
            if(!debugging)
                Position += FrontDirection * CurrentMovementSpeed;

            UpdateCameraVectors();
            CalculateView();

            lookBackCamera.Position = Position + FrontDirection * 80;
            lookBackCamera.FrontDirection = -FrontDirection;
            lookBackCamera.CalculateView();

        }
        public bool Resuming = false;
        float PauseRotation = 0f;
        bool yawCorrectionDir = false;
        public void PausedUpdate(float elapsedTime, Xwing xwing)
        {
            float y = 0f;
            if (!Resuming)
            {
                if (PauseRotationDir)
                {
                    PauseRotation += elapsedTime * 0.5f;
                    PauseRotation %= MathHelper.TwoPi;
                }
                else
                {
                    PauseRotation -= elapsedTime * 0.5f;
                    if (PauseRotation < 0)
                        PauseRotation += MathHelper.TwoPi;
                }
                

                if (xwing.Position.Y > 0)
                {
                    y = xwing.Position.Y + 8;
                    Pitch = 0f;
                }
                else
                {
                    y = 30f;
                    Pitch = -15f;
                }

                Position = new Vector3(
                    xwing.Position.X + 100f * MathF.Cos(PauseRotation),
                    y,
                    xwing.Position.Z + 100f * MathF.Sin(PauseRotation));

                Vector3 frontDirection = Vector3.Normalize(xwing.Position - Position);

                Yaw = MathHelper.ToDegrees(MathF.Atan2(frontDirection.Z, frontDirection.X));
            }
            else
            {
                var posDif = PrevPosition - Position;
                var len = posDif.Length();
                var dir = Vector3.Normalize(posDif);

                var restoredPos = len < 2f;
                var restoredYaw = Yaw > PrevYaw - 3f && Yaw < PrevYaw + 3f;
                var restoredPitch = Pitch > PrevPitch - 2f && Pitch < PrevPitch + 2f;

                
                if (!restoredPos)
                    Position += dir * elapsedTime * len * 5f;
                if (!restoredYaw)
                    if (!yawCorrectionDir)
                        Yaw += elapsedTime * yawDelta * 1.5f;
                    else
                    {
                        Yaw -= elapsedTime * yawDelta * 1.5f;
                        if (Yaw < 0)
                            Yaw += 360;
                    }
               if (!restoredPitch)
                    Pitch += elapsedTime * pitchDelta *1.5f;
                
                if(restoredPos && restoredYaw && restoredPitch)
                    restorePreviousPitchYawPos();
            }

            UpdateVectorView();
        }
        public void UpdateVectorView()
        {
            UpdateCameraVectors();
            CalculateView();
        }
        float yawDelta, pitchDelta;
        public void SoftReset()
        {
            Resuming = true;
            pitchDelta = PrevPitch - Pitch;
            
            yawDelta = PrevYaw - Yaw;
            //Debug.WriteLine("PY " + PrevYaw + " AY " + Yaw + " D " + yawDelta);
            
            yawCorrectionDir = yawDelta >= 180;

            //Debug.WriteLine("Corrected? PY " + PrevYaw + " AY " + Yaw + " D " + yawDelta);
        }
        public void restorePreviousPitchYawPos()
        {
            Pitch = PrevPitch;
            Yaw = PrevYaw;
            Position = PrevPosition;
            Resuming = false;
            TGCGame.Instance.GameState = TGCGame.GmState.Running;
        }
        public void Reset()
        {
            Pitch = 0f;
            Yaw = 90f;
            Position = new Vector3(MapLimit / 2 - BlockSize / 2, 0, BlockSize / 2);
        }
        public void ProcessKeyboard(Xwing xwing)
        {
            var keyboardState = Keyboard.GetState();
            SpeedMultiplier = 1f;
            if (!debugging)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    if (xwing.Energy > 0 && !xwing.BoostLock)
                    {
                        xwing.Boosting = true;
                        SpeedMultiplier = 1f + 6f * xwing.boostTime;
                        //Debug.WriteLine(SpeedMultiplier);
                    }
                    else if (xwing.Energy == 0 && xwing.Boosting)
                    {
                        xwing.BoostLock = true;
                        xwing.Boosting = false;
                    }
                    else if (xwing.Energy >= 3 && xwing.BoostLock)
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
            }
            else
            {
                //Free cam for debug
                if (keyboardState.IsKeyDown(Keys.LeftShift))
                    CurrentMovementSpeed *= 10f;

                if (keyboardState.IsKeyDown(Keys.A))
                    Position += -RightDirection * CurrentMovementSpeed;
                if (keyboardState.IsKeyDown(Keys.D))
                    Position += RightDirection * CurrentMovementSpeed;
                if (keyboardState.IsKeyDown(Keys.W))
                    Position += FrontDirection * CurrentMovementSpeed;
                if (keyboardState.IsKeyDown(Keys.S))
                    Position += -FrontDirection * CurrentMovementSpeed;

                if (ArrowsLookEnabled)
                {
                    if (keyboardState.IsKeyDown(Keys.Up))
                    {
                        Pitch += CurrentTurnSpeed;
                        delta.Y = CurrentTurnSpeed;
                        if (Pitch > 89.0f)
                            Pitch = 89.0f;
                    }
                    if (keyboardState.IsKeyDown(Keys.Down))
                    {
                        Pitch -= CurrentTurnSpeed;
                        delta.Y = -CurrentTurnSpeed;
                        if (Pitch < -89.0f)
                            Pitch = -89.0f;

                    }
                    if (keyboardState.IsKeyDown(Keys.Left))
                    {
                        Yaw -= CurrentTurnSpeed;
                        delta.X = -CurrentTurnSpeed;
                        if (Yaw < 0)
                            Yaw += 360;
                        Yaw %= 360;
                    }
                    if (keyboardState.IsKeyDown(Keys.Right))
                    {
                        Yaw += CurrentTurnSpeed;
                        delta.X = CurrentTurnSpeed;
                        Yaw %= 360;
                    }
                    xwing.updateRoll(delta);
                }
            }
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