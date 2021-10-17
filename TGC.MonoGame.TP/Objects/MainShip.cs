using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using static TGC.MonoGame.TP.Objects.CannonBall;

namespace TGC.MonoGame.TP.Objects
{
    public class MainShip
    {
        public Vector3 Position { get; set; }
        public Vector3 PositionAnterior { get; set; }
        public float speed { get; set; }
        private float maxspeed { get; set; }
        private float maxacceleration { get; set; }
        public Model modelo { get; set; }
        public Vector3 orientacion { get; set; }
        public Vector3 orientacionSobreOla { get; set; }
        public float anguloDeGiro { get; set; }
        public float anguloInicial { get; set; }
        public float giroBase { get; set; }
        private float time;
        private Boolean pressedAccelerator { get; set; }
        private int currentGear { get; set; }
        private Boolean HandBrake { get; set; }
        private Boolean pressedReverse { get; set; }

        private TGCGame _game;

        public string ModelName;
        public string SoundShotName;
        private Boolean CanShoot { get; set; }
        
        private Model cannonBall { get; set; }
        private List <CannonBall> cannonBalls= new List <CannonBall>();

        private SoundEffect soundShot { get; set; }

        public MainShip(Vector3 initialPosition, Vector3 currentOrientation, float MaxSpeed, TGCGame game)
        {
            speed = 0;
            Position = initialPosition;
            PositionAnterior = Position;
            orientacion = currentOrientation;
            maxspeed = MaxSpeed;
            maxacceleration = 0.1f;
            anguloDeGiro = 0f;
            anguloInicial = (float) (Math.PI/2);
            giroBase = 0.003f;
            pressedAccelerator = false;
            currentGear = 0;
            HandBrake = false;
            pressedReverse = false;
            ModelName = "Barco";
            SoundShotName = "mixkit-arcade-game-explosion-2759";
            _game = game;
        }

        public void LoadContent()
        {
            modelo = _game.Content.Load<Model>(TGCGame.ContentFolder3D + ModelName);
            soundShot = _game.Content.Load<SoundEffect>(TGCGame.ContentFolderSounds + SoundShotName);
            cannonBall = _game.Content.Load<Model>(TGCGame.ContentFolder3D + "sphere");
        }

        public void Draw()
        {
            modelo.Draw(
                Matrix.CreateRotationY(anguloDeGiro + anguloInicial) * Matrix.CreateScale(0.01f) *
                Matrix.CreateTranslation(Position), _game.Camera.View, _game.Camera.Projection);
            foreach (var cannon in cannonBalls)
            {
                cannon.Draw();
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var cannon in cannonBalls)
            {
                cannon.Update(gameTime);
            }
            ProcessKeyboard(_game.ElapsedTime);
            ProcessMouse(gameTime);
            UpdateMovementSpeed(gameTime);
            Move();
        }

        public void Move()
        {
            var newOrientacion = new Vector3((float)Math.Sin(anguloDeGiro), 0, (float)Math.Cos(anguloDeGiro));
            orientacion = newOrientacion;

            //TODO improve wave speed modification
            //var extraSpeed = 10;
            var extraSpeed=0;
            if (speed <= float.Epsilon) extraSpeed = 0; //Asi no se lo lleva el agua cuando esta parado
            var speedMod = speed + extraSpeed * -Vector3.Dot(orientacionSobreOla, Vector3.Up);
            
            Position += orientacion*speed ;
            if (PositionAnterior.Y < Position.Y)
            {
                Position -= orientacion * speed * (0.25f * (Position.Y - PositionAnterior.Y));
            }
            else
            {
                if (PositionAnterior.Y > Position.Y)
                {
                    Position += orientacion * speed * (0.25f * (PositionAnterior.Y - Position.Y));
                }
            }
        }

        private void UpdateMovementSpeed(GameTime gameTime)
        {
            float acceleration;
            time = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            if (HandBrake) acceleration = maxacceleration;
            else acceleration = maxacceleration * 8 * time;
            float GearMaxSpeed = (maxspeed * currentGear / 3);
            if (speed > GearMaxSpeed)
            {
                if (speed - acceleration < GearMaxSpeed)
                {
                    speed = GearMaxSpeed;
                }
                else
                {
                    speed -= acceleration;
                }
            }
            else if (speed < GearMaxSpeed)
            {
                if (speed + acceleration > GearMaxSpeed)
                {
                    speed = GearMaxSpeed;
                }
                else
                {
                    speed += acceleration;
                }
            }
        }

