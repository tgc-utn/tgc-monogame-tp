/* LO DE MASTER NO FUNCIONA LO DE SHIPS :( -------------
using System;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Objects
{
    class Ship
    {
        public Vector3 Position { get; set; }
        public float speed { get; set; }
        private float maxspeed { get; set; }
        private float maxacceleration { get; set; }
        public Model modelo { get; set; }
        public Vector3 orientacion { get; set; }
        public Vector3 orientacionSobreOla { get; set; }
        public float anguloDeGiro { get; set; }
        public float giroBase { get; set; }

        private Boolean pressedAccelerator { get; set; }
        private int currentGear { get; set; }
        private Boolean HandBrake { get; set; }
        private Boolean pressedReverse { get; set; }

        private TGCGame _game;

        public string ModelName;

        public Ship(Vector3 initialPosition, Vector3 currentOrientation, float MaxSpeed, TGCGame game)
        {
            speed = 0;
            Position = initialPosition;
            orientacion = currentOrientation;
            maxspeed = MaxSpeed;
            maxacceleration = 0.005f;
            anguloDeGiro = 0f;
            giroBase = 0.003f;
            pressedAccelerator = false;
            currentGear = 0;
            HandBrake = false;
            pressedReverse = false;
            _game = game;
        }

        public void LoadContent()
        {
            modelo = _game.Content.Load<Model>(TGCGame.ContentFolder3D + ModelName);

            var basicShader = _game.Content.Load<Effect>(TGCGame.ContentFolderEffects + "BasicShader");

            basicShader.Parameters["KAmbient"]?.SetValue(0.15f);
            basicShader.Parameters["KDiffuse"]?.SetValue(0.75f);
            basicShader.Parameters["KSpecular"]?.SetValue(1f);
            basicShader.Parameters["Shininess"]?.SetValue(20f);

            basicShader.Parameters["AmbientColor"]?.SetValue(new Vector3(1f, 0.98f, 0.98f));
            basicShader.Parameters["SpecularColor"]?.SetValue(new Vector3(1f, 1f, 1f));

            for (int i = 0; i < modelo.Meshes.Count; i++)
            {
                var mesh = modelo.Meshes[i];
                for (int j = 0; j < mesh.MeshParts.Count; j++)
                {
                    var part = mesh.MeshParts[j];
                    var partShader = basicShader.Clone();
                    partShader.Parameters["Texture"].SetValue(part.Effect.Parameters["Texture"]?.GetValueTexture2D());
                    part.Effect = partShader;
                }
            }
        }

        public void Draw()
        {
            var playerBoatWorld = _game.World * waterMatrix * Matrix.CreateTranslation(Position);
            for (int i = 0; i < modelo.Meshes.Count; i++)
            {
                var mesh = modelo.Meshes[i];
                for (int j = 0; j < mesh.MeshParts.Count; j++)
                {
                    var part = mesh.MeshParts[j];
                    var effect = part.Effect;
                    effect.Parameters["World"].SetValue(boneTransforms[mesh.ParentBone.Index] * playerBoatWorld);
                    effect.Parameters["View"].SetValue(_game.CurrentCamera.View);
                    effect.Parameters["Projection"].SetValue(_game.CurrentCamera.Projection);
                    effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(playerBoatWorld)));
                    effect.Parameters["CameraPosition"]?.SetValue(_game.CurrentCamera.Position);
                }
                mesh.Draw();
            }
        }

        public void Update(GameTime gameTime)
        {
            ProcessKeyboard(_game.ElapsedTime);
            UpdateMovementSpeed(_game.ElapsedTime);
            Move();
        }

        public void Move()
        {
            var newOrientacion = new Vector3((float)Math.Sin(anguloDeGiro), 0, (float)Math.Cos(anguloDeGiro));
            orientacion = newOrientacion;

            //TODO improve wave speed modification
            var extraSpeed = 10;
            if (speed <= float.Epsilon) extraSpeed = 0; //Asi no se lo lleva el agua cuando esta parado
            var speedMod = speed + extraSpeed * -Vector3.Dot(orientacionSobreOla, Vector3.Up);

            var newPosition = new Vector3(Position.X - speed * orientacion.X, Position.Y, Position.Z + speed * orientacion.Z);

            Position = newPosition;
        }

        private void UpdateMovementSpeed(float gameTime)
        {
            float acceleration;
            if (HandBrake) acceleration = maxacceleration;
            else acceleration = maxacceleration * 8;
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
        private void ProcessKeyboard(float elapsedTime)
        {
            var keyboardState = Keyboard.GetState();


            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (speed == 0) { }
                else
                {
                    if (anguloDeGiro + giroBase >= MathF.PI * 2)
                    {
                        anguloDeGiro = anguloDeGiro + giroBase - MathF.PI * 2;
                    }
                    else
                    {
                        anguloDeGiro -= giroBase;
                    }
                }
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                if (speed == 0) { }
                else
                {
                    if (anguloDeGiro + giroBase < 0)
                    {
                        anguloDeGiro = anguloDeGiro - giroBase + MathF.PI * 2;
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
                Console.WriteLine("b");
                pressedAccelerator = true;
                if (HandBrake) HandBrake = false;
            }
            if (this.pressedAccelerator == true && keyboardState.IsKeyUp(Keys.W))
            {
                Console.WriteLine("a");
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
*/