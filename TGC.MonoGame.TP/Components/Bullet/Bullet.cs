using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.MonoGame.TP.Components.Bullet
{
    class Bullet
    {
        private Vector3 Position { get; set; }
        private Vector3 Direction { get; set; }
        private Vector3 Up { get; set; }
        private float Velocity { get; set; }

        public Bullet()
        {
            Velocity = 15;
        }

        public void SetPosition(Vector3 Position)
        {
            this.Position = Position;
        }

        public Vector3 GetPosition()
        {
            return Position;
        }

        public void SetDirection(Vector3 Direction)
        {
            this.Direction = Direction;
        }

        public Vector3 GetDirection()
        {
            return Direction;
        }

        public void SetUp(Vector3 Up)
        {
            this.Up = Up;
        }

        public Vector3 GetUp()
        {
            return Up;
        }

        public void Update()
        {
            Position += Direction * Velocity;
        }
    }
}
