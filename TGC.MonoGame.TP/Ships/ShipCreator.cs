using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Ships
{
    public class Ship
    {
        private TGCGame Game;
        private float time;

        private Model ShipModel { get; set; }

        public string ModelName;
        private Effect ShipEffect { get; set; }
        
        public string EffectName;

        public Texture2D ShipTexture;

        public string TextureName;
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 wavesRotation { get; set; }
        public Vector3 Scale { get; set; }
        public float Speed { get; set; }

        private Vector3 FrontDirection;

        private float PlayerRotation;

        private float MovementSpeed { get; set; }
        private float RotationSpeed { get; set; }

        public Matrix PlayerBoatMatrix { get; set; }

        public bool playerMode = false;
        private Matrix waterMatrix { get; set; }


        //private Matrix[] BoneMatrix;



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
            MovementSpeed = 100.0f;
            RotationSpeed = 0.5f;
            FrontDirection = Vector3.Forward;
            PlayerRotation = 0;

            PlayerBoatMatrix = Matrix.Identity * Matrix.CreateScale(scale) * Matrix.CreateRotationY(rot.Y) * Matrix.CreateTranslation(pos);
        }
        public void LoadContent()
        {
            ShipModel = Game.Content.Load<Model>(TGCGame.ContentFolder3D + ModelName);
            ShipEffect = Game.Content.Load<Effect>(TGCGame.ContentFolderEffects + EffectName);
            ShipTexture = Game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + TextureName);

        }


        public void Draw()
        {
            ShipEffect.Parameters["ModelTexture"].SetValue(ShipTexture);
            DrawModel(ShipModel, PlayerBoatMatrix, ShipEffect);
            //DrawModel(ShipModel, Matrix.CreateScale(Scale) * Matrix.CreateRotationY((float)PlayerRotation) * Matrix.CreateTranslation(Position), ShipEffect);



            //DrawModel(ShipModel, Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position), ShipEffect);
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
        public void Update(GameTime gameTime)
        {
            float elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            float yPos = GetWaterPositionY(time);
            Position = new Vector3(Position.X, yPos, Position.Z);
            PlayerBoatMatrix = Matrix.CreateScale(Scale) * Matrix.CreateRotationY((float)PlayerRotation) * Matrix.CreateTranslation(Position);
            FrontDirection = - new Vector3((float)Math.Sin(PlayerRotation), 0.0f, (float)Math.Cos(PlayerRotation));

            if (playerMode)
            {
                ProcessKeyboard(elapsedTime);
            }
        }

        float frac(float val)
        {
            return val - MathF.Floor(val);
        }

        public float GetWaterPositionY(float gameTime)
        {

            float speed = 0.05f;
            float offset = 10f;
            float radius = 3f;
            var worldPosition = Position;

            var posY = 0.4 * MathF.Cos(((float)(worldPosition.X * 0.0075f) * .5f + gameTime * speed) * offset) * radius * 0.5f;
            posY += (1 - frac(gameTime * 0.1f)) * frac(gameTime * 0.1f) * 0.1f * MathF.Cos((-(float)(worldPosition.Z * 0.0075f) + gameTime * speed * 1.3f) * offset) * radius;

            posY *= 7f;

            var wavetan1 = Vector3.Normalize(new Vector3(1f,
                0.4f * MathF.Cos(((float)(worldPosition.X) * .5f + gameTime * speed) * offset) * radius * 0.5f
                , 0f));

            var wavetan2 = Vector3.Normalize(new Vector3(0,
                (1 - frac(gameTime * 0.1f)) * frac(gameTime * 0.1f) * 0.1f * MathF.Cos((-(float)(worldPosition.Z) + gameTime * speed * 1.3f) * offset) * radius,
                1));

            worldPosition = new Vector3(worldPosition.X, (float)(posY) * 2, worldPosition.Z);

            Position = worldPosition;

            var waterNormal = Vector3.Normalize(Vector3.Cross(wavetan2, wavetan1));

            wavesRotation = Rotation - Vector3.Dot(Rotation, waterNormal) * waterNormal;

            waterMatrix = Matrix.CreateLookAt(Vector3.Zero, wavesRotation, waterNormal);
            return (float)posY;
        }

        //float noise(in float2 st)
        //{
        //    float2 i = floor(st);
        //    float2 f = frac(st);

        //    // Four corners in 2D of a tile
        //    float a = random(i);
        //    float b = random(i + float2(1.0, 0.0));
        //    float c = random(i + float2(0.0, 1.0));
        //    float d = random(i + float2(1.0, 1.0));

        //    // Smooth Interpolation

        //    // Cubic Hermine Curve.  Same as SmoothStep()
        //    float2 u = f * f * (3.0 - 2.0 * f);
        //    // u = smoothstep(0.,1.,f);

        //    // Mix 4 coorners percentages
        //    return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
        //}


        //public float GetWaterPositionY(float gameTime)
        //{
        //    float WaveHeight = 50;
        //    float y = (float)Math.Sin(gameTime) * WaveHeight;

        //    //float4 worldPosition = mul(input.Position, World);

        //    //worldPosition.y += cos(Time + 15 * worldPosition.x * 0.01) * WaveHeight;

        //    //output.WorldPosition = worldPosition;

        //    //// World space to View space
        //    //float4 viewPosition = mul(worldPosition, View);
        //    //// View space to Projection space
        //    //output.Position = mul(viewPosition, Projection);
        //    //// Propagate texture coordinates
        //    //output.TextureCoordinate = input.TextureCoordinate;
        //    //// Propagate color by vertex
        //    //output.Color = input.Color;
        //    return y;
        //}

        private void ProcessKeyboard(float elapsedTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Game.Exit();
            }

            //var currentMovementSpeed = MovementSpeed;

            if (keyboardState.IsKeyDown(Keys.W))
            {
                MoveForward(MovementSpeed * elapsedTime);
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                MoveBackwards(MovementSpeed * elapsedTime);
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                RotateRight(RotationSpeed * elapsedTime);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                RotateLeft(RotationSpeed * elapsedTime);
            }


        }

        private void MoveForward(float amount)
        {
            Position += FrontDirection * amount;
        }
        private void MoveBackwards(float amount)
        {
            MoveForward(-amount);
        }
        private void RotateRight(float amount)
        {
            PlayerRotation += amount;
        }
        private void RotateLeft(float amount)
        {
            RotateRight(-amount);
        }
    }
}