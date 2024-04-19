using System.Numerics;
using System.Security;
using BepuUtilities;
using Microsoft.Xna.Framework;
using Matrix = Microsoft.Xna.Framework.Matrix;
using Vector3 = Microsoft.Xna.Framework.Vector3;


namespace TGC.MonoGame.TP
{

    class Camera
    {
        private Matrix _projection;
        private Matrix _view;
        private Vector3 _upVector = new Vector3(0, 1, 0);
        public Vector3 _relativePosition;

        private const float defaultNearPlaneDistance = 0.1f;
        private const float defaultFarPlaneDistance = 2000f;
        private const float defaultFOV = Microsoft.Xna.Framework.MathHelper.PiOver4;

        public Camera(Vector3 initialPosition,float aspectRatio, float fov = defaultFOV, float nearPlaneDistance = defaultNearPlaneDistance, float farPlaneDistance = defaultFarPlaneDistance)
        {
            _relativePosition = initialPosition;
            _projection = Matrix.CreatePerspectiveFieldOfView(aspectRatio, fov, nearPlaneDistance, farPlaneDistance);
        }

        public void Follow(Entity entity)
        {
            Vector3 realPosition = Vector3.Transform(_relativePosition,entity.World);
            _view = Matrix.CreateLookAt(realPosition, entity.World.Translation, _upVector);
        }

        public Matrix Projection {get {return _projection;}}
        public Matrix View {get {return _view;}}

    }


}