using Microsoft.Xna.Framework;


/// <summary>
///     The minimum behavior that a camera should have.
/// </summary>
public abstract class Camera
{
    public const float DefaultFieldOfViewDegrees = MathHelper.Pi / 3;
    public const float DefaultNearPlaneDistance = 20.5f;
    public const float DefaultFarPlaneDistance = 1300;

    public Camera(float aspectRatio, float nearPlaneDistance = DefaultNearPlaneDistance,
        float farPlaneDistance = DefaultFarPlaneDistance) : this(aspectRatio, nearPlaneDistance, farPlaneDistance,
        DefaultFieldOfViewDegrees)
    {
    }

    public Camera(float aspectRatio, float nearPlaneDistance, float farPlaneDistance, float fieldOfViewDegrees)
    {
        AspectRatio = aspectRatio;
        NearPlane = nearPlaneDistance;
        FarPlane = farPlaneDistance;
        FieldOfView = fieldOfViewDegrees;
        //BuildProjection(aspectRatio, nearPlaneDistance, farPlaneDistance, fieldOfViewDegrees);
        UpdateProjection();
    }

    /// <summary>
    ///     Aspect ratio, defined as view space width divided by height.
    /// </summary>
    public float AspectRatio { get; set; }

    /// <summary>
    ///     Distance to the far view plane.
    /// </summary>
    public float FarPlane { get; set; }

    /// <summary>
    ///     Field of view in the y direction, in radians.
    /// </summary>
    public float FieldOfView { get; set; }

    /// <summary>
    ///     Distance to the near view plane.
    /// </summary>
    public float NearPlane { get; set; }

    /// <summary>
    ///     Direction where the camera is looking.
    /// </summary>
    public Vector3 FrontDirection { get; set; }

    /// <summary>
    ///     The perspective projection matrix.
    /// </summary>
    public Matrix Projection { get; set; }

    /// <summary>
    ///     Position where the camera is located.
    /// </summary>
    public Vector3 Position { get; set; }

    /// <summary>
    ///     Represents the positive x-axis of the camera space.
    /// </summary>
    public Vector3 RightDirection { get; set; }

    /// <summary>
    ///     Vector up direction (may differ if the camera is reversed).
    /// </summary>
    public Vector3 UpDirection { get; set; }

    /// <summary>
    ///     The created view matrix.
    /// </summary>
    public Matrix View { get; set; }

    /// <summary>
    ///     Build a perspective projection matrix based on a field of view, aspect ratio, and near and far view plane
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

    public void UpdateProjection()
    {
        Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearPlane,
            FarPlane);
    }

    /// <summary>
    ///     Allows updating the internal state of the camera if this method is overwritten.
    ///     By default it does not perform any action.
    /// </summary>
    /// <param name="gameTime">Holds the time state of a <see cref="Game" />.</param>
    public abstract void Update(GameTime gameTime);
}
