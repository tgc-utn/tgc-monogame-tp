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

        public Matrix EnemyWorld;
        public Matrix Enemy2World;

        private Quaternion enemyRotation = Quaternion.Identity;

        private const string enemyShipPath = "Models/Spaceships/Motorcycle-MK2";
        private const string enemyTexturePath = "Textures/Spaceships/MK2/MK2-BaseColor";

        private const string enemyShipModel2 = "Models/Spaceships/SpaceShip-MK3";
        private const string enemyTexture2 = "Textures/Spaceships/MK3/MK3-Albedo";

        private Model enemySpaceship;
        private Model enemySpaceship2;

        public Vector3 enemyPosition;

        private int enemyHealth;

        private bool FlyLeftNow = false;
        private bool FlyRightNow = false;
        private bool FlyUpNow = false;
        private bool FlyDownNow = false;
        private bool BusyInOperation = false;
        private bool MovingForward = true;

        private float RotatedRight;
        private float RotatedLeft;
        private float RotatedUp;
        private float RotatedDown;

        private LaserManager _laserManager;


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
                MovingForward = true;
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
                MovingForward = true;
            }
            else
            {
                enemyRotation *= Quaternion.CreateFromYawPitchRoll(-MathHelper.PiOver2 / 60, 0, 0);
                RotatedLeft += (MathHelper.PiOver2 / 60);
            }
        }

        public void FlyUp()
        {
            if (RotatedUp >= 90)
            {
                BusyInOperation = false;
                RotatedUp = 0;
                MovingForward = true;
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
                MovingForward = true;
            }
            else
            {
                enemyRotation *= Quaternion.CreateFromYawPitchRoll(0, -(MathHelper.PiOver2 / 60), 0);
                RotatedDown += (MathHelper.PiOver2 / 60);
            }
        }

        public void SignalOperation(Vector3 playerpos)
        {
            var posdifer = enemyPosition - playerpos;

            if (Math.Sign(posdifer.X).Equals(-1))
            {
                FlyRightNow = true;
            }
            else
            {
                if(Math.Sign(posdifer.X).Equals(1))
                {
                    FlyLeftNow = true;
                }
                else
                {
                    FlyRightNow = false;
                    FlyLeftNow = false;
                }
            }
            
            if(Math.Sign(posdifer.Y).Equals(-1))
            {
                FlyUpNow = true;
            }
            else
            {
                if(Math.Sign(posdifer.Y).Equals(1))
                {
                    FlyDownNow = true;
                }
                else
                {
                    FlyUpNow = false;
                    FlyDownNow = false;
                }
            }

            if (Math.Sign(posdifer.Z).Equals(0))
            {
                 FlyLeftNow = false;
                 FlyRightNow = false;
                 FlyUpNow = false;
                 FlyDownNow = false;
             }
        }

        private void MoveForward(ref Vector3 position, Quaternion rotationQuat, float speed)
        {
            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -0.5f), rotationQuat);
            position += addVector * speed;
        }

        public void Update(GameTime gameTime, Vector3 playerpos)
        {
            SignalOperation(playerpos);

            if (MovingForward)
            {
                MoveForward(ref enemyPosition, enemyRotation, 0.5f);
            }

            if (FlyLeftNow)
            {
                FlyLeft();
            }
            else if (FlyRightNow)
            {
                FlyRight();
            }
            else if(FlyUpNow)
            {
                FlyUp();
            }
            else if(FlyDownNow)
            {
                FlyDown();
            }
            else
            {
                enemyRotation = Quaternion.Identity;
            }

            EnemyWorld = Matrix.CreateFromQuaternion(enemyRotation) * Matrix.CreateTranslation(enemyPosition);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            enemySpaceship.Draw(EnemyWorld *
                            Matrix.CreateScale(.08f) *
                            Matrix.CreateTranslation(enemyPosition), view, projection);
        }

        //TODO: mover metodos a otra clase de otro tipo de enemigo
        public void Update(Vector3 playerpos, GameTime gameTime, float movementSpeed)
        {
            Enemy2World = Matrix.CreateFromQuaternion(enemyRotation) * Matrix.CreateTranslation(playerpos);

            _laserManager.ShootLaser(gameTime, playerpos + new Vector3(0,0,1), enemyRotation);

            _laserManager.UpdateLaser(movementSpeed);
        }

        public void DrawEnemy2(Matrix view, Matrix projection,Vector3 playerpos, Quaternion playerRotation, Vector3 cameraPosition, Vector3 cameraDirection, GraphicsDeviceManager graphics)
        {
            enemySpaceship2.Draw(Enemy2World *
                            Matrix.CreateScale(.005f) *
                            Matrix.CreateFromQuaternion(playerRotation) *
                            Matrix.CreateTranslation(playerpos.X, playerpos.Y, playerpos.Z + 1)
                            , view, projection);

            _laserManager.DrawLasers(view, projection, cameraPosition, cameraDirection, graphics);
        }
                    
        public void LoadContentEnemigoVigilante(ContentManager content, GraphicsDeviceManager graphics)
        {
            enemySpaceship2 = content.Load<Model>(enemyShipModel2);

            var spaceShipEffect3 = (BasicEffect)enemySpaceship2.Meshes[0].Effects[0];
            spaceShipEffect3.TextureEnabled = true;
            spaceShipEffect3.Texture = content.Load<Texture2D>(enemyTexture2);

            _laserManager = new LaserManager();
            _laserManager.LoadContent("Textures/Lasers/red-laser", "Effects/Trench", content, graphics);
        }
    }
}
