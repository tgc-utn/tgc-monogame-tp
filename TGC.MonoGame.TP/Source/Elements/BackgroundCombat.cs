using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    public class BackgroundCombat
    {
        public List<Ship> backgroundShips = new List<Ship>();
        TGCGame Game;

        int pairs;
        int maxPairs;
        float genTimer = 0;
        float genTimerMax = 50;
        Random r;
        public BackgroundCombat() 
        {
            Game = TGCGame.Instance;
        }
        public void GenerateBackgroundCombat(float elapsedTime)
        {
            if(genTimer < genTimerMax)
            {
                genTimer += elapsedTime * 60;
            }
            else
            {
                genTimer = 0;
                r = new Random();
                genTimerMax = r.Next(1000, 7000);


                if(pairs < maxPairs)
                {
                    //generate
                    pairs++;
                }
                
            }
            

        }
        public void UpdateAll(float elapsedTime)
        {
            backgroundShips.ForEach(ship => ship.Update(elapsedTime));
            backgroundShips.RemoveAll(ship => ship.ShouldBeDestroyed());
        }
        public void AddAllRequiredToDraw(ref List<Ship> shipList)
        {
            var frustum = Game.BoundingFrustum;
            var onScreen = backgroundShips.FindAll(ship => ship.onScreen);

            foreach (var ship in onScreen)
                shipList.Add(ship);
           
        }
    }
    public class Ship
    {
        Vector3 Position;
        Vector3 FrontDirection;

        Matrix YPR;
        public Matrix SRT;
        Matrix Scale;

        bool Chasing;
        public bool Allied; 
        float betweenFire = 0;
        float fireRate;

        float Yaw, Pitch, Roll = 0f;

        float age = 0f;
        BoundingBox BB;
        Vector3 BBsize = new Vector3(10f, 5f, 10f);
        BoundingSphere BS;

        public bool onScreen;
        public Ship(Vector3 pos, Vector3 front, bool ally, bool chasing)
        {
            Position = pos;
            FrontDirection = front;
            Allied = ally;
            Chasing = chasing;

            if (Allied) 
            {
                Scale = Matrix.CreateScale(2.5f);
                BB = new BoundingBox(pos - BBsize, pos + BBsize);
            }
            else 
            {
                Scale = Matrix.CreateScale(0.02f);
                BS = new BoundingSphere(pos, 10f);
            }
        }
        public void Update(float elapsedTime)
        {
            Position += FrontDirection * elapsedTime;

            age += elapsedTime * 60;
            betweenFire += fireRate * 30f * elapsedTime;
            var frustum = TGCGame.Instance.BoundingFrustum;

            if (Allied)
            {
                BB.Min = Position - BBsize;
                BB.Max = Position + BBsize;

                onScreen = frustum.Intersects(BB);
            }
            else
            {
                BS.Center = Position;
                onScreen = frustum.Intersects(BS);
            }

            Yaw = MathF.Atan2(FrontDirection.X, FrontDirection.Z);

            Pitch = -MathF.Asin(FrontDirection.Y);

            YPR = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);
            SRT = Scale * YPR * Matrix.CreateTranslation(Position);

            if(Chasing)
                fireLaser();
        }
        void fireLaser()
        {
            if (betweenFire < 1)
                return;
            betweenFire = 0;

            Random r = new Random();

            fireRate = (float)(0.001d + r.NextDouble() * 0.05d);
            if(Allied)
                Laser.AlliedLasers.Add(new Laser(Position, YPR, SRT, FrontDirection, new Vector3(0f, 0f, 0.8f)));
            else
                Laser.EnemyLasers.Add(new Laser(Position, YPR, SRT, FrontDirection, new Vector3(0.8f, 0f, 0f)));

        }
        public bool ShouldBeDestroyed()
        {
            return age >= 5 && !onScreen; 
        }
    }
}
