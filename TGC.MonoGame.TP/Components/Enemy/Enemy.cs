using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.MonoGame.TP.Components.Enemy
{
    public class Enemy
    {
        private Vector3 Position { get; set; }
        private Vector3 Direction { get; set; }
        private Vector3 Up { get; set; }
        private float Velocity { get; set; }
        private int Life { get; set; }

        private readonly int minDistance = 100;

        public Enemy()
        {
            Random random = new Random();
            Velocity = 2f * random.Next(1,3);
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

        public void Update(Vector3 CameraPosition, List<Enemy> OtherEnemies, Vector3 PlayerPosition)
        {
            Vector3 distance = CameraPosition - Position;
            Position += (distance) * Velocity/200;

            foreach(Enemy enemy in OtherEnemies) 
            {
                if (this != enemy && Vector3.Distance(Position,enemy.Position) < minDistance)
                {
                    AvoidCollision(enemy.Position);
                }
            }

            if (Vector3.Distance(Position, PlayerPosition) < minDistance)
            {
                AvoidCollision(PlayerPosition);
            }
        }

        private void AvoidCollision(Vector3 EnemyPosition)
        {
            Vector3 direction = Position - EnemyPosition;
            direction.Normalize();
            Position += direction;
        }
    }
}
