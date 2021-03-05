namespace TGC.MonoGame.TP.Components.Camera
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The minimum behavior that a camera should have.
    /// </summary>
    public abstract class Camera
    {
        /// <summary>
        /// Defines the DefaultFieldOfViewDegrees.
        /// </summary>
        public const float DefaultFieldOfViewDegrees = MathHelper.PiOver4;

        /// <summary>
        /// Defines the DefaultNearPlaneDistance.
        /// </summary>
        public const float DefaultNearPlaneDistance = 0.1f;

        /// <summary>
        /// Defines the DefaultFarPlaneDistance.
        /// </summary>
        public const float DefaultFarPlaneDistance = 2000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="aspectRatio">The aspectRatio<see cref="float"/>.</param>
        /// <param name="nearPlaneDistance">The nearPlaneDistance<see cref="float"/>.</param>
        /// <param name="farPlaneDistance">The farPlaneDistance<see cref="float"/>.</param>
        public Camera(float aspectRatio, float nearPlaneDistance = DefaultNearPlaneDistance,
            float farPlaneDistance = DefaultFarPlaneDistance) : this(aspectRatio, nearPlaneDistance, farPlaneDistance,
            DefaultFieldOfViewDegrees)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="aspectRatio">The aspectRatio<see cref="float"/>.</param>
        /// <param name="nearPlaneDistance">The nearPlaneDistance<see cref="float"/>.</param>
        /// <param name="farPlaneDistance">The farPlaneDistance<see cref="float"/>.</param>
        /// <param name="fieldOfViewDegrees">The fieldOfViewDegrees<see cref="float"/>.</param>
        public Camera(float aspectRatio, float nearPlaneDistance, float farPlaneDistance, float fieldOfViewDegrees)
        {
            BuildProjection(aspectRatio, nearPlaneDistance, farPlaneDistance, fieldOfViewDegrees);
        }

        /// <summary>
        /// Gets or sets the AspectRatio
        /// Aspect ratio, defined as view space width divided by height..
        /// </summary>
        public float AspectRatio { get; set; }

        /// <summary>
        /// Gets or sets the FarPlane
        /// Distance to the far view plane..
        /// </summary>
        public float FarPlane { get; set; }

        /// <summary>
        /// Gets or sets the FieldOfView
        /// Field of view in the y direction, in radians..
        /// </summary>
        public float FieldOfView { get; set; }

        /// <summary>
        /// Gets or sets the NearPlane
        /// Distance to the near view plane..
        /// </summary>
        public float NearPlane { get; set; }

        /// <summary>
        /// Gets or sets the FrontDirection
        /// Direction where the camera is looking..
        /// </summary>
        public Vector3 FrontDirection { get; set; }

        /// <summary>
        /// Gets or sets the Projection
        /// The perspective projection matrix..
        /// </summary>
        public Matrix Projection { get; set; }

        /// <summary>
        /// Gets or sets the Position
        /// Position where the camera is located..
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the RightDirection
        /// Represents the positive x-axis of the camera space..
        /// </summary>
        public Vector3 RightDirection { get; set; }

        /// <summary>
        /// Gets or sets the UpDirection
        /// Vector up direction (may differ if the camera is reversed)..
        /// </summary>
        public Vector3 UpDirection { get; set; }

        /// <summary>
        /// Gets or sets the View
        /// The created view matrix..
        /// </summary>
        public Matrix View { get; set; }

        /// <summary>
        /// Build a perspective projection matrix based on a field of view, aspect ratio, and near and far view plane
        ///     distances.
        /// </summary>
        /// <param name="aspectRatio">The aspect ratio, defined as view space width divided by height.</param>
        /// <param name="nearPlaneDistance">The distance to the near view plane.</param>
        /// <param name="farPlaneDistance">The distance to the far view plane.</param>
        /// <param name="fieldOfViewDegrees">The field of view in the y direction, in degrees.</param>
        public void BuildProjection(float aspectRatio, float nearPlaneDistance, float farPlaneDistance,
            float fieldOfViewDegrees)
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfViewDegrees, aspectRatio, nearPlaneDistance,
                farPlaneDistance);
        }

        /// <summary>
        /// Allows updating the internal state of the camera if this method is overwritten.
        ///     By default it does not perform any action.
        /// </summary>
        /// <param name="gameTime">Holds the time state of a <see cref="Game" />.</param>
        public abstract void Update(GameTime gameTime);
    }
}
