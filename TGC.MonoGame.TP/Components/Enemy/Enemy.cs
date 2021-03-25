using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.MonoGame.TP.Components.Enemy
{
    class Enemy
    {
        private Vector3 Position { get; set; }
        private Vector3 Direction { get; set; }
        private Vector3 Up { get; set; }
        private float Velocity { get; set; }
        private int Life { get; set; }

        public Enemy()
        {
            Velocity = 0.005f;
            Life = 130;
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

        public int GetLife()
        {
            return Life;
        }

        public void TakeDamage(int Damage)
        {
            Life -= Damage;
        }

        public void Update(Vector3 CamaraPosition)
        {
            Position += (CamaraPosition - Position) * Velocity;
        }
    }
}
