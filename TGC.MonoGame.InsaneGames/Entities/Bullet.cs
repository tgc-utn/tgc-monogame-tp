using System;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.InsaneGames.Entities
{
    class Bullet : Entity
    {
        protected Vector3 Speed, Acceleration, InitialPos;
        
        protected Vector3 LastPosition, CurrentPosition;
        protected float Damage;
        override public Vector3 BottomVertex 
        { 
            get { return LastPosition; }
        }
        override public Vector3 UpVertex 
        { 
            get { return CurrentPosition; }
        }
        public Bullet(float damage, Vector3 speed, Vector3 acceleration, Vector3 initialPos)
        {
            Damage = damage;
            Speed = speed;
            Acceleration = acceleration;
            InitialPos = initialPos;
            LastPosition = initialPos;
        }

        public override void Update(GameTime gameTime)
        {
            LastPosition = CurrentPosition;
            var time = gameTime.ElapsedGameTime.TotalSeconds;
            CurrentPosition = 0.5f * Acceleration * (float) Math.Pow(time, 2) + Speed * (float) time + InitialPos;
        }
    }
}