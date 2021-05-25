using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Ships
{
    public class Ship
    {
        private TGCGame Game;
        private Model ShipModel { get; set; }

        public string ModelName;
        private Effect ShipEffect { get; set; }
        
        public string EffectName;

        public Texture2D ShipTexture;

        public string TextureName;
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public float Speed { get; set; }
        //public Vector3 WaveOrientation { get; set; }

        //public float anguloDeGiro { get; set; }
        //public float giroBase { get; set; }

        // int _maxLife = 100;

        //private int _currentLife = 100;

        //private float _shootingCooldownTime = 0.8f;

        //private float _timeToCooldown = 0f;

        public bool playerMode = false;


        //private Matrix waterMatrix;

        private Matrix[] BoneMatrix;

        public Ship(TGCGame game, Vector3 pos, Vector3 rot, Vector3 scale, float speed, string modelName, string effect, string textureName)
        {
            Game = game;
            Position = pos;
            Rotation = rot;
            Scale = scale;
            Speed = speed;
            ModelName = modelName;
            EffectName = effect;
            TextureName = textureName;
        }

        public void LoadContent()
        {
            ShipModel = Game.Content.Load<Model>(TGCGame.ContentFolder3D + "Botes/" + ModelName);
            ShipEffect = Game.Content.Load<Effect>(TGCGame.ContentFolderEffects + EffectName);
            ShipTexture = Game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + "Botes/" + TextureName);

            BoneMatrix = new Matrix[ShipModel.Bones.Count];
            ShipModel.CopyAbsoluteBoneTransformsTo(BoneMatrix);
        }

        public void Draw()
        {
            ShipEffect.Parameters["ModelTexture"].SetValue(ShipTexture);
            DrawModel(ShipModel, Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position), ShipEffect);
        }

        private void DrawModel(Model geometry, Matrix transform, Effect effect)
        {
            foreach (var mesh in geometry.Meshes)
            {
                effect.Parameters["World"].SetValue(transform);
                effect.Parameters["View"].SetValue(Game.CurrentCamera.View);
                effect.Parameters["Projection"].SetValue(Game.CurrentCamera.Projection);
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
                mesh.Draw();
            }
        }
        /*
        public void Update(GameTime gameTime)
        {
            
            UpdateShipRegardingWaves(_game.ElapsedTime);
            if (CanBeControlled)
            {
                ProcessKeyboard(_game.ElapsedTime);
                UpdateMovementSpeed(_game.ElapsedTime);
                Move();
                if (_timeToCooldown >= float.Epsilon)
                {
                    _timeToCooldown -= Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
        }*/

    /*
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

    public void UpdateShipRegardingWaves(float time)
    {
        float waveFrequency = 0.01f;
        float waveAmplitude = 20;
        time *= 2;

        var worldVector = Position;

        var newY = (MathF.Sin(worldVector.X * waveFrequency + time) + MathF.Sin(worldVector.Z * waveFrequency + time)) * waveAmplitude;

        var tangent1 = Vector3.Normalize(new Vector3(1,
            (MathF.Cos(worldVector.X * waveFrequency + time) * waveFrequency * waveAmplitude) * 0.5f
            , 0));
        var tangent2 = Vector3.Normalize(new Vector3(0,
            (MathF.Cos(worldVector.Z * waveFrequency + time) * waveFrequency * waveAmplitude) * 0.5f
            , 1));

        worldVector = new Vector3(worldVector.X, newY + 10, worldVector.Z);

        Position = worldVector;

        var waterNormal = Vector3.Normalize(Vector3.Cross(tangent2, tangent1));

        //Proyectamos la orientacion sobre el plano formado con la normal del agua para subir o bajar la proa del barco
        orientacionSobreOla = orientacion - Vector3.Dot(orientacion, waterNormal) * waterNormal;

        waterMatrix = Matrix.CreateLookAt(Vector3.Zero, orientacionSobreOla, waterNormal);
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

        if (Mouse.GetState().LeftButton == ButtonState.Pressed && _timeToCooldown < float.Epsilon)
        {
            var bulletOrientation = orientacion;
            bulletOrientation.X = -bulletOrientation.X;
            _game.Bullets.Add(new Bullet(_game, Position, bulletOrientation + Vector3.Up * 0.2f));
            _timeToCooldown = _shootingCooldownTime;
        }

    }*/
}
}