        private void ProcessMouse(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            if (mouseState.RightButton.Equals(ButtonState.Pressed) && CanShoot)
            {
                CanShoot = false;
                soundShot.Play();
                var matriz = new Matrix();
                matriz.M11 = mouseState.X;
                matriz.M21 = mouseState.Y;
                matriz.M31 = 1;
                
                /*var test = Matrix.Multiply(_game.Camera.Projection, matriz);
                var mouse = mouseState.Position.ToVector2();
                var screenSize = new Point(_game.GraphicsDevice.Viewport.Width / 2, _game.GraphicsDevice.Viewport.Height / 2);*/
                
                
                Vector3 nearScreen = new Vector3((float) mouseState.X / _game.GraphicsDevice.Viewport.Width,
                    (float)mouseState.Y / _game.GraphicsDevice.Viewport.Height, 0);
                Vector3 farScreen = new Vector3((float)mouseState.X / _game.GraphicsDevice.Viewport.Width,
                    (float)mouseState.Y / _game.GraphicsDevice.Viewport.Height, 1);
                Vector3 nearWorld = _game.GraphicsDevice.Viewport.Unproject(nearScreen, _game.Camera.Projection, _game.Camera.View, Matrix.Identity);
                Vector3 farWorld = _game.GraphicsDevice.Viewport.Unproject(farScreen, _game.Camera.Projection, _game.Camera.View, Matrix.Identity);
                Vector3 direction = farWorld - nearWorld;
                float zFactor = -nearWorld.Y / direction.Y;
                Vector3 zeroWorldPoint = nearWorld + direction * zFactor;
                
                
                Vector3 source = new Vector3(mouseState.X ,
                    mouseState.Y, 0);
                Vector3 source1 = new Vector3(mouseState.X ,
                    mouseState.Y, 1000000);
                Vector3 projectiontest =  _game.GraphicsDevice.Viewport.Unproject(source, _game.Camera.Projection, _game.Camera.View, _game.World);
                Vector3 projectiontest1 =  _game.GraphicsDevice.Viewport.Unproject(source1, _game.Camera.Projection, _game.Camera.View, _game.World);
                Vector3 proyectTotal = projectiontest1 - projectiontest;
                var test2 = Vector3.Transform(new Vector3(mouseState.X/_game.GraphicsDevice.Viewport.Width,mouseState.Y/_game.GraphicsDevice.Viewport.Height,1),   Matrix.Invert(_game.Camera.View)* Matrix.Invert(_game.Camera.Projection));
                cannonBalls.Add(new CannonBall(projectiontest,_game,cannonBall));
                
            }

            if (!mouseState.RightButton.Equals(ButtonState.Pressed))
            {
                CanShoot = true;
            }
        }

        private void ProcessKeyboard(float elapsedTime)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.D))
            {
                if (speed == 0)
                {
                }
                else
                {
                    if (anguloDeGiro + giroBase >= MathF.PI * 2)
                    {
                        anguloDeGiro +=  giroBase - MathF.PI * 2;
                    }
                    else
                    {
                        anguloDeGiro -= giroBase;
                    }
                }
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (speed == 0)
                {
                }
                else
                {
                    if (anguloDeGiro + giroBase < 0)
                    {
                        anguloDeGiro += - giroBase + MathF.PI * 2;
                    }
                    else
                    {
                        anguloDeGiro += giroBase;
                    }
                }
            }

            if (this.pressedAccelerator == false && keyboardState.IsKeyDown(Keys.W) && currentGear < 3)
            {
                currentGear++;
                pressedAccelerator = true;
                if (HandBrake) HandBrake = false;
            }

            if (this.pressedAccelerator == true && keyboardState.IsKeyUp(Keys.W))
            {
                pressedAccelerator = false;
            }

            if (this.pressedReverse == false && keyboardState.IsKeyDown(Keys.S) && currentGear > -2)
            {
                currentGear--;
                pressedReverse = true;
                if (HandBrake) HandBrake = false;
            }

            if (this.pressedReverse == true && keyboardState.IsKeyUp(Keys.S))
            {
                pressedReverse = false;
            }

            if (HandBrake == false && keyboardState.IsKeyDown(Keys.Space))
            {
                HandBrake = true;
                currentGear = 0;
            }
        }   
    }
}