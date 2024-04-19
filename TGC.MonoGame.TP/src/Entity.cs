using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace TGC.MonoGame.TP
{


    class Entity
    {

        private Quaternion _orientation;

        private Vector3 _position;

        private Matrix _world;

        private Renderable _renderable;

        public Entity(Quaternion initialOrientation, Vector3 initialPosition, Renderable renderable)
        {
            _orientation = initialOrientation;
            _position = initialPosition;
            _renderable = renderable;
        }

        public void Update()
        {
            _world = Matrix.CreateTranslation(_position) * Matrix.CreateFromQuaternion(_orientation);
        }

        public void Draw(Camera camera){
            _renderable.Draw(_world,camera);
        }

        public Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }
        public Quaternion Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                _orientation = value;
            }

        }

        public Matrix World { get { return _world; } }


    }

}