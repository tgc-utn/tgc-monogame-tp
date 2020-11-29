using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Chinchulines.Graphics;
using Chinchulines.Enemigo;
using Chinchulines.Entities;
using Microsoft.Xna.Framework.Content;

namespace Chinchulines.Enemigo
{
    class Enemy
    {

        public Matrix EnemyWorld = Matrix.Identity;

        private Quaternion enemyRotation = Quaternion.Identity;

        private const string enemyShipPath = "Models/Spaceships/Motorcycle-MK2";
        private const string enemyTexturePath = "Textures/Spaceships/MK2/MK2-BaseColor";
        private Model enemySpaceship;

        public Vector3 enemyPosition;

        private int enemyHealth;

        private bool FlyLeftNow = false;
        private bool FlyRightNow = false;
        private bool FlyUpNow = false;
        private bool FlyDownNow = false;
        private bool BusyInOperation = false;

        private float RotatedRight;
        private float RotatedLeft;
        private float RotatedUp;
        private float RotatedDown;

        public Enemy(Vector3 Pos)
        {
            enemyPosition = Pos;

            enemyHealth = 100;
        }

        public void LoadContent(ContentManager content)
        {
            enemySpaceship = content.Load<Model>(enemyShipPath);

            var spaceShipEffect2 = (BasicEffect)enemySpaceship.Meshes[0].Effects[0];
            spaceShipEffect2.TextureEnabled = true;
            spaceShipEffect2.Texture = content.Load<Texture2D>(enemyTexturePath);
        }
        
        public void FlyRight()
        {
            if (RotatedRight >= 90)
            {
                BusyInOperation = false;
                RotatedRight = 0;
            }
            else
            {
                enemyRotation *= Quaternion.CreateFromYawPitchRoll(MathHelper.PiOver2 / 60, 0, 0);
                RotatedRight += MathHelper.PiOver2 / 60;
            }
        }

        public void FlyLeft()
        {
            if (RotatedLeft >= 90)
            {
                BusyInOperation = false;
                RotatedLeft = 0;
            }
            else
            {
                enemyRotation *= Quaternion.CreateFromYawPitchRoll(-(MathHelper.PiOver2 / 60), 0, 0);
                RotatedLeft += (MathHelper.PiOver2 / 60);
            }
        }

        public void FlyUp()
        {
            if (RotatedUp >= 90)
            {
                BusyInOperation = false;
                RotatedUp = 0;
            }
            else
            {
                enemyRotation *= Quaternion.CreateFromYawPitchRoll(0, MathHelper.PiOver2 / 60, 0);
                RotatedUp += (MathHelper.PiOver2 / 60);
            }
        }

        public void FlyDown()
        {
            if (RotatedDown >= 90)
            {
                BusyInOperation = false;
                RotatedDown = 0;
            }
            else
            {
                enemyRotation *= Quaternion.CreateFromYawPitchRoll(0, -(MathHelper.PiOver2 / 60), 0);
                RotatedDown += (MathHelper.PiOver2 / 60);
            }
        }

        public void SignalOperation(Vector3 playerpos)
        {
            Vector3 posdifer = playerpos - enemyPosition;

            if (posdifer.Z < 0)
            {
                FlyLeftNow = false;
                FlyRightNow = false;
                FlyUpNow = false;
                FlyDownNow = false;
            }
            else
            {

                if (posdifer.X < 10 && posdifer.X > 0)
                {
                    FlyRightNow = true;
                }
                else
                {
                    if (posdifer.X > -10 && posdifer.X < 0)
                    {
                        FlyLeftNow = true;
                    }
                    else
                    {
                        FlyRightNow = false;
                        FlyLeftNow = false;
                    }
                }

                if (posdifer.Y < 10)
                {
                    FlyUpNow = true;
                }
                else
                {
                    if (posdifer.Y > -10)
                    {
                        FlyDownNow = true;
                    }
                    else
                    {
                        FlyUpNow = false;
                        FlyDownNow = false;
                    }
                }
            }
        }

        private void MoveForward(ref Vector3 position, Quaternion rotationQuat, float speed)
        {
            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -0.5f), rotationQuat);
            position += addVector * speed;
        }

        public void Update(GameTime gameTime, Vector3 playerpos, LaserManager ls)
        {
            SignalOperation(playerpos);

            MoveForward(ref enemyPosition, enemyRotation, 0.1f);

            if (FlyLeftNow)
            {
                FlyLeft();
                FlyLeftNow = false;
            }
            if (FlyRightNow)
            {
                FlyRight();
                FlyRightNow = false;
            }
            if(FlyUpNow)
            {
                FlyUp();
                FlyUpNow = false;
            }
            if(FlyDownNow)
            {
                FlyDown();
                FlyDownNow = false;
            }

            EnemyWorld = Matrix.CreateFromQuaternion(enemyRotation) * Matrix.CreateTranslation(enemyPosition);

            ls.ShootLaser(gameTime, enemyPosition, enemyRotation);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            enemySpaceship.Draw(EnemyWorld *
                            Matrix.CreateScale(.05f) *
                            Matrix.CreateTranslation(enemyPosition), view, projection);
        }
    }

}
