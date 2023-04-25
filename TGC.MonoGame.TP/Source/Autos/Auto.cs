using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Design;

namespace TGC.MonoGame.TP
{
    public class Auto : IElementoDinamico
    { 
        private Vector3 Position;
        private Vector3 Velocity;
        private float AccelerationMagnitude = 2500f;
        private float Rotation;
        private float JumpPower = 50000f;
        private float Turning = 0f;
        private bool cambio = false;
        private float Escala = 0.75f;

        public Auto(Vector3 posicionInicial, float escala = 0.75f) : base(posicionInicial, Vector3.Zero, escala)
        {
            Model = TGCGame.GameContent.M_Auto;
            Position = posicionInicial;
            Escala = escala;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            float accelerationSense = 0f;
            Vector3 acceleration = Vector3.Zero;
            float dTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            if(keyboardState.IsKeyDown(Keys.M) && !cambio ){
                cambio = true;
                Model = TGCGame.GameContent.M_AutoPegni;
                Escala = 0.3f;
            }
            
            // GRAVEDAD
            float floor = 0f;
            Vector3 Gravity = -Vector3.Up * 15f;
            Matrix MatrixRotation = Matrix.CreateRotationY(Rotation);
  
            // GIRO
            Turning += keyboardState.IsKeyDown(Keys.A) ? 1f : 0;
            Turning -= keyboardState.IsKeyDown(Keys.D) ? 1f : 0;
            Rotation = Turning * dTime;

            if(Position.Y<floor){
                Position = new Vector3(Position.X, floor, Position.Z);
                Velocity = new Vector3(Velocity.X, 0, Velocity.Z);
            
                // ACELERACION
                if (keyboardState.IsKeyDown(Keys.W))
                    accelerationSense = 1f;
                else if (keyboardState.IsKeyDown(Keys.S))
                    accelerationSense = -0.5f; //Reversa mas lenta

                Vector3 accelerationDirection = -MatrixRotation.Forward;
                acceleration = accelerationDirection * accelerationSense * AccelerationMagnitude;

                // ROZAMIENTO
                float u = -1.35f; //Coeficiente de Rozamiento
                if (keyboardState.IsKeyDown(Keys.LeftShift)) // LShift para Frenar
                    u*=2;
                Vector3 Friction = new Vector3(Velocity.X, 0, Velocity.Z) * u * dTime;
                Velocity += Friction;
            }
            else {
                Velocity += Gravity;
            }
            
            // SALTO
            if (keyboardState.IsKeyDown(Keys.Space) && Position.Y==floor)
                Velocity += Vector3.Up * JumpPower * dTime;

            Velocity += acceleration * dTime;
            Position += Velocity * dTime;
            
            // MATRIZ DE MUNDO
            World = 
                Matrix.CreateScale(Escala) * 
                MatrixRotation *
                Matrix.CreateTranslation(Position);
        }

        public void Teleport(Vector3 toXYZ){
            Position = toXYZ;
        }
    }
}
