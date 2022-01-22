namespace TGC.MonoGame.TP.Components.Camera
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Defines the <see cref="FreeCamera" />.
    /// </summary>
    internal class FreeCamera : Camera
    {
        /// <summary>
        /// Defines the screenCenter.
        /// </summary>
        private readonly Point screenCenter;

        /// <summary>
        /// Defines the changed.
        /// </summary>
        private bool changed;

        /// <summary>
        /// Defines the pastMousePosition.
        /// </summary>
        private Vector2 pastMousePosition;

        /// <summary>
        /// Defines the accumulatedTime.
        /// </summary>
        private float accumulatedTime = 0f;

        /// <summary>
        /// Defines the bobOscilate.
        /// </summary>
        private float bobOscilate = 0f;

        /// <summary>
        /// Define de dash hability power starts on 51
        /// </summary>
        private float dashPower;

        /// <summary>
        /// Define if player is dashing or not
        /// </summary>
        private bool isDashing;

        // Angles
        /// <summary>
        /// Defines the pitch.
        /// </summary>
        public float pitch;

        /// <summary>
        /// Defines the yaw.
        /// </summary>
        public float yaw = -90f;

        public Vector2 ColumnPosition = new Vector2(50, 50);
        public Vector2 Column2Position = new Vector2(50, 2650);
        public Vector2 Column3Position = new Vector2(2650, 50);
        public Vector2 Column4Position = new Vector2(2650, 2650);

        public void SetDashPower(float newDashPower)
        {
            dashPower = newDashPower;
        }

        public float GetDashPower()
        {
            return dashPower;
        }

        public void SetIsDashing(bool newIsDashing)
        {
            isDashing = newIsDashing;
        }

        public bool GetIsDashing()
        {
            return isDashing;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreeCamera"/> class.
        /// </summary>
        /// <param name="aspectRatio">The aspectRatio<see cref="float"/>.</param>
        /// <param name="position">The position<see cref="Vector3"/>.</param>
        /// <param name="screenCenter">The screenCenter<see cref="Point"/>.</param>
        public FreeCamera(float aspectRatio, Vector3 position, Point screenCenter) : this(aspectRatio, position)
        {
            this.screenCenter = screenCenter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreeCamera"/> class.
        /// </summary>
        /// <param name="aspectRatio">The aspectRatio<see cref="float"/>.</param>
        /// <param name="position">The position<see cref="Vector3"/>.</param>
        public FreeCamera(float aspectRatio, Vector3 position) : base(aspectRatio)
        {
            Position = position;
            pastMousePosition = Mouse.GetState().Position.ToVector2();
            Mouse.SetCursor(MouseCursor.Crosshair);
            UpdateCameraVectors();
            CalculateView();
        }

        /// <summary>
        /// Gets or sets the MovementSpeed.
        /// </summary>
        public float MovementSpeed { get; set; } = 850f;

        /// <summary>
        /// Gets or sets the MouseSensitivity.
        /// </summary>
        public float MouseSensitivity { get; set; } = 12f;

        /// <summary>
        /// The CalculateView.
        /// </summary>
        private void CalculateView()
        {
            //Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 2, 16/9, 0.00000000000000001f, 1000);
            View = Matrix.CreateLookAt(Position, Position + FrontDirection, UpDirection);
        }

        /// <inheritdoc />
        public override void Update(GameTime gameTime)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            changed = false;
            ProcessKeyboard(elapsedTime);
            ProcessMouseMovement(elapsedTime);
            
            if (changed)
                CalculateView();
        }

        /// <summary>
        /// The BobingOscilation.
        /// </summary>
        /// <returns>The <see cref="float"/>.</returns>
        private float BobingOscilation()
        {
            return MathF.Sin(accumulatedTime / 0.1f);
        }

        /// <summary>
        /// The ProcessKeyboard.
        /// </summary>
        /// <param name="elapsedTime">The elapsedTime<see cref="float"/>.</param>
        private void ProcessKeyboard(float elapsedTime)
        {
            accumulatedTime += elapsedTime;
            var keyboardState = Keyboard.GetState();

            var currentMovementSpeed = MovementSpeed;

            var newPosition = Position;

            if (dashPower >= 50 && isDashing) isDashing = false;

            if (dashPower < 51) dashPower += 0.1f;

            if (keyboardState.IsKeyDown(Keys.LeftShift) && dashPower >= 50 && !isDashing)
            {
                dashPower -= 50;
                isDashing = true;
                currentMovementSpeed *= 150f;
            }                

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                newPosition += -RightDirection * currentMovementSpeed * elapsedTime;
                bobOscilate = BobingOscilation();
                changed = true;
            }

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                newPosition += RightDirection * currentMovementSpeed * elapsedTime;
                bobOscilate = BobingOscilation();
                changed = true;
            }

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                newPosition += FrontDirection * currentMovementSpeed * elapsedTime;
                bobOscilate = BobingOscilation();
                changed = true;
            }

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                newPosition += -FrontDirection * currentMovementSpeed * elapsedTime;
                bobOscilate = BobingOscilation();
                changed = true;
            }

            // Collision with external space
            if (newPosition.X >= -2700 && newPosition.Z >= -2700 && newPosition.X <= 5400 && newPosition.Z <= 5400)
            {
                // Collision with columns
                if(Vector2.Distance(new Vector2(newPosition.X,newPosition.Z),ColumnPosition) >= 125 
                && Vector2.Distance(new Vector2(newPosition.X, newPosition.Z), Column2Position) >= 125
                && Vector2.Distance(new Vector2(newPosition.X, newPosition.Z), Column3Position) >= 125
                && Vector2.Distance(new Vector2(newPosition.X, newPosition.Z), Column4Position) >= 125)
                    Position = new Vector3(newPosition.X, bobOscilate, newPosition.Z);
            }
        }

        /// <summary>
        /// The ProcessMouseMovement.
        /// </summary>
        /// <param name="elapsedTime">The elapsedTime<see cref="float"/>.</param>
        private void ProcessMouseMovement(float elapsedTime)
        {
            var mouseState = Mouse.GetState();

            var mouseDelta = mouseState.Position.ToVector2() - pastMousePosition;
            mouseDelta *= -1 * MouseSensitivity * elapsedTime;

            yaw -= mouseDelta.X;
            pitch += mouseDelta.Y;

            if (pitch > 89.0f)
                pitch = 89.0f;
            if (pitch < -89.0f)
                pitch = -89.0f;

            changed = true;
            UpdateCameraVectors();

            Mouse.SetPosition(screenCenter.X, screenCenter.Y);

            pastMousePosition = Mouse.GetState().Position.ToVector2();
        }

        /// <summary>
        /// The UpdateCameraVectors.
        /// </summary>
        private void UpdateCameraVectors()
        {
            // Calculate the new Front vector
            Vector3 tempFront;
            tempFront.X = MathF.Cos(MathHelper.ToRadians(yaw)) * MathF.Cos(MathHelper.ToRadians(pitch));
            tempFront.Y = MathF.Sin(MathHelper.ToRadians(pitch));
            tempFront.Z = MathF.Sin(MathHelper.ToRadians(yaw)) * MathF.Cos(MathHelper.ToRadians(pitch));

            FrontDirection = Vector3.Normalize(tempFront);

            // Also re-calculate the Right and Up vector
            // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
            RightDirection = Vector3.Normalize(Vector3.Cross(FrontDirection, Vector3.Up));
            UpDirection = Vector3.Normalize(Vector3.Cross(RightDirection, FrontDirection));
        }
    }
}
